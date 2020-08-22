import ScoreboardPlayerSorting from "../../enums/draw/scoreboard-player-sorting.enum";
import TimeSpanUnitsOfTime from "../../enums/data/time-span-units-of-time.enum";
import PlayerAngularChatSettings from "./player-angular-chat-settings.interface";
import LanguageValue from "../../enums/output/language-value.enum";

export default interface PlayerSettings {
    PlayerId: number;
    Language: LanguageValue;
    AllowDataTransfer: boolean;
    ShowConfettiAtRanking: boolean;
    Timezone: string;
    DiscordUserId?: number;
    Hitsound: boolean;
    Bloodscreen: boolean;
    FloatingDamageInfo: boolean;
    Voice3D: boolean;
    VoiceAutoVolume: boolean;
    VoiceVolume: number;
    MapBorderColor: string;
    DateTimeFormat: string;
    BloodscreenCooldownMs: number;
    HudAmmoUpdateCooldownMs: number;
    HudHealthUpdateCooldownMs: number;
    AFKKickAfterSeconds: number;
    AFKKickShowWarningLastSeconds: number;
    ShowFloatingDamageInfoDurationMs: number;
    NametagDeadColor: string;
    NametagHealthEmptyColor: string;
    NametagHealthFullColor: string;
    NametagArmorEmptyColor: string; 
    NametagArmorFullColor: string;
    CheckAFK: boolean;
    WindowsNotifications: boolean;
    ShowCursorOnChatOpen: boolean;
    ScoreboardPlayerSorting: ScoreboardPlayerSorting;
    ScoreboardPlayerSortingDesc: boolean;
    ScoreboardPlaytimeUnit: TimeSpanUnitsOfTime;
    ShowCursorInfo: boolean;
    ShowLobbyLeaveInfo: boolean;

    Chat: PlayerAngularChatSettings;
}
