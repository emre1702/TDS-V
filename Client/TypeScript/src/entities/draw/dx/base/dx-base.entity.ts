import { getTextScaleHeight } from "natives";
import DxType from "../../../../datas/enums/draw/dx-type.enum";
import DxService from "../../../../services/draw/dx.service";
import Font from "../../../../datas/enums/draw/font.enum";

export abstract class DxBase {
    frontPriority: number;
    activated: boolean;

    protected alphaAnim: { targetAlpha: number, startMs: number, endMs: number };
    
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

    blendAlpha(targetAlpha: number, msToEnd: number) {
        const currentMs = Date.now();
        this.alphaAnim = { targetAlpha: targetAlpha, startMs: currentMs, endMs: currentMs + msToEnd };
    }

    remove() {
        this.dxService.remove(this);

        for (const child of this.children) {
            child.remove();
        }
        this.children.length = 0;
    }

    protected getBlendValue(start: number, end: number, startMs: number, endMs: number): number {
        const progress = Math.min(1, (Date.now() - startMs) / (endMs - startMs));
        return Math.floor(start + progress * (end - start));
    }

    protected getAbsoluteX(x: number, relative: boolean): number {
        return Math.round(relative ? x * this.dxService.resX : x);
    }

    protected getAbsoluteY(y: number, relative: boolean): number {
        return Math.round(relative ? y * this.dxService.resY : y);
    }

    protected getRelativeX(x: number, relative: boolean): number {
        return relative ? x : x / this.dxService.resX;
    }

    protected getRelativeY(y: number, relative: boolean): number {
        return relative ? y : y / this.dxService.resY;
    }

    protected getTextAbsoluteHeight(lineCount: number, scale: number, font: Font, relative: boolean): number {
        let textHeight = this.getAbsoluteY(getTextScaleHeight(scale, font), relative);

        // + 5 ... because of the margin between the lines
        textHeight = Math.floor(textHeight * lineCount + textHeight * 0.4 * (lineCount - 0.3));
        return textHeight;
    }

    protected getCurrentAlpha(initAlpha: number) {
        if (!this.alphaAnim) {
            return initAlpha;
        }
        return this.getBlendValue(initAlpha, this.alphaAnim.targetAlpha, this.alphaAnim.startMs, this.alphaAnim.endMs);
    }
}

export default DxBase;
