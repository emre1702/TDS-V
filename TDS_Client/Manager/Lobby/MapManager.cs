using System;
using TDS_Client.Enum;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Client.Manager.Lobby
{
    internal static class MapManager
    {
        private static bool _open;
        private static int _lobbyIdAtLastLoad;
        private static string _lastMapsJson;

        public static void ToggleMenu(EKey _)
        {
            if (ChatManager.IsOpen)
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
                Angular.OpenMapMenu(_lastMapsJson);
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
                Angular.CloseMapMenu();
            CursorManager.Visible = false;
        }

        public static void LoadMapList(string mapjson)
        {
            _lastMapsJson = mapjson;
            _lobbyIdAtLastLoad = Settings.LobbyId;
            Angular.OpenMapMenu(mapjson);
        }
    }
}