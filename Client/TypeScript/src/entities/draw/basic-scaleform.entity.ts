import game from "natives";
import alt from "alt-client";

export default class BasicScaleform {
    get isLoaded(): boolean {
        return game.hasScaleformMovieLoaded(this.handle);
    }
    get isValid(): boolean {
        return this.handle !== 0;
    }

    private handle: number;
    private functionQueue: {funcName: string, args: any[]}[] = [];

    constructor(scaleformName: string) {
        this.handle = game.requestScaleformMovie(scaleformName);
    }


    call(funcName: string, ...args: any[]) {
        if (!this.isLoaded || !this.isValid) {
            this.functionQueue.push({ funcName, args });
            return;
        }

        game.beginScaleformMovieMethod(this.handle, funcName);

        for (const arg in args) {

            switch (typeof arg) {
                case "string":
                    game.scaleformMovieMethodAddParamTextureNameString(arg);
                    break;
                case "boolean":
                    game.scaleformMovieMethodAddParamBool(arg);
                    break;
                case "number":
                    if (Number.isInteger(arg)) {
                        game.scaleformMovieMethodAddParamInt(arg);
                    } else {
                        game.scaleformMovieMethodAddParamFloat(arg);
                    }
                    break;
            }
        }

        game.endScaleformMovieMethod();
    }

    render2D(pos: alt.Vector2, size: alt.Vector2) {
        this.onUpdate();
        if (this.isLoaded && this.isValid) {
            game.drawScaleformMovie(this.handle, pos.x, pos.y, size.x, size.y, 255, 255, 255, 255, 0);
        }
    }

    render3D(pos: alt.Vector3, rot: alt.Vector3, scale: alt.Vector3) {
        this.onUpdate();
        if (this.isLoaded && this.isValid) {
            game.drawScaleformMovie3dSolid(this.handle, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, 2, 2, 1, scale.x, scale.y, scale.z, 2);
        }
    }

    render3DAdditive(pos: alt.Vector3, rot: alt.Vector3, scale: alt.Vector3) {
        this.onUpdate();
        if (this.isLoaded && this.isValid) {
            game.drawScaleformMovie3d(this.handle, pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, 2, 2, 1, scale.x, scale.y, scale.z, 2);
        }
    }

    renderFullscreen() {
        this.onUpdate();
        if (this.isLoaded && this.isValid) {
            game.drawScaleformMovieFullscreen(this.handle, 255, 255, 255, 255, 0);
        }
    }

    private onUpdate() {
        if (!this.functionQueue.length || !this.isLoaded || !this.isValid) {
            return;
        }

        for (const entry of this.functionQueue) {
            this.call(entry.funcName, ...entry.args);
        }
        this.functionQueue.length = 0;
    }

    remove() {
        game.setScaleformMovieAsNoLongerNeeded(this.handle);
        this.functionQueue.length = 0;
    }
}
