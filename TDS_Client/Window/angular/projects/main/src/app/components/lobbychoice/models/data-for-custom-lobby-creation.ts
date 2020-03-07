import { CustomLobbyWeaponData } from './custom-lobby-weapon-data';

export interface DataForCustomLobbyCreation {
    /** WeaponDatas */
    [0]: CustomLobbyWeaponData[];

    /** ArenaWeaponDatas */
    [1]: CustomLobbyWeaponData[];
}
