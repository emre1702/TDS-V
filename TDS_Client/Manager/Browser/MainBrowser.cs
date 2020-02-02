﻿using RAGE.Ui;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Manager.Utility;
using TDS_Common.Default;
using TDS_Common.Manager.Utility;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Browser
{
    internal static class MainBrowser
    {
        public static HtmlWindow Browser { get; set; }
        private static bool _roundEndReasonShowing;
        private readonly static Queue<string> _executeQueue = new Queue<string>();

        public static void Load()
        {
            Browser = new HtmlWindow(ClientConstants.MainBrowserPath);
            OnLoaded();
        }

        private static void Execute(string execStr)
        {
            if (Browser == null)
                _executeQueue.Enqueue(execStr);
            else
                Browser.ExecuteJs(execStr);
        }

        #region Events

        private static void OnLoaded()
        {
            foreach (var execStr in _executeQueue)
            {
                Browser.ExecuteJs(execStr);
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
            Execute($"addPlayerTalking('{name}')");
        }

        public static void StopPlayerTalking(string name)
        {
            Execute($"removePlayerTalking('{name}')");
        }
    }
}
