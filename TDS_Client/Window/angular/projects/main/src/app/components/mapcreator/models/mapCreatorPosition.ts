import { MapCreatorPositionType } from '../enums/mapcreatorpositiontype.enum';

export class MapCreatorPosition {
    /** Id */
    [0]: number;
    /** Type */
    [1]: MapCreatorPositionType;
    /** Info */
    [2]?: string | number;
    /** PosX */
    [3]: number;
    /** PosY */
    [4]: number;
    /** PosZ */
    [5]: number;
    /** RotX */
    [6]: number;
    /** RotY */
    [7]: number;
    /** RotZ */
    [8]: number;

    constructor(id: number, type: MapCreatorPositionType, posX: number, posY: number, posZ: number, rotX: number, rotY: number, rotZ: number) {
        this[0] = id;
        this[1] = type;
        this[3] = posX;
        this[4] = posY;
        this[5] = posZ;
        this[6] = rotX;
        this[7] = rotY;
        this[8] = rotZ;
    }
}
