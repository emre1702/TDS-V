using Newtonsoft.Json;
using RAGE.Ui;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;

namespace TDS_Client.Manager.Browser
{
    static class MainBrowser
    {
        private static HtmlWindow browser;
        private static bool RoundEndReasonShowing;

        public static void Load()
        {
            browser = new HtmlWindow("package://TDS-V/window/main/index.html");
            browser.MarkAsChat();
        }

        #region Events
        public static void OnLoadOwnMapRatings(string datajson)
        {
            browser.ExecuteJs($"loadMyMapRatings(`{datajson}`);");
        }

        public static void OnSendMapRating(string currentmap, int rating)
        {
            RAGE.Events.CallRemote(DToServerEvent.AddRatingToMap, currentmap, rating);
        }
        #endregion

        public static void ShowBloodscreen()
        {
            browser.ExecuteJs("showBloodscreen();");
        }

        public static void PlaySound(string soundname)
        {
            browser.ExecuteJs($"playSound({soundname})");
        }

        public static void PlayHitsound()
        {
            browser.ExecuteJs("playHitsound();");
        }

        public static void AddKillMessage(string msg)
        {
            browser.ExecuteJs($"addKillMessage('{msg}');");
        }

        public static void SendAlert(string msg)
        {
            browser.ExecuteJs($"alert('{msg}');");
        }

        public static void OpenMapMenuInBrowser(string mapslistjson)
        {
            browser.ExecuteJs($"openMapMenu('{Settings.Language.Enum}', '{mapslistjson}');");
        }

        public static void CloseMapMenuInBrowser()
        {
            browser.ExecuteJs("closeMapMenu();");
        }

        public static void LoadMapVotingsForMapBrowser(string mapvotesjson)
        {
            browser.ExecuteJs($"loadMapVotings('{mapvotesjson}');");
        }
        
        public static void ClearMapVotingsInBrowser()
        {
            browser.ExecuteJs("clearMapVotings();");
        }

        public static void AddVoteToMapInMapMenuBrowser(string mapname, string oldvotemapname)
        {
            browser.ExecuteJs($"addVoteToMapVoting('{mapname}', '{oldvotemapname}');");
        }

        public static void LoadMapFavouritesInBrowser(string mapfavouritesjson)
        {
            browser.ExecuteJs($"loadFavouriteMaps('{mapfavouritesjson}');");
        }

        public static void ToggleCanVoteForMapWithNumpadInBrowser(bool canvote)
        {
            browser.ExecuteJs($"toggleCanVoteForMapWithNumpad({(canvote ? 1 : 0)});");
        }

        public static void LoadOrderNamesInBrowser(string ordernamesjson)
        {
            browser.ExecuteJs($"loadOrderNames('{ordernamesjson}');");
        }

        public static void ShowRoundEndReason(string reason, string currentmap)
        {
            RoundEndReasonShowing = true;
            browser.ExecuteJs($"showRoundEndReason(`{reason}`, `{currentmap}`);");
        }

        public static void HideRoundEndReason()
        {
            if (RoundEndReasonShowing)
            {
                browser.ExecuteJs("hideRoundEndReason();");
                RoundEndReasonShowing = false;
            }
        }

        public static void LoadPlayersForChat(List<RAGE.Elements.Player> players)
        {
            IEnumerable<string> names = players.Select(p => p.Name);
            browser.ExecuteJs($"loadNamesForChat(`{JsonConvert.SerializeObject(names)}`)");
        }

        public static void AddPlayerForChat(RAGE.Elements.Player player)
        {
            browser.ExecuteJs($"addNameForChat(`{player.Name}`)");
        }

        public static void RemovePlayerForChat(RAGE.Elements.Player player)
        {
            browser.ExecuteJs($"removeNameForChat(`{player.Name}`)");
        }
    }
}
