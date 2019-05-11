using Newtonsoft.Json;
using RAGE.Ui;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Browser
{
    internal static class MainBrowser
    {
        public static HtmlWindow Browser { get; set; }
        private static bool roundEndReasonShowing;
        private readonly static Queue<string> executeQueue = new Queue<string>();

        public static void Load()
        {
            Browser = new HtmlWindow(Constants.MainBrowserPath);
            Browser.MarkAsChat();
        }

        private static void Execute(string execStr)
        {
            if (Browser == null)
                executeQueue.Enqueue(execStr);
            else
                Browser.ExecuteJs(execStr);
        }

        #region Events

        public static void OnLoaded()
        {
            Team.LoadOrderNames();
            foreach (var execStr in executeQueue)
            {
                Browser.ExecuteJs(execStr);
            }
            executeQueue.Clear();
        }

        public static void OnLoadOwnMapRatings(string datajson)
        {
            Execute($"loadMyMapRatings(`{datajson}`);");
        }

        public static void OnSendMapRating(string currentmap, int rating)
        {
            EventsSender.Send(DToServerEvent.AddRatingToMap, currentmap, rating);
        }

        #endregion Events

        public static void ShowBloodscreen()
        {
            Execute("showBloodscreen();");
        }

        public static void PlaySound(string soundname)
        {
            Execute($"playSound('{soundname}')");
        }

        public static void PlayHitsound()
        {
            Execute("playHitsound();");
        }

        public static void AddKillMessage(string msg)
        {
            Execute($"addKillMessage('{msg}');");
        }

        public static void SendAlert(string msg)
        {
            Execute($"alert('{msg}');");
        }

        public static void OpenMapMenuInBrowser(string mapslistjson)
        {
            Execute($"openMapMenu('{(int)Settings.Language.Enum}', '{mapslistjson}');");
        }

        public static void CloseMapMenuInBrowser()
        {
            Execute("closeMapMenu();");
        }

        public static void LoadMapVotingsForMapBrowser(string mapvotesjson)
        {
            Execute($"loadMapVotings('{mapvotesjson}');");
        }

        public static void ClearMapVotingsInBrowser()
        {
            Execute("clearMapVotings();");
        }

        public static void AddVoteToMapInMapMenuBrowser(string mapname, string oldvotemapname)
        {
            Execute($"addVoteToMapVoting('{mapname}', '{oldvotemapname}');");
        }

        public static void LoadMapFavouritesInBrowser(string mapfavouritesjson)
        {
            Execute($"loadFavouriteMaps('{mapfavouritesjson}');");
        }

        public static void ToggleCanVoteForMapWithNumpadInBrowser(bool canvote)
        {
            Execute($"toggleCanVoteForMapWithNumpad({canvote});");
        }

        public static void LoadOrderNamesInBrowser(string ordernamesjson)
        {
            Execute($"loadOrderNames('{ordernamesjson}');");
        }

        public static void ToggleOrders(bool show)
        {
            Execute($"toggleOrders({show})");
        }

        public static void ShowRoundEndReason(string reason, string currentmap)
        {
            roundEndReasonShowing = true;
            Execute($"showRoundEndReason(`{reason}`, `{currentmap}`);");
        }

        public static void HideRoundEndReason()
        {
            if (!roundEndReasonShowing)
                return;
            Execute("hideRoundEndReason();");
            roundEndReasonShowing = false;
        }

        public static void LoadPlayersForChat(List<Player> players)
        {
            IEnumerable<string> names = players.Select(p => p.Name);
            Execute($"loadNamesForChat(`{JsonConvert.SerializeObject(names)}`)");
        }

        public static void AddPlayerForChat(Player player)
        {
            Execute($"addNameForChat(`{player.Name}`)");
        }

        public static void RemovePlayerForChat(Player player)
        {
            Execute($"removeNameForChat(`{player.Name}`)");
        }

        public static void LoadUserName()
        {
            Execute($"loadUserName('{Player.LocalPlayer.Name}')");
        }

        public static void StartBombTick(int msToDetonate, int startAtMs)
        {
            Execute($"startBombTickSound({msToDetonate}, {startAtMs})");
        }

        public static void StopBombTick()
        {
            Execute("stopBombTickSound()");
        }
    }
}