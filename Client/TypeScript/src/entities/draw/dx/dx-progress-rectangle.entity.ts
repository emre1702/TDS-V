import DxService from "../../../services/draw/dx.service";
import DxBase from "./base/dx-base.entity";
import DxType from "../../../datas/enums/draw/dx-type.enum";
import alt from "alt-client";
import Font from "../../../datas/enums/draw/font.enum";
import Alignment from "../../../datas/enums/draw/alignment.enum";
import { DxText } from "./dx-text.entity";
import { DxRectangle } from "./dx-rectangle.entity";


export default class DxProgressRectangle extends DxBase {
    private textX: number;
    private textY: number;
    private frontRectOffsetX: number;

    private textDraw: DxText;
    private frontRectangle: DxRectangle;
    private backRectangle: DxRectangle;

    private automaticData: { startMs: number, msToEnd: number };

    get progress(): number {
        return this._progress;
    }
    set progress(value: number) {
        this._progress = Math.min(1, Math.max(0, value));
    }
    private _progress: number;

    constructor(dxService: DxService,
        private text: string, private x: number, private y: number,
        private width: number, private height: number,
        private textColor: alt.RGBA, private backColor: alt.RGBA, private progressColor: alt.RGBA,
        private textScale: number = 1, private textFont: Font = Font.ChaletLondon,
        private frontRectOffsetAbsoluteX: number = 3, private frontRectOffsetAbsoluteY: number = 3,
        private filling: boolean = true,
        private alignmentX: Alignment = Alignment.Center, private alignmentY: Alignment = Alignment.Center,
        private relativePos: boolean = true,
        frontPriority: number = 0, activated: boolean = true) {
        super(dxService, frontPriority, activated);

        this.createBackRectangle();
        this.createFrontRectangle();
        this.createTextDraw();
    }


    draw(): void {
        if (this.automaticData) {
            this.progress = (Date.now() - this.automaticData.startMs) / this.automaticData.msToEnd;
        }

        const frontMaxWidth = this.width - this.frontRectOffsetX * 2;
        const frontActualWidth = this.filling
            ? frontMaxWidth * this.progress
            : frontMaxWidth - frontMaxWidth * this.progress; 

        this.frontRectangle.setWidth(frontActualWidth, this.relativePos);

        this.backRectangle.draw();
        this.frontRectangle.draw();
        this.textDraw.draw();
        
    }

    getDxType(): DxType {
        return DxType.ProgressRectangle;
    }

    setAutomatic(msToEnd: number, restart: boolean = true) {
        this.automaticData = { startMs: Date.now(), msToEnd: msToEnd };
        if (restart) {
            this.progress = 0;
        } else {
            this.automaticData.startMs -= msToEnd / this.progress;
        }
    }

    private createTextDraw() {
        this.textX = this.getTextPos(this.x, this.width, this.alignmentX);
        this.textY = this.getTextPos(this.y, this.height, this.alignmentY);

        this.textDraw = new DxText(this.dxService, this.text, this.textX, this.textY, this.textScale, this.textColor, this.textFont,
            Alignment.Center, Alignment.Center, this.relativePos, false, true, this.frontPriority + 2, false);
        this.children.push(this.textDraw);
    }

    private createFrontRectangle() {
        this.frontRectOffsetX = this.relativePos ? this.getRelativeX(this.frontRectOffsetAbsoluteX, false) : this.frontRectOffsetAbsoluteX;
        const offsetY = this.relativePos ? this.getRelativeY(this.frontRectOffsetAbsoluteY, false) : this.frontRectOffsetAbsoluteY;
        const x = this.getFrontRectX(this.x, this.width, this.alignmentX) + this.frontRectOffsetX;
        const y = this.getFrontRectX(this.y, this.height, this.alignmentY) + offsetY;
        const height = this.height - offsetY * 2;

        this.frontRectangle = new DxRectangle(this.dxService, x, y, 0, height, this.progressColor, Alignment.Start, Alignment.Center, this.relativePos, this.frontPriority + 1, false);
        this.children.push(this.frontRectangle);
    }

    private createBackRectangle() {
        this.backRectangle = new DxRectangle(this.dxService, this.x, this.y, this.width, this.height, this.backColor, this.alignmentX, this.alignmentY, this.relativePos, this.frontPriority, false);
        this.children.push(this.backRectangle);
    }

    private getTextPos(rectStart: number, length: number, alignment: Alignment) {
        switch (alignment) {
            case Alignment.Start:
                return rectStart + length / 2;
            case Alignment.Center:
                return rectStart;
            case Alignment.End:
                return rectStart - length / 2;
        }
    }

    private getFrontRectX(rectStart: number, width: number, alignment: Alignment) {
        switch (alignment) {
            case Alignment.Start:
                return rectStart;
            case Alignment.Center:
                return rectStart - width / 2;
            case Alignment.End:
                return rectStart - width;
        }
    }
}
