import { LanguageEnum } from '../../../enums/language.enum';
import { TimeSpanUnitsOfTime } from '../enums/timespan-units-of-time.enum';
import { ScoreboardPlayerSorting } from '../enums/scoreboard-player-sorting';

export interface UserpanelSettingNormalDataDto {
    /** PlayerId */
    [0]: number;

    /** Language */
    [1]: LanguageEnum;
    /** AllowDataTransfer */
    [2]: boolean;
    /** ShowConfettiAtRanking */
    [3]: boolean;
    /** Timezone */
    [4]: string;
    /** DateTimeFormat */
    [13]: string;
    /** DiscordUserId */
    [5]: number;
    /** CheckAFK */
    [25]: boolean;
    /** WindowsNotifications */
    [26]: boolean;

    /** Hitsound */
    [6]: boolean;
    /** Bloodscreen */
    [7]: boolean;
    /** FloatingDamageInfo */
    [8]: boolean;

    /** Voice3D */
    [9]: boolean;
    /** VoiceAutoVolume */
    [10]: boolean;
    /** VoiceVolume */
    [11]: number;

    /** MapBorderColor */
    [12]: string;
    /** NametagDeadColor */
    [20]: string;
    /** NametagHealthEmptyColor */
    [21]: string;
    /** NametagHealthFullColor */
    [22]: string;
    /** NametagArmorEmptyColor */
    [23]?: string;
    /** NametagArmorFullColor */
    [24]: string;

    /** BloodscreenCooldownMs */
    [14]: number;
    /** HudAmmoUpdateCooldownMs */
    [15]: number;
    /** HudHealthUpdateCooldownMs */
    [16]: number;
    /** AFKKickAfterSeconds */
    [17]: number;
    /** AFKKickShowWarningLastSeconds */
    [18]: number;
    /** ShowFloatingDamageInfoDurationMs */
    [19]: number;

    /** ChatWidth */
    [27]: number;
    /** ChatMaxHeight */
    [28]: number;
    /** ChatFontSize */
    [29]: number;
    /** HideDirtyChat */
    [30]: boolean;
    /** ShowCursorOnChatOpen */
    [31]: boolean;
    /** HideChatInfo */
    [35]: boolean;
    /** ChatInfoFontSize */
    [36]: number;
    /** ChatInfoMoveTimeMs */
    [37]: number;

    /** ScoreboardPlayerSorting */
    [32]: ScoreboardPlayerSorting;
    /** ScoreboardPlayerSortingDesc */
    [33]: boolean;
    /** ScoreboardPlaytimeUnit */
    [34]: TimeSpanUnitsOfTime;

    /** UseDarkTheme */
    [38]: boolean;
    /** ThemeBackgroundAlphaPercentage */
    [39]: number;
    /** ThemeMainColor */
    [40]: string;
    /** ThemeSecondaryColor */
    [41]: string;
    /** ThemeWarnColor */
    [42]: string;
    /** ThemeBackgroundDarkColor */
    [43]: string;
    /** ThemeBackgroundLightColor */
    [44]: string;
}
