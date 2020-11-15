import { ScoreboardPlayerSorting } from '../../enums/scoreboard-player-sorting';
import { TimeSpanUnitsOfTime } from '../../enums/timespan-units-of-time.enum';
import { SettingsScoreboardIndex } from '../enums/settings-scoreboard-index.enum';

export interface UserpanelSettingsScoreboard {
    [SettingsScoreboardIndex.ScoreboardPlayerSorting]: ScoreboardPlayerSorting;
    [SettingsScoreboardIndex.ScoreboardPlayerSortingDesc]: boolean;
    [SettingsScoreboardIndex.ScoreboardPlaytimeUnit]: TimeSpanUnitsOfTime;
}