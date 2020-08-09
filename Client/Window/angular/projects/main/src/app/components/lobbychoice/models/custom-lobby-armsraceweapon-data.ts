import { WeaponHash } from '../enums/weapon-hash.enum';

export interface CustomLobbyArmsRaceWeaponData {
    /** WeaponHash */
    [0]?: WeaponHash;

    /** AtKill */
    [1]: number;
}
