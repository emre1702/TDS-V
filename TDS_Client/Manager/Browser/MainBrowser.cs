using Newtonsoft.Json;
using RAGE.Ui;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Manager.Account;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Browser
{
    static class MainBrowser
    {
        public static HtmlWindow Browser { get; set; }
        private static bool roundEndReasonShowing;

        public static void Load()
        {
            Browser = new HtmlWindow(Constants.MainBrowserPath);
            Browser.MarkAsChat();
        }

        #region Events
        public static void OnLoadOwnMapRatings(string datajson)
        {
            Browser.ExecuteJs($"loadMyMapRatings(`{datajson}`);");
        }

        public static void OnSendMapRating(string currentmap, int rating)
        {
            EventsSender.Send(DToServerEvent.AddRatingToMap, currentmap, rating);
        }
        #endregion

        public static void ShowBloodscreen()
        {
            Browser.ExecuteJs("showBloodscreen();");
        }

        public static void PlaySound(string soundname)
        {
            Browser.ExecuteJs($"playSound('{soundname}')");
        }

        public static void PlayHitsound()
        {
            Browser.ExecuteJs("playHitsound();");
        }

        public static void AddKillMessage(string msg)
        {
            Browser.ExecuteJs($"addKillMessage('{msg}');");
        }

        public static void SendAlert(string msg)
        {
            Browser.ExecuteJs($"alert('{msg}');");
        }

        public static void OpenMapMenuInBrowser(string mapslistjson)
        {
            Browser.ExecuteJs($"openMapMenu('{(int)Settings.Language.Enum}', '{mapslistjson}');");
        }

        public static void CloseMapMenuInBrowser()
        {
            Browser.ExecuteJs("closeMapMenu();");
        }

        public static void LoadMapVotingsForMapBrowser(string mapvotesjson)
        {
            Browser.ExecuteJs($"loadMapVotings('{mapvotesjson}');");
        }
        
        public static void ClearMapVotingsInBrowser()
        {
            Browser.ExecuteJs("clearMapVotings();");
        }

        public static void AddVoteToMapInMapMenuBrowser(string mapname, string oldvotemapname)
        {
            Browser.ExecuteJs($"addVoteToMapVoting('{mapname}', '{oldvotemapname}');");
        }

        public static void LoadMapFavouritesInBrowser(string mapfavouritesjson)
        {
            Browser.ExecuteJs($"loadFavouriteMaps('{mapfavouritesjson}');");
        }

        public static void ToggleCanVoteForMapWithNumpadInBrowser(bool canvote)
        {
            Browser.ExecuteJs($"toggleCanVoteForMapWithNumpad({canvote});");
        }

        public static void LoadOrderNamesInBrowser(string ordernamesjson)
        {
            Browser.ExecuteJs($"loadOrderNames('{ordernamesjson}');");
        }

        public static void ToggleOrders(bool show)
        {
            Browser.ExecuteJs($"toggleOrders({show})");
        }

        public static void ShowRoundEndReason(string reason, string currentmap)
        {
            roundEndReasonShowing = true;
            Browser.ExecuteJs($"showRoundEndReason(`{reason}`, `{currentmap}`);");
        }

        public static void HideRoundEndReason()
        {
            if (!roundEndReasonShowing)
                return;
            Browser.ExecuteJs("hideRoundEndReason();");
            roundEndReasonShowing = false;
        }

        public static void LoadPlayersForChat(List<Player> players)
        {
            IEnumerable<string> names = players.Select(p => p.Name);
            Browser.ExecuteJs($"loadNamesForChat(`{JsonConvert.SerializeObject(names)}`)");
        }

        public static void AddPlayerForChat(Player player)
        {
            Browser.ExecuteJs($"addNameForChat(`{player.Name}`)");
        }

        public static void RemovePlayerForChat(Player player)
        {
            Browser.ExecuteJs($"removeNameForChat(`{player.Name}`)");
        }

        public static void LoadUserName()
        {
            Browser.ExecuteJs($"loadUserName('{Player.LocalPlayer.Name}')");
        }

        public static void StartBombTick(uint msToDetonate, uint startAtMs)
        {
            Browser.ExecuteJs($"startBombTickSound({msToDetonate}, {startAtMs})");
        }

        public static void StopBombTick()
        {
            Browser.ExecuteJs("stopBombTickSound()");
        }
    }
}
