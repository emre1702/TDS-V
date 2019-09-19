import { MapCreatorPositionType } from '../enums/mapcreatorpositiontype.enum';
export class MapCreatorPosition {
    public Id: number;
    public Type: MapCreatorPositionType;
    public Info?: string | number;
    public PosX: number;
    public PosY: number;
    public PosZ: number;
    public RotX: number;
    public RotY: number;
    public RotZ: number;

    constructor(id: number, type: MapCreatorPositionType, posX: number, posY: number, posZ: number, rotX: number, rotY: number, rotZ: number) {
        this.Id = id;
        this.Type = type;
        this.PosX = posX;
        this.PosY = posY;
        this.PosZ = posZ;
        this.RotX = rotX;
        this.RotY = rotY;
        this.RotZ = rotZ;
    }
}
