using RAGE.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Manager.Utility;
using TDS_Shared.Default;
using TDS_Shared.Manager.Utility;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Browser
{
    internal static class MainBrowser
    {
        public static HtmlWindow Browser { get; set; }
        private static bool _roundEndReasonShowing;
        private readonly static Queue<Action> _executeQueue = new Queue<Action>();

        public static void Load()
        {
            Browser = new HtmlWindow(ClientConstants.MainBrowserPath);
            OnLoaded();
        }

        private static void Execute(string execStr)
        {
            if (Browser == null)
                _executeQueue.Enqueue(() => Browser.ExecuteJs(execStr));
            else
                Browser.ExecuteJs(execStr);
        }

        private static void Call(string eventName, params object[] args)
        {
            if (Browser == null)
                _executeQueue.Enqueue(() => Browser.Call(eventName, args));
            else
                Browser.Call(eventName, args);
        }

        #region Events

        private static void OnLoaded()
        {
            foreach (var exec in _executeQueue)
            {
                exec();
            }
            _executeQueue.Clear();
        }

        public static void OnLoadOwnMapRatings(string datajson)
        {
            Execute($"loadMyMapRatings(`{datajson}`);");
        }

        #endregion Events

        public static void ShowBloodscreen()
        {
            Call("c");
        }

        public static void PlaySound(string soundname)
        {
            Call("a", soundname);
        }

        public static void PlayHitsound()
        {
            Call("b");
        }

        public static void AddKillMessage(string msg)
        {
            Call("d", msg);
        }

        public static void SendAlert(string msg)
        {
            Execute($"alert('{msg}');");
        }

        public static void ShowRoundEndReason(string reason, int mapId)
        {
            _roundEndReasonShowing = true;
            Execute($"showRoundEndReason(`{reason}`, {mapId});");
        }

        public static void HideRoundEndReason()
        {
            if (!_roundEndReasonShowing)
                return;
            Execute("hideRoundEndReason();");
            _roundEndReasonShowing = false;
        }

        public static void StartBombTick(int msToDetonate, int startAtMs)
        {
            Execute($"startBombTickSound({msToDetonate}, {startAtMs})");
        }

        public static void StopBombTick()
        {
            Execute("stopBombTickSound()");
        }

        public static void StartPlayerTalking(string name)
        {
            Call("e", name);
        }

        public static void StopPlayerTalking(string name)
        {
            Call("f", name);
        }
    }
}
