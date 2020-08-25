import { injectable, inject } from "inversify";
import DIIdentifier from "../../datas/enums/dependency-injection/di-identifier.enum";
import game from "natives";
import alt from "alt-client";

@injectable()
export default class Camera {
    name: string;

    private _position = new alt.Vector3(0, 0, 0);
    get position(): alt.Vector3 {
        return this._position;
    }
    set position(value: alt.Vector3) {
        this._position = value;
    }

    private handle: number;
    private everyTickId: number;

    constructor(
        @inject(DIIdentifier.CamerasService) private camerasService: CamerasService,
        name: string) {

        this.handle = game.createCam("DEFAULT_SCRIPTED_CAMERA", false);

        this.everyTickId = alt.everyTick(this.onUpdate.bind(this));
    }

    remove() {
        if (this.handle) {
            game.destroyCam(this.handle, false);
        }
        alt.clearEveryTick(this.everyTickId);
    }
}
