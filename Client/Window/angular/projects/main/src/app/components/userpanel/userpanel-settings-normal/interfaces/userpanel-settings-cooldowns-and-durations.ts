import { SettingsCooldownsAndDurationsIndex } from '../enums/settings-cooldowns-and-durations-index.enum';

export interface UserpanelSettingsCooldownsAndDurations {
    /** BloodscreenCooldownMs */
    [SettingsCooldownsAndDurationsIndex.BloodscreenCooldownMs]: number;

    /** HudAmmoUpdateCooldownMs */
    [SettingsCooldownsAndDurationsIndex.HudAmmoUpdateCooldownMs]: number;

    /** HudHealthUpdateCooldownMs */
    [SettingsCooldownsAndDurationsIndex.HudHealthUpdateCooldownMs]: number;

    /** AFKKickAfterSeconds */
    [SettingsCooldownsAndDurationsIndex.AFKKickAfterSeconds]: number;

    /** AFKKickShowWarningLastSeconds */
    [SettingsCooldownsAndDurationsIndex.AFKKickShowWarningLastSeconds]: number;

    /** ShowFloatingDamageInfoDurationMs */
    [SettingsCooldownsAndDurationsIndex.ShowFloatingDamageInfoDurationMs]: number;
}