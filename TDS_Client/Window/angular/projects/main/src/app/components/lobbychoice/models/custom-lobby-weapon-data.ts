import { WeaponHash } from '../enums/weapon-hash.enum';

export interface CustomLobbyWeaponData {
    /** WeaponHash */
    [0]: WeaponHash;

    /** Ammo */
    [1]: number;

    /** Damage */
    [2]: number;

    /** HeadshotMultiplicator */
    [3]: number;
}
