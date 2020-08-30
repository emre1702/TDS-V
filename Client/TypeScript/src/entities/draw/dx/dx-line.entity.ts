import DxService from "../../../services/draw/dx.service";
import DxBase from "./base/dx-base.entity";
import DxType from "../../../datas/enums/draw/dx-type.enum";
import game from "natives";
import alt from "alt-client";
import { getWorldCoordFromScreenCoord } from "../../../datas/utils";
import CamerasService from "../../../services/cameras/cameras.service";

export default class DxLine extends DxBase {
    
    constructor(dxService: DxService, private camerasService: CamerasService,
        private startX: number, private startY: number, private startZ: number | undefined,
        private endX: number, private endY: number, private endZ: number | undefined,
        private color: alt.RGBA, private relative: boolean = true,
        frontPriority: number = 0, activated: boolean = true) {
        super(dxService, frontPriority, activated);

        if (this.startZ === undefined || this.endZ === undefined) {
            [this.startX, this.startY, this.startZ] = this.convertScreenToWorld(this.startX, this.startY);
            [this.endX, this.endY, this.endZ] = this.convertScreenToWorld(this.endX, this.endY);
        }
    }

    draw(): void {
        game.drawLine(this.startX, this.startY, this.startZ, this.endX, this.endY, this.endZ, this.color.r, this.color.g, this.color.b, this.getCurrentAlpha(this.color.a));
    }

    getDxType(): DxType {
        return DxType.Line;
    }

    private convertScreenToWorld(x: number, y: number): [number, number, number] {
        x = this.getRelativeX(x, this.relative);
        y = this.getRelativeY(y, this.relative);
        const pos = getWorldCoordFromScreenCoord({ x, y }, this.camerasService.activeCamera);

        return [pos.x, pos.y, pos.z];
    }
}
