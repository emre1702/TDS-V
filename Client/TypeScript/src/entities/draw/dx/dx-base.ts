import DxService from "../../../handler/draw/dx/dx.service";
import { getTextScaleHeight } from "natives";
import Font from "../../../data/enums/dx/font.enum";
import DxType from "../../../data/enums/dx/dx-type.enum";

abstract class DxBase {
    frontPriority: number;
    activated: boolean;

    protected readonly children: DxBase[] = [];
    protected readonly dxService: DxService;

    constructor(
        dxService: DxService, frontPriority: number = 0, activated: boolean = true) {

        this.dxService = dxService;

        this.activated = activated;
        this.frontPriority = frontPriority;

        this.dxService.add(this);
    }

    abstract draw(): void;
    abstract getDxType(): DxType;

    remove() {
        this.dxService.remove(this);

        for (const child of this.children) {
            child.remove();
        }
        this.children.length = 0;
    }

    protected getBlendValue(currentTick: number, start: number, end: number, startTick: number, endTick: number): number {
        const progress = Math.min(1, (currentTick - startTick) / (endTick - startTick));
        return Math.floor(start + progress * (end - start));
    }

    protected getAbsoluteX(x: number, relative: boolean, isText: boolean = false): number {
        return Math.round(relative ? x * (isText ? 1920 : this.dxService.resX) : x * (isText ? (1920 / this.dxService.resX) : 1));
    }

    protected getAbsoluteY(y: number, relative: boolean, isText: boolean = false): number {
        return Math.round(relative ? y * (isText ? 1080 : this.dxService.resY) : y * (isText ? (1080 / this.dxService.resY) : 1));
    }

    protected getRelativeX(x: number, relative: boolean, isText: boolean = false): number {
        return relative ? x : x / (isText ? 1920 : this.dxService.resX);
    }

    protected getRelativeY(y: number, relative: boolean, isText: boolean = false): number {
        return relative ? y : y / (isText ? 1080 : this.dxService.resY);
    }

    protected getTextAbsoluteHeight(lineCount: number, scale: number, font: Font, relative: boolean): number {
        let textHeight = this.getAbsoluteY(getTextScaleHeight(scale, font), relative, true);

        // + 5 ... because of the margin between the lines
        textHeight = Math.floor(textHeight * lineCount + textHeight * 0.4 * (lineCount - 0.3));
        return textHeight;
    }
}

export default DxBase;
