import { injectable } from "inversify";

import { getScreenResolution } from "natives";
import { everyTick } from "alt-client";
import DxBase from "../../entities/draw/dx/base/dx-base.entity";

@injectable()
class DxService {
    resX: number;
    resY: number;

    private dxDraws: DxBase[] = [];

    constructor() {
        everyTick(this.renderAll.bind(this));
        this.refreshResolution();
    }

    add(dx: DxBase) {
        this.dxDraws.push(dx);
        this.dxDraws = this.dxDraws.sort((a, b) => a.frontPriority < b.frontPriority ? -1 : 1);
    }

    remove(dx: DxBase) {
        const index = this.dxDraws.indexOf(dx);
        if (index >= 0) {
            this.dxDraws.splice(index, 1);
        }
    }

    private refreshResolution() {
        [, this.resX, this.resY] = getScreenResolution(0, 0);
    }

    private renderAll() {
        for (const draw of this.dxDraws) {
            if (draw.activated) {
                draw.draw();
            }
        }
    }
}

export default DxService;
