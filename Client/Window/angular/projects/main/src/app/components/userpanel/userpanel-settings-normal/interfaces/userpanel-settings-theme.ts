import { SettingsThemeIndex } from '../enums/settings-theme-index.enum';

export interface UserpanelSettingsTheme {
    [SettingsThemeIndex.UseDarkTheme]: boolean;
    [SettingsThemeIndex.ThemeMainColor]: string;
    [SettingsThemeIndex.ThemeSecondaryColor]: string;
    [SettingsThemeIndex.ThemeWarnColor]: string;
    [SettingsThemeIndex.ThemeBackgroundDarkColor]: string;
    [SettingsThemeIndex.ThemeBackgroundLightColor]: string;
    [SettingsThemeIndex.ToolbarDesign]: number;
}