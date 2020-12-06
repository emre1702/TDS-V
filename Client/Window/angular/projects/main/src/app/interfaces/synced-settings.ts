import { HudDesign } from '../components/hud/enums/hud-design.enum';

export interface SyncedSettings {
    /** ChatWidth */
    0: number;

    /** ChatMaxHeight */
    1: number;

    /** ChatFontSize */
    2: number;

    /** ChatHideDirtyChat */
    3: boolean;

    /** ChatInfoHide */
    4: boolean;

    /** ChatInfoFontSize */
    5: number;

    /** ChatInfoAnimationTimeMs */
    6: number;

    /** KillInfoShowIcon */
    7: boolean;

    /** KillInfoFontSize */
    8: number;

    /** KillInfoIconWidth */
    9: number;

    /** KillInfoSpacing */
    10: number;

    /** KillInfoDuration */
    11: number;

    /** KillInfoIconHeight */
    12: number;

    /** UseDarkTheme */
    13: boolean;

    /** ThemeMainColor */
    14: string;

    /** ThemeSecondaryColor */
    15: string;

    /** ThemeWarnColor */
    16: string;

    /** ThemeBackgroundDarkColor */
    17: string;

    /** ThemeBackgroundLightColor */
    18: string;

    /** ToolbarDesign */
    19: number;

    /** HudDesign */
    20: HudDesign;

    /** HudBackgroundColor */
    21: string;

    /** RoundStatsBackgroundColor */
    22: string;

    /** HudFontColor */
    23: string;

    /** RoundStatsFontColor */
    24: string;
}