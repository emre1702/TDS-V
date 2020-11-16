using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Events;
using TDS.Client.Handler.Sync;
using TDS.Shared.Data.Enums;
using TDS.Shared.Data.Models;
using TDS.Shared.Default;

namespace TDS.Client.Handler.Lobby
{
    public class MapManagerHandler
    {
        private readonly BindsHandler _bindsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly CursorHandler _cursorHandler;
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly RemoteEventsSender _remoteEventsSender;
        private readonly SettingsHandler _settingsHandler;

        private string _lastMapsJson;
        private int _lobbyIdAtLastLoad;
        private bool _mapBuyDataSynced;
        private bool _open;

        public MapManagerHandler(EventsHandler eventsHandler, BrowserHandler browserHandler, SettingsHandler settingsHandler, CursorHandler cursorHandler,
            RemoteEventsSender remoteEventsSender, DataSyncHandler dataSyncHandler, BindsHandler bindsHandler)
        {
            _browserHandler = browserHandler;
            _settingsHandler = settingsHandler;
            _cursorHandler = cursorHandler;
            _remoteEventsSender = remoteEventsSender;
            _dataSyncHandler = dataSyncHandler;
            _bindsHandler = bindsHandler;

            eventsHandler.DataChanged += OnMapsBoughtCounterChanged;
            eventsHandler.LobbyLeft += EventsHandler_LobbyLeft;
            eventsHandler.LoggedIn += EventsHandler_LoggedIn;

            RAGE.Events.Add(FromBrowserEvent.CloseMapVotingMenu, _ => CloseMenu(false));
            RAGE.Events.Add(ToClientEvent.MapsListRequest, OnMapListRequestMethod);
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
            if (_settingsHandler is null || !_settingsHandler.InLobbyWithMaps)
                return;

            _lastMapsJson = mapjson;
            _lobbyIdAtLastLoad = _settingsHandler.LobbyId;
            if (!_mapBuyDataSynced)
            {
                OnMapsBoughtCounterChanged(RAGE.Elements.Player.LocalPlayer, PlayerDataKey.MapsBoughtCounter, _dataSyncHandler.GetData(PlayerDataKey.MapsBoughtCounter, 1));
            }
            _browserHandler.Angular.OpenMapMenu(mapjson);
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

        private void EventsHandler_LobbyLeft(SyncedLobbySettings settings)
        {
            CloseMenu();
        }

        private void EventsHandler_LoggedIn()
        {
            _bindsHandler.Add(Key.F3, ToggleMenu);
        }

        private void OnMapListRequestMethod(object[] args)
        {
            LoadMapList((string)args[0]);
        }

        private void OnMapsBoughtCounterChanged(RAGE.Elements.Player player, PlayerDataKey key, object data)
        {
            if (player != RAGE.Elements.Player.LocalPlayer)
                return;
            if (key != PlayerDataKey.MapsBoughtCounter)
                return;
            _mapBuyDataSynced = true;
            _browserHandler.Angular.SyncMapPriceData((int)data);
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
    }
}
