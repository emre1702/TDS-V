using RAGE.Ui;
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
            browser.ExecuteJs($"openMapMenu('{Settings.MyLanguage}', '{mapslistjson}');");
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
    }
}
