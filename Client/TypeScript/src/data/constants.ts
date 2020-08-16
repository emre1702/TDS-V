import ToServerEvent from "./enums/events/to-server-event.enum";
import CooldownEventData from "./interfaces/events/cooldown-event-data.interface";
import Sound from "./enums/output/sound.enum";
import alt from "types-client"

export const cooldownEventsDict: { [key: string]: CooldownEventData } = {
    [ToServerEvent.AddRatingToMap]: { cooldownMs: 1000 },
    [ToServerEvent.BuyMap]: { cooldownMs: 2000 },
    [ToServerEvent.CancelCharCreateData]: { cooldownMs: 1000 },
    [ToServerEvent.ChooseTeam]: { cooldownMs: 1000 },
    [ToServerEvent.CommandUsed]: { cooldownMs: 500 },
    [ToServerEvent.CreateCustomLobby]: { cooldownMs: 2000 },
    [ToServerEvent.GetSupportRequestData]: { cooldownMs: 1000 },
    [ToServerEvent.GetVehicle]: { cooldownMs: 2000 },
    [ToServerEvent.JoinLobby]: { cooldownMs: 3000 },
    [ToServerEvent.JoinLobbyWithPassword]: { cooldownMs: 1000 },
    [ToServerEvent.LanguageChange]: { cooldownMs: 500 },
    [ToServerEvent.LeaveLobby]: { cooldownMs: 500 },
    [ToServerEvent.LoadApplicationDataForAdmin]: { cooldownMs: 3000 },
    [ToServerEvent.LobbyChatMessage]: { cooldownMs: 250 },
    [ToServerEvent.LoadMapForMapCreator]: { cooldownMs: 10000 },
    [ToServerEvent.LoadMapNamesToLoadForMapCreator]: { cooldownMs: 10000 },
    [ToServerEvent.LoadUserpanelData]: { cooldownMs: 1000 },
    [ToServerEvent.MapsListRequest]: { cooldownMs: 1000 },
    [ToServerEvent.MapVote]: { cooldownMs: 500 },
    [ToServerEvent.RequestPlayersForScoreboard]: { cooldownMs: 5000 },
    [ToServerEvent.SaveCharCreateData]: { cooldownMs: 10000 },
    [ToServerEvent.SaveMapCreatorData]: { cooldownMs: 10000 },
    [ToServerEvent.SaveSettings]: { cooldownMs: 3000 },
    [ToServerEvent.SendApplication]: { cooldownMs: 3000 },
    [ToServerEvent.SendApplicationInvite]: { cooldownMs: 5000 },
    [ToServerEvent.SendMapCreatorData]: { cooldownMs: 10000 },
    [ToServerEvent.SendMapRating]: { cooldownMs: 2000 },
    [ToServerEvent.SendSupportRequest]: { cooldownMs: 2000 },
    [ToServerEvent.SendSupportRequestMessage]: { cooldownMs: 300 },
    [ToServerEvent.SendTeamOrder]: { cooldownMs: 2000 },
    [ToServerEvent.ToggleMapFavouriteState]: { cooldownMs: 500 },
    [ToServerEvent.TryLogin]: { cooldownMs: 4000 },
    [ToServerEvent.TryRegister]: { cooldownMs: 4000 }
}

export const gangHouseFreeBlipModel = 374;
export const neededDistanceToBeNotAFK = 1;
export const soundPaths: { [key: number]: string } = {
    //todo: Path is propably incorrect, change it for alt:V
    [Sound.Hit]: "package://sounds/hit.mp3"
}




        public static PedHash[] TeamSpawnPedHash = new PedHash[]
{
    PedHash.Blackops02SMY, PedHash.ChiCold01GMM, PedHash.Cop01SMY, PedHash.Claude01,
        PedHash.ExArmy01, PedHash.Famca01GMY, PedHash.FibSec01
};

        #endregion Public Fields

        #region Public Properties

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
        public static string RegisterLoginBrowserPath => "package://Window/registerlogin/index.html";
        public static int ScoreboardLoadCooldown => 2000;
        public static string TargetHashName => "v_ret_ta_skull";
