import DxBase from "../base/dx-base.entity";
import alt from "alt-client";
import DxType from "../../../../datas/enums/draw/dx-type.enum";
import DxTextRectangle from "../dx-text-rectangle.entity";
import DxService from "../../../../services/draw/dx.service";
import DxGridRow from "./dx-grid-row.entity";
import Font from "../../../../datas/enums/draw/font.enum";
import Alignment from "../../../../datas/enums/draw/alignment.enum";
import DxGridColumn from "./dx-grid-column.entity";

export default class DxGridCell extends DxBase {

    private dxTextRectangle: DxTextRectangle;

    constructor(dxService: DxService, text: string, row: DxGridRow, column: DxGridColumn, public backColor: alt.RGBA | undefined = undefined,
        textColor: alt.RGBA | undefined = undefined, scale: number | undefined = undefined, font: Font | undefined = undefined,
        alignmentX: Alignment | undefined = undefined, relativePos: boolean = true,
        frontPriority: number = 2) {
        super(dxService, frontPriority, false);

        alignmentX = alignmentX ?? row.textAlignmentX;

        this.dxTextRectangle = new DxTextRectangle(dxService, text, column.x, row.y, column.width, row.height,
            textColor ?? row.textColor, backColor, scale ?? row.scale, font ?? row.font, 3,
            Alignment.Start, Alignment.Start,
            alignmentX ?? row.textAlignmentX, Alignment.Center,
            relativePos, frontPriority, false);
        this.children.push(this.dxTextRectangle);

        row.addCell(this);
    }

    draw(): void {
        this.dxTextRectangle.draw();
    }

    getDxType(): DxType {
        return DxType.GridCell;
    }

    noBackgroundNextCall() {
        this.dxTextRectangle.noBackgroundNextCall = true;
    }

}
