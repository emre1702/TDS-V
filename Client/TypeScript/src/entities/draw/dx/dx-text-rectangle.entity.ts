import DxBase from "./base/dx-base.entity";
import DxType from "../../../datas/enums/draw/dx-type.enum";
import { DxText } from "./dx-text.entity";
import { DxRectangle } from "./dx-rectangle.entity";
import DxService from "../../../services/draw/dx.service";
import alt from "alt-client";
import game from "natives";
import Font from "../../../datas/enums/draw/font.enum";
import Alignment from "../../../datas/enums/draw/alignment.enum";

export default class DxTextRectangle extends DxBase {
    private textDraw: DxText;
    private rectangle: DxRectangle;
    noBackgroundNextCall: boolean;

    constructor(dxService: DxService,
                private text: string | undefined, private x: number, public y: number, private width: number, private height: number,
                private textColor: alt.RGBA, rectColor: alt.RGBA | undefined, private textScale: number = 1, private textFont: Font = Font.ChaletLondon,
                private textOffsetAbsoluteX: number = 0,
                private alignmentX: Alignment = Alignment.Start, private alignmentY = Alignment.Start,
                private textAlignmentX: Alignment = Alignment.Center, private textAlignmentY = Alignment.Center,
                private relativePos: boolean = true, frontPriority: number = 0, activated: boolean = true) {
        super(dxService, frontPriority, activated);

        if (rectColor) {
            this.rectangle = new DxRectangle(dxService, x, y, width, height, rectColor, alignmentX, alignmentY, relativePos, frontPriority, activated);
            this.children.push(this.rectangle);
        }

        if (text) {
            this.createTextDraw();
        }
    }

    private createTextDraw() {
        const textOffsetX = this.relativePos ? this.getRelativeX(this.textOffsetAbsoluteX, false) : this.textOffsetAbsoluteX;
        const textPosX = this.getTextPosX(textOffsetX);
        const textPosY = this.getTextPosY();
        this.textDraw = new DxText(this.dxService, this.text, textPosX, textPosY, this.textScale, this.textColor, this.textFont,
            this.textAlignmentX, this.textAlignmentY, this.relativePos, false, true, this.frontPriority + 1, false);
        this.textDraw.setBoundary(this.getRectangleLeftX() + textOffsetX, this.getRectangleRightX() - textOffsetX);
        this.children.push(this.textDraw);
    }

    setText(text: string) {
        if (!this.textDraw) {
            this.createTextDraw();
        } else {
            this.textDraw.setText(text);
        }
    }

    draw(): void {
        if (this.textDraw) {
            this.textDraw.draw();
        }
        if (this.rectangle) {
            if (!this.noBackgroundNextCall) {
                this.rectangle.draw();
            } else {
                this.noBackgroundNextCall = false;
            }
            
        }
    }

    getDxType(): DxType {
        return DxType.TextRectangle;
    }

    private getTextPosX(offsetX: number): number {
        switch (this.textAlignmentX) {
            case Alignment.Start:
                return this.getRectangleCenterX() + offsetX;
            case Alignment.Center:
                return this.getRectangleCenterX();
            case Alignment.End:
                return this.getRectangleRightX() - offsetX;
        }
    }

    /** Currently only works for 1 liners correctly */
    private getTextPosY(): number {
        const height = game.getTextScaleHeight(this.textScale, this.textFont);
        switch (this.textAlignmentY) {
            case Alignment.Start:
                return this.getRectangleTopY();
            case Alignment.Center:
                return this.getRectangleCenterY() - height / 2;
            case Alignment.End:
                return this.getRectangleBottomY() - height;
        }
    }

    private getRectangleLeftX(): number {
        switch (this.alignmentX) {
            case Alignment.Start:
                return this.x;
            case Alignment.Center:
                return this.x - this.width / 2;
            case Alignment.End:
                return this.x - this.width;
        }
    }

    private getRectangleCenterX(): number {
        switch (this.alignmentX) {
            case Alignment.Start:
                return this.x + this.width / 2;
            case Alignment.Center:
                return this.x;
            case Alignment.End:
                return this.x - this.width / 2;
        }
    }

    private getRectangleRightX(): number {
        switch (this.alignmentX) {
            case Alignment.Start:
                return this.x + this.width;
            case Alignment.Center:
                return this.x + this.width / 2;
            case Alignment.End:
                return this.x;
        }
    }

    private getRectangleTopY(): number {
        switch (this.alignmentY) {
            case Alignment.Start:
                return this.y;
            case Alignment.Center:
                return this.y - this.height / 2;
            case Alignment.End:
                return this.y - this.height;
        }
    }

    private getRectangleCenterY(): number {
        switch (this.alignmentY) {
            case Alignment.Start:
                return this.y + this.height / 2;
            case Alignment.Center:
                return this.y;
            case Alignment.End:
                return this.y - this.height / 2;
        }
    }

    private getRectangleBottomY(): number {
        switch (this.alignmentY) {
            case Alignment.Start:
                return this.y + this.height;
            case Alignment.Center:
                return this.y + this.height / 2;
            case Alignment.End:
                return this.y;
        }
    }
}
