import { LobbyMapLimitType } from '../enums/lobby-map-limit-type';
import { CustomLobbyTeamData } from './custom-lobby-team-data';
import { CustomLobbyWeaponData } from './custom-lobby-weapon-data';

export class CustomLobbyData {
    /**  LobbyId? */
    [0]?: number;
    /** Name */
    [1]: string;
    /** OwnerName? */
    [2]?: string;
    /** Password */
    [3]: string;
    /** StartHealth */
    [4]: number;
    /** StartArmor */
    [5]: number;
    /** AmountLifes */
    [6]: number;

    /** MixTeamsAfterRound */
    [7]: boolean;
    /** ShowRanking */
    [8]: boolean;

    /** BombDetonateTimeMs */
    [9]: number;
    /** BombDefuseTimeMs */
    [10]: number;
    /** BombPlantTimeMs */
    [11]: number;
    /** RoundTime */
    [12]: number;
    /** CountdownTime */
    [13]: number;

    /** SpawnAgainAfterDeathMs */
    [14]: number;
    /** MapLimitTime */
    [15]: number;

    /** MapLimitType */
    [16]: LobbyMapLimitType;

    /** Teams */
    [17]: CustomLobbyTeamData[];
    /** Maps */
    [18]: number[];
    /** Weapons */
    [19]: CustomLobbyWeaponData[];
}
