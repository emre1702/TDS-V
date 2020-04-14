using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Lobby
{
    public class MapManagerHandler
    {
        private bool _open;
        private int _lobbyIdAtLastLoad;
        private string _lastMapsJson;
        private bool _mapBuyDataSynced;

        private readonly IModAPI ModAPI;
        private readonly BrowserHandler _browserHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly BindsHandler _bindsHandler;

        public MapManagerHandler(EventsHandler eventsHandler, IModAPI modAPI, BrowserHandler browserHandler, SettingsHandler settingsHandler, CursorHandler cursorHandler,
            RemoteEventsSender remoteEventsSender, DataSyncHandler dataSyncHandler, BindsHandler bindsHandler)
        {
            ModAPI = modAPI;
            _browserHandler = browserHandler;
            _settingsHandler = settingsHandler;
            _cursorHandler = cursorHandler;
            _remoteEventsSender = remoteEventsSender;
            _dataSyncHandler = dataSyncHandler;
            _bindsHandler = bindsHandler;

            eventsHandler.DataChanged += OnMapsBoughtCounterChanged;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.LoggedIn += EventsHandler_LoggedIn;

            modAPI.Event.Add(FromBrowserEvent.CloseMapVotingMenu, _ => CloseMenu(false));
            modAPI.Event.Add(ToClientEvent.MapsListRequest, OnMapListRequestMethod);
        }

        public void ToggleMenu(Key _)
        {
            if (_browserHandler.InInput)
                return;

            if (!_open)
            {
                if (!_settingsHandler.InLobbyWithMaps)
                    return;
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }

        private void OpenMenu()
        {
            _cursorHandler.Visible = true;
            _open = true;

            if (_lobbyIdAtLastLoad == _settingsHandler.LobbyId)
            {
                _browserHandler.Angular.OpenMapMenu(_lastMapsJson);
                return;
            }

            _remoteEventsSender.Send(ToServerEvent.MapsListRequest);
        }

        public void CloseMenu(bool sendToBrowser = true)
        {
            if (!_open)
                return;
            _open = false;
            if (sendToBrowser)
                _browserHandler.Angular.CloseMapMenu();
            _cursorHandler.Visible = false;
        }

        public void LoadMapList(string mapjson)
        {
            _lastMapsJson = mapjson;
            _lobbyIdAtLastLoad = _settingsHandler.LobbyId;
            if (!_mapBuyDataSynced)
            {
                OnMapsBoughtCounterChanged(ModAPI.LocalPlayer, PlayerDataKey.MapsBoughtCounter, _dataSyncHandler.GetData(PlayerDataKey.MapsBoughtCounter, 1));
            }
            _browserHandler.Angular.OpenMapMenu(mapjson);
        }

        private void OnMapsBoughtCounterChanged(IPlayer player, PlayerDataKey key, object data)
        {
            if (player != ModAPI.LocalPlayer)
                return;
            if (key != PlayerDataKey.MapsBoughtCounter)
                return;
            _mapBuyDataSynced = true;
            _browserHandler.Angular.SyncMapPriceData((int)data);
        }

        private void EventsHandler_LoggedIn()
        {
            _bindsHandler.Add(Key.F3, ToggleMenu);
        }

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            CloseMenu();
        }

        private void OnMapListRequestMethod(object[] args)
        {
            LoadMapList((string)args[0]);
        }
    }
}
