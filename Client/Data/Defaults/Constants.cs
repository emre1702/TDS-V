using System.Collections.Generic;
using TDS.Client.Data.Enums;
using TDS.Shared.Data.Enums;

namespace TDS.Client.Data.Defaults
{
    public class Constants
    {
        public static uint GangHouseFreeBlipModel = 374;
        public static float NeededDistanceToBeNotAFK = 1f;

        public static Dictionary<Sound, string> SoundPaths = new Dictionary<Sound, string>()
        {
            [Sound.Hitsound] = "package://sounds/hit.mp3",
        };

        public static PedHash[] TeamSpawnPedHash = new PedHash[]
        {
            PedHash.Blackops02SMY, PedHash.ChiCold01GMM, PedHash.Cop01SMY, PedHash.Claude01,
            PedHash.ExArmy01, PedHash.Famca01GMY, PedHash.FibSec01
         };

        public static string AngularMainBrowserPath => "package://Window/angular/main/index.html";
        public static string AngularMapCreatorObjectChoiceBrowserPath => "package://Window/angular/map-creator-object-choice/index.html";
        public static string AngularMapCreatorVehicleChoiceBrowserPath => "package://Window/angular/map-creator-vehicle-choice/index.html";
        public static string BombPlantPlaceHashName => "prop_mp_placement_med";
        public static int DefaultSpectatePlayerChangeEaseTime => 1500;
        public static string LobbyChoiceBrowserPath => "package://Window/choice/index.html";
        public static string MainBrowserPath => "package://Window/main/index.html";
        public static string MapCenterHashName => "prop_flagpole_1a";
        public static uint MapLimitFasterCheckTimeMs => 100;
        public static string MapLimitHashName => "prop_flagpole_1a";
        public static int MaxPossibleArmor => 16959;
        public static int MaxPossibleHealth => 16959;  //Todo: Find out max health & refresh in Angular
        public static string RegisterLoginBrowserPath => "package://Window/registerlogin/index.html";
        public static int ScoreboardLoadCooldown => 2000;
        public static string TargetHashName => "v_ret_ta_skull";

        public static List<PedHash> DamageTestLobbyPedModels = new List<PedHash>
        {
            PedHash.Abigail, PedHash.Agent14, PedHash.Andreas, PedHash.Michael, PedHash.Trevor, PedHash.Tonya
        };
    }
}
