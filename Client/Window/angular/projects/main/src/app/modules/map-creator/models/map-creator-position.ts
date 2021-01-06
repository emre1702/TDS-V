import { MapCreatorPositionType } from '../enums/map-creator-position-type';

export interface MapCreatorPosition {
    /** Id */
    0: number;

    /** Type */
    1: MapCreatorPositionType;

    /** Info */
    2?: string | number;

    /** PosX */
    3: number;

    /** PosY */
    4: number;

    /** PosZ */
    5: number;

    /** RotX */
    6: number;

    /** RotY */
    7: number;

    /** RotZ */
    8: number;

    /** OwnerRemoteId */
    9: number;
}
