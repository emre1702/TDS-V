using System;
using RAGE.Elements;
using TDS_Client.Enum;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Shared.Default;
using TDS_Shared.Enum;

namespace TDS_Client.Manager.Lobby
{
    internal static class MapManager
    {
        private static bool _open;
        private static int _lobbyIdAtLastLoad;
        private static string _lastMapsJson;
        private static bool _mapBuyDataSynced;

        public static void Init()
        {
            PlayerDataSync.OnDataChanged += OnMapsBoughtCounterChanged;
        }

        public static void ToggleMenu(EKey _)
        {
            if (Browser.Angular.Shared.InInput)
                return;

            if (!_open)
            {
                if (!Settings.InLobbyWithMaps)
                    return;
                OpenMenu();
            }
            else
            {
                CloseMenu();
            }
        }

        private static void OpenMenu()
        {
            CursorManager.Visible = true;
            _open = true;

            if (_lobbyIdAtLastLoad == Settings.LobbyId)
            {
                Browser.Angular.Main.OpenMapMenu(_lastMapsJson);
                return;
            }

            EventsSender.Send(DToServerEvent.MapsListRequest);
        }

        public static void CloseMenu(bool sendToBrowser = true)
        {
            if (!_open)
                return;
            _open = false;
            if (sendToBrowser)
                Browser.Angular.Main.CloseMapMenu();
            CursorManager.Visible = false;
        }

        public static void LoadMapList(string mapjson)
        {
            _lastMapsJson = mapjson;
            _lobbyIdAtLastLoad = Settings.LobbyId;
            if (!_mapBuyDataSynced)
            {
                OnMapsBoughtCounterChanged(Player.LocalPlayer, PlayerDataKey.MapsBoughtCounter, PlayerDataSync.GetData(PlayerDataKey.MapsBoughtCounter, 1));
            }
            Browser.Angular.Main.OpenMapMenu(mapjson);
        }

        private static void OnMapsBoughtCounterChanged(Player player, PlayerDataKey key, object data)
        {
            if (player != Player.LocalPlayer)
                return;
            if (key != PlayerDataKey.MapsBoughtCounter)
                return;
            _mapBuyDataSynced = true;
            Browser.Angular.Main.SyncMapPriceData((int)data);
        }

    }
}
