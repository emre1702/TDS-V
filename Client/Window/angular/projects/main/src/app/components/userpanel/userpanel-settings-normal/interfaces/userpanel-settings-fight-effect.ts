import { SettingsFightEffectIndex } from '../enums/settings-fight-effect-index.enum';

export interface UserpanelSettingsFightEffect {
    /** Bloodscreen */
    [SettingsFightEffectIndex.Bloodscreen]: boolean;

    /** Hitsound */
    [SettingsFightEffectIndex.Hitsound]: boolean;

    /** FloatingDamageInfo */
    [SettingsFightEffectIndex.FloatingDamageInfo]: boolean;
}