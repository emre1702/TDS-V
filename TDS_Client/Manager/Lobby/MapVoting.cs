using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using Events = RAGE.Events;

namespace TDS_Client.Manager.Lobby
{
    static class MapVoting
    {
        private static bool open;
        private static uint lobbyIdAtLastLoad;
#warning reset that when the lobby gets removed (so a new one with same Id won't get used)
        private static string lastMapsJson;
        private static bool loadedFavorites;

        public static void ToggleMenu()
        {
            if (!open)
            {
                if (MainBrowser.IsChatOpen)
                    return;
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
                MainBrowser.OpenMapMenuInBrowser(lastMapsJson);
                return;
            }

            Events.CallRemote(DToServerEvent.MapsListRequest);  
            
            if (!loadedFavorites)
            {
                /*string favoritesJson = mp.storage.data.mapFavorites;
                if(favouritesstr != undefined) {
                    mapvotingdata.favourites = JSON.parse(favouritesstr);
                    loadMapFavouritesInBrowser(favouritesstr);
                } else 
                loadMapFavouritesInBrowser("[]");

                mapvotingdata.favouritesloaded = true;*/
            }
        }

        public static void CloseMenu()
        {
            if (!open)
                return;
            open = false;
            MainBrowser.CloseMapMenuInBrowser();
            CursorManager.Visible = false;
        }

        public static void LoadMapList(string mapjson)
        {
            lastMapsJson = mapjson;
            lobbyIdAtLastLoad = Settings.LobbyId;
            MainBrowser.OpenMapMenuInBrowser(mapjson);
        }

        public static void AddVote(string newmap, string oldmap)
        {
            MainBrowser.AddVoteToMapInMapMenuBrowser(newmap, oldmap);
        }
    }
}
