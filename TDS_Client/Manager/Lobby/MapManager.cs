using System;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Client.Manager.Lobby
{
    internal static class MapManager
    {
        private static bool open;
        private static int lobbyIdAtLastLoad;
        private static string lastMapsJson;

        public static void ToggleMenu(ConsoleKey _)
        {
            if (ChatManager.IsOpen)
                return;

            if (!open)
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
            open = true;

            if (lobbyIdAtLastLoad == Settings.LobbyId)
            {
                Angular.OpenMapMenu(lastMapsJson);
                return;
            }

            EventsSender.Send(DToServerEvent.MapsListRequest);
        }

        public static void CloseMenu(bool sendToBrowser = true)
        {
            if (!open)
                return;
            open = false;
            if (sendToBrowser)
                Angular.CloseMapMenu();
            CursorManager.Visible = false;
        }

        public static void LoadMapList(string mapjson)
        {
            lastMapsJson = mapjson;
            lobbyIdAtLastLoad = Settings.LobbyId;
            Angular.OpenMapMenu(mapjson);
        }
    }
}