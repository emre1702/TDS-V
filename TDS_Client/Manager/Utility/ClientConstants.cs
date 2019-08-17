namespace TDS_Client.Manager.Utility
{
    internal class ClientConstants
    {
        public static string LobbyChoiceBrowserPath => "package://Window/choice/index.html";
        public static string MainBrowserPath => "package://Window/main/index.html";
        public static string AngularBrowserPath => "package://Window/mainNew/index.html";
        public static string RegisterLoginBrowserPath => "package://Window/registerlogin/index.html";
        public static int ScoreboardLoadCooldown => 5000;
        public static ulong ShowFloatingDamageInfoMs => 1000;
        public static uint MapLimitFasterCheckTimeMs => 100;
        public static int DefaultSpectatePlayerChangeEaseTime => 1500;
    }
}