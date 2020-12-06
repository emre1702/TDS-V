import { SettingsHudIndex } from '../enums/settings-hud-index.enum';
import { HudDesign } from '../../../hud/enums/hud-design.enum';

export interface UserpanelSettingsHud {
    /** HudDesign */
    [SettingsHudIndex.HudDesign]: HudDesign;
    /** HudBackgroundColor */
    [SettingsHudIndex.HudBackgroundColor]: string;
    /** RoundStatsBackgroundColor */
    [SettingsHudIndex.RoundStatsBackgroundColor]: string;
    /** HudFontColor */
    [SettingsHudIndex.HudFontColor]: string;
    /** RoundStatsFontColor */
    [SettingsHudIndex.RoundStatsFontColor]: string;
}