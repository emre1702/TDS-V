import { LanguageEnum } from '../../../enums/language.enum';

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

    /** Hitsound */
    [6]: boolean;
    /** Bloodscreen */
    [7]: boolean;
    /** FloatingDamageInfo */
    [8]: boolean;
    /** CheckAFK */
    [25]: boolean;

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

}
