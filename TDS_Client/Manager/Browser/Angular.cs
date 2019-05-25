using Newtonsoft.Json;
using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Default;
using TDS_Client.Manager.Utility;
using TDS_Common.Enum;

namespace TDS_Client.Manager.Browser
{
    class Angular
    {
        public static HtmlWindow Browser { get; set; }
        private readonly static Queue<string> _executeQueue = new Queue<string>();

        public static void Load()
        {
            Browser = new HtmlWindow(Constants.AngularBrowserPath);
            OnLoaded();
        }

        private static void Execute(string eventName, params object[] args)
        {
            string execStr = GetExecStr(eventName, args);
            if (Browser == null)
                _executeQueue.Enqueue(execStr);
            else
                Browser.ExecuteJs(execStr);
        }

        private static string GetExecStr(string eventName, params object[] args)
        {
            var strBuilder = new StringBuilder($"RageAngularEvent(`{eventName}`");
            foreach (var arg in args)
            {
                if (arg is string)
                    strBuilder.Append($", `{arg}`");
                else if (!arg.GetType().IsValueType)
                    strBuilder.Append($", `{JsonConvert.SerializeObject(arg)}`");
                else if (arg is char)
                    strBuilder.Append($", '{arg}'");
                else if (arg is bool)
                    strBuilder.Append($", {((bool)arg ? 1 : 0)}");
                else
                    strBuilder.Append($", {arg.ToString()}");
            }

            strBuilder.Append(")");

            return strBuilder.ToString();
        }

        private static void OnLoaded()
        {
            foreach (var execStr in _executeQueue)
            {
                Browser.ExecuteJs(execStr);
            }
            _executeQueue.Clear();
        }

        public static void LoadLanguage(ELanguage language)
        {
            Execute(DToBrowserEvent.LoadLanguage, (int)language);
        }

        public static void OpenMapMenu(string mapsListJson)
        {
            Execute(DToBrowserEvent.OpenMapMenu, mapsListJson);
        }

        public static void CloseMapMenu()
        {
            Execute(DToBrowserEvent.CloseMapMenu);
        }

        public static void AddMapToVoting(string mapVoteJson)
        {
            Execute(DToBrowserEvent.AddMapToVoting, mapVoteJson);
        }

        public static void SetMapVotes(int mapId, int amountVotes)
        {
            Execute(DToBrowserEvent.SetMapVotes, mapId, amountVotes);
        }

        public static void LoadMapVoting(string mapVotesJson)
        {
            Execute(DToBrowserEvent.LoadMapVoting, mapVotesJson);
        }

        public static void ResetMapVoting()
        {
            Execute(DToBrowserEvent.ResetMapVoting);
        }

        public static void LoadFavoriteMaps(string mapFavoritesJson)
        {
            Execute(DToBrowserEvent.LoadFavoriteMaps, mapFavoritesJson);
        }

        public static void ToggleTeamOrderModus(bool activated)
        {
            Execute(DToBrowserEvent.ToggleTeamOrderModus, activated);
        }
    }
}
