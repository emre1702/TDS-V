using Newtonsoft.Json;
using Player = RAGE.Elements.Player;
using RAGE.Ui;
using System.Collections.Generic;
using System.Text;
using TDS_Client.Default;
using TDS_Client.Manager.Utility;
using TDS_Common.Enum;
using System.Globalization;
using System;

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
                else if (arg is bool b)
                    strBuilder.Append($", {(b ? 1 : 0)}");
                else if (arg is float f)
                {
                    f = (float) Math.Floor(f * 100) / 100;
                    strBuilder.Append($", {f.ToString(CultureInfo.InvariantCulture)}");
                }
                else if (arg is double d)
                {
                    d = Math.Floor(d * 100) / 100;
                    strBuilder.Append($", {d.ToString(CultureInfo.InvariantCulture)}");
                }
                else if (arg is decimal de)
                {
                    de = Math.Floor(de * 100)/100;
                    strBuilder.Append($", {de.ToString(CultureInfo.InvariantCulture)}");
                }
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

        public static void SendCurrentPositionRotation()
        {
            var pos = Player.LocalPlayer.Position;
            var rot = Player.LocalPlayer.GetHeading();
            Execute(DFromBrowserEvent.GetCurrentPositionRotation, pos.X, pos.Y, pos.Z, rot);
        }

        public static void ToggleTeamOrderModus(bool activated)
        {
            Execute(DToBrowserEvent.ToggleTeamOrderModus, activated);
        }

        public static void ToggleChatOpened(bool activated)
        {
            Execute(DToBrowserEvent.ToggleChatOpened, activated);
        }

        public static void ToggleFreeroam(bool activated)
        {
            Execute(DToBrowserEvent.ToggleFreeroam, activated);
        }

        public static void ToggleMapCreator(bool activated)
        {
            Execute(DToBrowserEvent.ToggleMapCreator, activated);
        }

        public static void SendMapCreatorReturn(int err)
        {
            Execute(DFromBrowserEvent.SendMapCreatorData, err);
        }

        public static void SaveMapCreatorReturn(int err)
        {
            Execute(DFromBrowserEvent.SaveMapCreatorData, err);
        }

        public static void LoadMySavedMapNames(string json)
        {
            Execute(DFromBrowserEvent.LoadMySavedMapNames, json);
        }

        public static void LoadMySavedMap(string json)
        {
            Execute(DFromBrowserEvent.LoadMySavedMap, json);
        }
    }

}
