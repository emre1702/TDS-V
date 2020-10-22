import { WeaponHash } from '../../lobbychoice/enums/weapon-hash.enum';

export interface DamageTestWeapon {
    /** Weapon */
    [0]: WeaponHash;

    /** Damage */
    [1]: number;

    /** HeadshotMultiplicator */
    [2]: number;
}
