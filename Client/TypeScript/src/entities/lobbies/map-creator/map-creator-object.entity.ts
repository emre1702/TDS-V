import alt from "alt-client";
import MapObject from "../../wrappers/map-object.wrapper";
import ChatService from "../../../services/output/chat.service";
import MapCreatorPosType from "../../../datas/enums/lobbies/map-creator-pos-type.enum";
import EventsService from "../../../services/events/events.service";
import { inject } from "inversify";
import DIIdentifier from "../../../datas/enums/dependency-injection/di-identifier.enum";
import MapCreatorObjectsService from "../../../services/lobbies/map-creator/map-creator-objects.service";
import { mapObjectBlipRadius, ConstBlipSprites } from "../../../datas/constants";
import MapCreatorObjectData from "../../../datas/interfaces/lobbies/map-creator/map-creator-object-data.interface";
import MapCreatorObjectPos from "../../../datas/interfaces/lobbies/map-creator/map-creator-object-pos.interface";

export default class MapCreatorObject extends MapObject {
    data: MapCreatorObjectData;
    teamNumber: number;
    isSynced = false;

    private _movingPos: alt.Vector3;
    get movingPos(): alt.Vector3 {
        return this._movingPos;
    }
    set movingPos(pos: alt.Vector3) {
        this._movingPos = pos;
        this.setPos(pos);
        this.blip.pos = pos;
    }
    
    private _movingRot: alt.Vector3;
    get movingRot(): alt.Vector3 {
        return this._movingRot;
    }
    set movingRot(rot: alt.Vector3) {
        this._movingRot = rot;
        this.setRot(rot);
        this.blip.heading = rot.z;
    }

    size: alt.Vector3;
    blip: alt.RadiusBlip;

    deleted = false;
    

    constructor(
        @inject(DIIdentifier.ChatService) chatService: ChatService,
        @inject(DIIdentifier.MapCreatorObjectsService) mapCreatorObjectsService: MapCreatorObjectsService,
        @inject(DIIdentifier.EventsService) private eventsService: EventsService,
        model: number | string, pos: alt.Vector3, rot: alt.Vector3, type: MapCreatorPosType, ownerId: number,
        teamNumber: number = undefined, objectName: string = undefined, id: number = -1
    ) {
        super(model, pos, rot, chatService);

        this.data = {
            Type: type, OwnerId: ownerId, ObjOrVehName: objectName, PosData: { Id: id, Pos: pos, Rot: rot }
        };
        this.teamNumber = teamNumber;

        if (this.data.PosData.Id == -1) {
            this.data.PosData.Id = ++mapCreatorObjectsService.idCounter;
            eventsService.onMapCreatorSyncLatestObjectIDRequest.emit(mapCreatorObjectsService.idCounter);
        } else {
            mapCreatorObjectsService.idCounter = Math.max(mapCreatorObjectsService.idCounter, this.data.PosData.Id);
        }

        this.doQueue((handle) => {

            this.size = this.getSize();
            this.blip = this.createBlip();
        });
    }

    isMine(): boolean {
        return this.data.OwnerId == alt.Player.local.getTDSId();
    }

    loadEntityData() {
        this.data.PosData.Pos = this.getPos();
        this._movingPos = { ...this.data.PosData.Pos };

        this.data.PosData.Rot = this.getRot();
        this._movingRot = { ...this.data.PosData.Rot };
    }

    loadPos(pos: MapCreatorObjectPos) {
        this.movingPos = pos.Pos;
        this.movingRot = pos.Rot;

        this.data.PosData = pos;
    }

    resetObjectPos() {
        this.setPos(this.data.PosData.Pos);
        this.setRot(this.data.PosData.Rot);
        this.loadEntityData();

        this.blip.pos = this.data.PosData.Pos;
        this.blip.heading = this.data.PosData.Rot.z;
    }

    remove(syncToServer: boolean) {
        this.deleted = true;
        if (this.blip) {
            this.blip.destroy();
        }
        this.destroy();

        this.eventsService.onMapCreatorObjectDeleted.emit(this);
        if (syncToServer) {
            this.eventsService.onMapCreatorSyncObjectDeleted.emit(this);
        }
    }

    private createBlip(): alt.RadiusBlip {
        const blip = new alt.RadiusBlip(this.data.PosData.Pos.x, this.data.PosData.Pos.y, this.data.PosData.Pos.z, mapObjectBlipRadius);
        blip.name = String(this.data.PosData.Id);

        switch (this.data.Type) {
            case MapCreatorPosType.TeamSpawn:
                blip.sprite = ConstBlipSprites.teamSpawn;
                break;
            case MapCreatorPosType.MapLimit:
                blip.sprite = ConstBlipSprites.mapLimit;
                break;
            case MapCreatorPosType.BombPlantPlace:
                blip.sprite = ConstBlipSprites.bombPlantPlace;
                break;
            case MapCreatorPosType.MapCenter:
                blip.sprite = ConstBlipSprites.mapCenter;
                break;
            case MapCreatorPosType.Target:
                blip.sprite = ConstBlipSprites.target;
                break;
            case MapCreatorPosType.Object:
                blip.sprite = ConstBlipSprites.object;
                break;
            case MapCreatorPosType.Vehicle:
                blip.sprite = ConstBlipSprites.vehicle;
                break;
        }
        

        return null;
    }
}
