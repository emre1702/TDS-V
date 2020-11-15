import { SettingsIngameColorsIndex } from '../enums/settings-ingame-colors-index.enum';

export interface UserpanelSettingsIngameColors {
    [SettingsIngameColorsIndex.MapBorderColor]: string;
    [SettingsIngameColorsIndex.NametagDeadColor]?: string;
    [SettingsIngameColorsIndex.NametagHealthEmptyColor]: string;
    [SettingsIngameColorsIndex.NametagHealthFullColor]: string;
    [SettingsIngameColorsIndex.NametagArmorEmptyColor]?: string;
    [SettingsIngameColorsIndex.NametagArmorFullColor]: string;
}