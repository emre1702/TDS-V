import { UserpanelAdminTargetHistoryDataDto } from './userpanelAdminTargetHistoryDataDto';
import { UserpanelPlayerLobbyStatsDataDto } from './userpanelPlayerLobbyStatsDataDto';
export interface UserpanelStatsDataDto {
    Id: number;
    Name: string;
    SCName: string;
    Gang?: string;
    AdminLvl: number;
    Donation: number;
    IsVip: boolean;
    Money: number;
    TotalMoney: number;
    PlayTime: number;

    MuteTime?: number;
    VoiceMuteTime?: number;

    BansInLobbies: string[];

    AmountMapsCreated: number;
    MapsRatedAverage: number;
    CreatedMapsAverageRating: number;
    AmountMapsRated: number;

    LastLogin: string;
    RegisterTimestamp: string;

    LobbyStats: UserpanelPlayerLobbyStatsDataDto[];
    Logs: UserpanelAdminTargetHistoryDataDto[];
}
