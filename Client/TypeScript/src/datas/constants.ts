import ToServerEvent from "./enums/events/to-server-event.enum";
import CooldownEventData from "./interfaces/events/cooldown-event-data.interface";
import Sound from "./enums/output/sound.enum";
import PedHash from "./enums/gta/ped-hash.enum";
import LanguageValue from "./enums/output/language-value.enum";
import Language from "./interfaces/output/language.interface";
import German from "./models/output/german.language";
import English from "./models/output/english.language";
import Control from "./enums/input/control.enum";
import InputGroup from "./enums/input/input-group.enum";

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

export const gangHouseFreeBlipAlpha = 170;
export const gangHouseFreeBlipModel = 374;
export const neededDistanceToBeNotAFK = 1;
export const soundPaths: { [key: number]: string } = {
    //todo: Path is propably incorrect, change it for alt:V
    [Sound.Hit]: "http://resource/client/sounds/hit.mp3"
}
export const teamSpawnPedHash: PedHash[] = [
    PedHash.Blackops02SMY, PedHash.ChiCold01GMM, PedHash.Cop01SMY, PedHash.Claude01,
    PedHash.ExArmy01, PedHash.Famca01GMY, PedHash.FibSec01
];
export const angularMainBrowserPath = "http://resource/client/angular/main/index.html";
export const angularMapCreatorObjectChoiceBrowserPath = "http://resource/client/angular/map-creator-object-choice/index.html";
export const angularMapCreatorVehicleChoiceBrowserPath = "http://resource/client/angular/map-creator-vehicle-choice/index.html";
export const bombPlantPlaceHashName = "prop_mp_placement_med";
export const defaultSpectatePlayerChangeEaseTime = 1500;
export const plainHtmlMainBrowserPath = "http://resource/client/plainHtml/main/index.html";
export const mapCenterHashName = "prop_flagpole_1a";
export const mapLimitFasterCheckTimeMs = 100;
export const mapLimitHashName = "prop_flagpole_1a";
export const registerLoginBrowserPath = "http://resource/client/plainHtml/registerlogin/index.html";
export const scoreboardLoadCooldown = 2000;
export const targetHashName = "v_ret_ta_skull";
export const mapObjectBlipRadius = 50;

export class ConstBlipSprites {
    static bombPlantPlace = 433;
    static mapCenter = 629;
    static mapLimit = 441;
    static object = 1;
    static target = 303;
    static teamSpawn = 491;
    static vehicle = 523;
    static gangHouseOccupied = 492;
}

export const screenFadeInTimeAfterSpawn = 2000;
export const screenFadeOutTimeAfterSpawn = 2000;

export const languagesDict: { [key: number]: Language } = {
    [LanguageValue.German]: new German(),
    [LanguageValue.English]: new English()
}
export const serverTeamSuffixMinAdminLevel = 1;
export const serverTeamSuffix = "[TDS]";

export const attackControls: Control[] = [
    Control.Attack, Control.Attack2, Control.MeleeAttackLight, Control.MeleeAttackHeavy, Control.MeleeAttackAlternate, Control.MeleeAttack1, Control.MeleeAttack2
];
export const attackInputGroup = InputGroup.Look;



/*public const string DateTimeOffsetFormat = "dddd, MMM dd yyyy HH:mm:ss zzz";
public const string 

public const string TargetHashName = "v_ret_ta_skull";*/
