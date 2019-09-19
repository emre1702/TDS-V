import { LanguageEnum } from '../../../enums/language.enum';

export interface UserpanelSettingDataDto {
    PlayerId: number;
    Language: LanguageEnum;
    Hitsound: boolean;
    Bloodscreen: boolean;
    FloatingDamageInfo: boolean;
    AllowDataTransfer: boolean;
    Voice3D: boolean;
    VoiceAutoVolume: boolean;
    VoiceVolume: number;
}
