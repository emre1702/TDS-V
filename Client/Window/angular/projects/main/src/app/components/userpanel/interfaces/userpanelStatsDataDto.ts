import { UserpanelAdminTargetHistoryDataDto } from './userpanelAdminTargetHistoryDataDto';
import { UserpanelPlayerLobbyStatsDataDto } from './userpanelPlayerLobbyStatsDataDto';
export interface UserpanelStatsDataDto {
    /** Id */
    [0]: number;
    /** Name */
    [1]: string;
    /** SCId */
    [2]: number;
    /** Gang */
    [3]?: string;
    /** AdminLvl */
    [4]: number;
    /** Donation */
    [5]: number;
    /** IsVip */
    [6]: boolean;
    /** Money */
    [7]: number;
    /** TotalMoney */
    [8]: number;
    /** PlayTime */
    [9]: number;

    /** MuteTime */
    [10]?: number;
    /** VoiceMuteTime */
    [11]?: number;

    /** BansInLobbies */
    [12]: string[];

    /** AmountMapsCreated */
    [13]: number;
    /** MapsRatedAverage */
    [14]: number;
    /** CreatedMapsAverageRating */
    [15]: number;
    /** AmountMapsRated */
    [16]: number;

    /** LastLogin */
    [17]: string;
    /** RegisterTimestamp */
    [18]: string;

    /** LobbyStats */
    [19]: UserpanelPlayerLobbyStatsDataDto[];
    /** Logs */
    [20]: UserpanelAdminTargetHistoryDataDto[];
}
