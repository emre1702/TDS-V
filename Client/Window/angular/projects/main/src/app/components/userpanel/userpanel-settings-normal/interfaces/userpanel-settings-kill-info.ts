import { SettingsKillInfoIndex } from '../enums/settings-kill-info-index.enum';

export interface UserpanelSettingsKillInfo {
    [SettingsKillInfoIndex.KillInfoShowIcon]: boolean;
    [SettingsKillInfoIndex.KillInfoFontSize]: number;
    [SettingsKillInfoIndex.KillInfoIconWidth]: number;
    [SettingsKillInfoIndex.KillInfoSpacing]: number;
    [SettingsKillInfoIndex.KillInfoDuration]: number;
    [SettingsKillInfoIndex.KillInfoIconHeight]: number;
}