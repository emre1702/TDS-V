import LobbyType from "../../enums/lobbies/lobby-type.enum";
import MapLimitType from "../../enums/lobbies/map-limit-type.enum";

export default interface LobbySettings {
    Id: number;
    Name: string;
    Type: LobbyType;
    IsOfficial: boolean;
    SpawnAgainAfterDeathMs?: number;
    BombDefuseTimeMs?: number;
    BombPlantTimeMs?: number;
    CountdownTime?: number;
    RoundTime?: number;
    BombDetonateTimeMs?: number;
    MapLimitTime?: number;
    InLobbyWithMaps: boolean;
    MapLimitType?: MapLimitType;
    StartHealth: number;
    StartArmor: number;
    IsGangActionLobby: boolean;
}
