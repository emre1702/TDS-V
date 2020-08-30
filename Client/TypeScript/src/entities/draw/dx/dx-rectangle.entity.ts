import DxBase from "./base/dx-base.entity";
import DxService from "../../../services/draw/dx.service";
import DxType from "../../../datas/enums/draw/dx-type.enum";
import Alignment from "../../../datas/enums/draw/alignment.enum";
import alt from "alt-client";
import game from "natives";

export class DxRectangle extends DxBase {
    

    constructor(dxService: DxService,
                private x: number, private y: number, private width: number, private height: number,
                private color: alt.RGBA, 
                private alignmentX: Alignment = Alignment.Start,
                private alignmentY: Alignment = Alignment.Start,
                relativePos: boolean = true,
                frontPriority: number = 0, activated: boolean = true) {
        super(dxService, frontPriority, activated);

        this.x = this.getRelativeX(x, relativePos);
        this.y = this.getRelativeY(y, relativePos);
        this.width = this.getRelativeX(width, relativePos);
        this.height = this.getRelativeY(height, relativePos);

        this.color = color;
        this.alignmentX = alignmentX;

        this.x += this.getAddingToXForAlignment();
        this.y += this.getAddingToYForAlignment();

        if (alignmentY == Alignment.Start)
            this.y += this.height / 2;
        else if (alignmentY == Alignment.End)
            this.y -= this.height / 2;
    }

    setWidth(width: number, relativePos: boolean) {
        this.x -= this.getAddingToXForAlignment();
        this.width = this.getRelativeX(width, relativePos);
        this.x += this.getAddingToXForAlignment();
    }

    setHeight(height: number, relativePos: boolean) {
        this.y -= this.getAddingToXForAlignment();
        this.height = this.getRelativeY(height, relativePos);
        this.y += this.getAddingToXForAlignment();
    }

    setAlignmentX(alignmentX: Alignment) {
        this.x -= this.getAddingToXForAlignment();
        this.alignmentX = alignmentX;
        this.x += this.getAddingToXForAlignment();
    }

    setAlignmentY(alignmentY: Alignment) {
        this.y -= this.getAddingToYForAlignment();
        this.alignmentY = alignmentY;
        this.y += this.getAddingToYForAlignment();
    }

    draw(): void {
        game.drawRect(this.x, this.y, this.width, this.height, this.color.r, this.color.g, this.color.b, this.getCurrentAlpha(this.color.a), false);
    }

    getDxType(): DxType {
        return DxType.Rectangle;
    }

    private getAddingToXForAlignment() {
        if (this.alignmentX == Alignment.Start) {
            return this.width / 2;
        } else if (this.alignmentX == Alignment.End) {
            return -this.width / 2;
        }
        return 0;
    }

    private getAddingToYForAlignment() {
        if (this.alignmentY == Alignment.Start) {
            return this.height / 2;
        } else if (this.alignmentY == Alignment.End) {
            return -this.height / 2;
        }
        return 0;
    }


}
