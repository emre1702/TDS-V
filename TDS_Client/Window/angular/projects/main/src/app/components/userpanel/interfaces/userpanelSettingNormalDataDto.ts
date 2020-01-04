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
    /** DiscordUserId */
    [5]: number;

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

    /** DateTimeFormat */
    [13]: string;
}
