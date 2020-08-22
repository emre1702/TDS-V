import MapCreatorPosType from "../../../enums/lobbies/map-creator-pos-type.enum";
import MapCreatorObjectPos from "./map-creator-object-pos.interface";

export default interface MapCreatorObjectData {
    PosData: MapCreatorObjectPos;
    Type: MapCreatorPosType;
    ObjOrVehName: string;
    OwnerId: number;
}
