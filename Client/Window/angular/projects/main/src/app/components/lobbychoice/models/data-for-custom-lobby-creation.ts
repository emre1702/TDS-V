import { CustomLobbyWeaponData } from './custom-lobby-weapon-data';
import { CustomLobbyArmsRaceWeaponData } from './custom-lobby-armsraceweapon-data';

export interface DataForCustomLobbyCreation {
    /** WeaponDatas */
    [0]: CustomLobbyWeaponData[];

    /** ArenaWeaponDatas */
    [1]: CustomLobbyWeaponData[];

    /** ArenaArmsRaceWeaponDatas */
    [2]: CustomLobbyArmsRaceWeaponData[];
}
