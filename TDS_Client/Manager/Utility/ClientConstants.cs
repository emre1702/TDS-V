using RAGE.Game;

namespace TDS_Client.Manager.Utility
{
    internal class ClientConstants
    {
        public static string LobbyChoiceBrowserPath => "package://Window/choice/index.html";
        public static string MainBrowserPath => "package://Window/main/index.html";
        public static string AngularBrowserPath => "package://Window/mainNew/index.html";
        public static string RegisterLoginBrowserPath => "package://Window/registerlogin/index.html";
        public static int ScoreboardLoadCooldown => 5000;
        public static ulong ShowFloatingDamageInfoMs = 1000;
        public static uint MapLimitFasterCheckTimeMs => 100;
        public static int DefaultSpectatePlayerChangeEaseTime => 1500;
        public const int NametagAlpha = 255;
        public static uint MapCenterHash => Misc.GetHashKey("prop_flagpole_1a");
        public static uint MapLimitHash => Misc.GetHashKey("prop_flagpole_1a"); 
        public static uint BombPlantPlaceHash => Misc.GetHashKey("prop_mp_placement_med");
        public static uint[] TeamSpawnPedHash = new uint[] { 0x7A05FA59, 0x106D9A99, 0x5E3DA4A4, 0xC0F371B7, 0x45348DBB, 0xE83B93B7, 0x5CDEF405 };
    }
}