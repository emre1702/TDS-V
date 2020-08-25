import alt from "alt-client";
import game from "natives";
import { loadModelAsync } from "../../datas/helper/model.helper";
import ChatService from "../../services/output/chat.service";
import SettingsService from "../../services/settings/settings.service";

export default class MapObject {

    private callPromise: Promise<void>;
    private handle: number;

    constructor(
        chatService: ChatService, settingsService: SettingsService,
        model: number | string, pos: alt.Vector3, rot: alt.Vector3,) {
        this.callPromise = loadModelAsync(model).then(modelHash => {
            this.handle = game.createObjectNoOffset(modelHash, pos.x, pos.y, pos.z, false, false, false);
            game.setEntityRotation(this.handle, rot.x, rot.y, rot.z, 2, true);
            game.freezeEntityPosition(this.handle, true);
        }).catch(reason => {
            chatService.output(settingsService.language.get(reason));
        });
    }

    getPos(): alt.Vector3 {
        if (!this.handle) {
            return new alt.Vector3(0, 0, 0);
        }
        return game.getEntityCoords(this.handle, true) as alt.Vector3;
    }

    setPos(pos: alt.Vector3): void {
        this.callPromise = this.callPromise.then(() => {
            game.setEntityCoordsNoOffset(this.handle, pos.x, pos.y, pos.z, true, false, false);
        });
    }

    getRot(): alt.Vector3 {
        if (!this.handle) {
            return new alt.Vector3(0, 0, 0);
        }
        return game.getEntityRotation(this.handle, 2) as alt.Vector3;
    }

    setRot(rot: alt.Vector3): void {
        this.callPromise = this.callPromise.then(() => {
            game.setEntityRotation(this.handle, rot.x, rot.y, rot.z, 2, true);
        });
    }

    activatePhysics() {
        this.callPromise = this.callPromise.then(() => {
            game.activatePhysics(this.handle);
        });
    }

    getSize(): alt.Vector3 {
        if (!this.handle) {
            return new alt.Vector3(0, 0, 0);
        }
        const [, a, b] = game.getModelDimensions(this.handle, new alt.Vector3(0, 0, 0), new alt.Vector3(9999, 9999, 9999));
        return new alt.Vector3(b.x - a.x, b.y - a.y, b.z - a.z);
    }

    freeze(toggle: boolean) {
        this.callPromise = this.callPromise.then(() => {
            game.freezeEntityPosition(this.handle, toggle);
        });
    }

    setCollision(toggle: boolean, keepPhysics: boolean) {
        this.callPromise = this.callPromise.then(() => {
            game.freezeEntityPosition(this.handle, toggle);
        });
    }

    doQueue(action: (handle: number) => void): void {
        this.callPromise = this.callPromise.then(() => action(this.handle));
    }

    destroy(): void {
        this.callPromise = this.callPromise.then(() => {
            game.deleteObject(this.handle);
            throw new Error();
        });
        
    }
}
