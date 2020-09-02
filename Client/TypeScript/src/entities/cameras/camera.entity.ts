import { injectable, inject } from "inversify";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import game from "natives";
import alt from "alt-client";
import CamerasService from "../../services/cameras/cameras.service";
import { getDirectionByRotation } from "../../datas/utils";
import PedBone from "../../datas/enums/gta/ped-bone.enum";

@injectable()
export default class Camera {
    name: string;

    private _position = new alt.Vector3(0, 0, 0);
    get position(): alt.Vector3 {
        return this._position;
    }
    set position(value: alt.Vector3) {
        this._position = value;
        game.setCamCoord(this.handle, value.x, value.y, value.z);
    }

    private _rotation = new alt.Vector3(0, 0, 0);
    get rotation(): alt.Vector3 {
        return this._rotation;
    }
    set rotation(value: alt.Vector3) {
        this._rotation = value;
        game.setCamRot(this.handle, value.x, value.y, value.z, 2);
    }

    private _spectatingEntity: alt.Entity;
    get spectatingEntity(): alt.Entity {
        return this._spectatingEntity;
    }
    set spectatingEntity(value: alt.Entity | undefined) {
        this._spectatingEntity = value;

        if (this.entityAttachUpdateTickId) {
            alt.clearEveryTick(this.entityAttachUpdateTickId);
            this.entityAttachUpdateTickId = undefined;
        }
        if (value) {
            this.entityAttachUpdateTickId = alt.everyTick(this.attachToEntityOnTick.bind(this));
        }
    }

    get direction(): alt.Vector3 {
        return getDirectionByRotation(this._rotation);
    }

    get isActive(): boolean {
        return this.camerasService.activeCamera == this;
    }
    set isActive(value: boolean) {
        if (value) {
            this.setActive();
        } else {
            this.setInactive();
        }
    }

    private handle: number;
    private entityAttachUpdateTickId: number | undefined;

    constructor(
        @inject(DIIdentifier.CamerasService) private camerasService: CamerasService,
        name: string) {

        this.handle = game.createCam("DEFAULT_SCRIPTED_CAMERA", false);
    }

    //Todo Implement this
    attach(entity: alt.Entity, bone: PedBone, x: number, y: number, z: number, heading: boolean) {
        //Cam.AttachTo(ped, bone, x, y, z, heading);
    }

    //Todo Implement this
    detach() {
        //Cam.Detach();
        this.spectatingEntity = undefined;
    }

    lookAt(ped: alt.Entity, bone: PedBone, posOffsetX: number, posOffsetY: number, posOffsetZ: number,
        lookAtOffsetX: number, lookAtOffsetZ: number) {
        this.position = game.getPedBoneCoords(ped.scriptID, bone, posOffsetZ, posOffsetY, posOffsetX) as alt.Vector3;
        this.pointAtCoord(game.getPedBoneCoords(ped.scriptID, bone, lookAtOffsetX, 0, lookAtOffsetZ) as alt.Vector3);
        game.setFocusEntity(ped.scriptID);
        this.camerasService.focusAtPos = null;
    }

    pointAtCoord(pos: alt.Vector3) {
        game.pointCamAtCoord(this.handle, pos.x, pos.y, pos.z);
        this.camerasService.setFocusArea(pos);
    }

    render(render: boolean, ease: boolean = false, easeTime: number = 0) {
        game.renderScriptCams(render, ease, easeTime, true, false, 0);
    }

    renderToPosition(pos: alt.Vector3, ease: boolean = false, easeTime: number = 0) {
        this.setPosition(pos);
        this.render(true, ease, easeTime);
    }

    setFov(fov: number) {
        fov = Math.min(130, Math.max(1, fov));
        game.setCamFov(this.handle, fov);
    }

    setPosition(position: alt.Vector3, instantly: boolean = false) {
        this.position = position;

        if (instantly) {
            this.render(true, false, 0);
        }
    }

    spectate(entity: alt.Entity) {
        if (this._spectatingEntity != null) {
            this.detach();
        }

        this.spectatingEntity = entity;
        this.attach(entity, PedBone.SKEL_Head, 0, -2, 0.3, true);
        game.setFocusEntity(entity.scriptID);
        this.camerasService.focusAtPos = null;
    }

    remove() {
        if (this.handle) {
            game.destroyCam(this.handle, false);
        }
        alt.clearEveryTick(this.entityAttachUpdateTickId);
    }

    setActive(instantly: boolean = false) {
        if (this.camerasService.activeCamera === this) {
            return;
        }

        const prevCam = this.camerasService.activeCamera;
        if (prevCam) {
            prevCam.setInactive();
        }
        
        this.camerasService.activeCamera = this;
        game.setCamActive(this.handle, true);

        if (this._spectatingEntity) {
            this.entityAttachUpdateTickId = alt.everyTick(this.attachToEntityOnTick.bind(this));
        }
        if (instantly) {
            this.render(true, false, 0);
        }
    }

    setInactive(instantly: boolean = false) {
        if (this.camerasService.activeCamera !== this) {
            return;
        }
        this.camerasService.activeCamera = undefined;
        game.setCamActive(this.handle, false);
        this.detach();

        if (this.entityAttachUpdateTickId !== undefined) {
            alt.clearEveryTick(this.entityAttachUpdateTickId);
            this.entityAttachUpdateTickId = undefined;
        } 

        if (instantly) {
            this.camerasService.removeFocusArea();
            this.render(false, false, 0);
        }
    }

    //Todo Check if 
    private attachToEntityOnTick() {
        if (!this._spectatingEntity || !this._spectatingEntity.valid) {
            alt.clearEveryTick(this.entityAttachUpdateTickId);
            this.entityAttachUpdateTickId = undefined;
        }
        this.rotation = this._spectatingEntity.rot;
    }
}
