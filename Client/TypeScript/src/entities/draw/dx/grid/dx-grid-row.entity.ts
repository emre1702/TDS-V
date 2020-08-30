import DxBase from "../base/dx-base.entity";
import DxType from "../../../../datas/enums/draw/dx-type.enum";
import DxService from "../../../../services/draw/dx.service";
import alt from "alt-client";
import Font from "../../../../datas/enums/draw/font.enum";
import Alignment from "../../../../datas/enums/draw/alignment.enum";
import DxGridCell from "./dx-grid-cell.entity";
import DxGrid from "./dx-grid.entity";
import DxTextRectangle from "../dx-text-rectangle.entity";

export default class DxGridRow extends DxBase {
    y: number;
    private textHeight: number;

    private useColorForWholeRow: boolean;
    private cells: DxGridCell[] = [];
    private textRectangle: DxTextRectangle;

    /**
     * alignmentY is always Start!
     */
    constructor(dxService: DxService, grid: DxGrid,
        public height: number, private backColor: alt.RGBA, text: string | undefined, public textColor: alt.RGBA = new alt.RGBA(255, 255, 255, 255),
        public scale: number = 0.4, public font: Font = Font.ChaletLondon, public textAlignmentX: Alignment = Alignment.Start,
        relativePos: boolean = true, frontPriority: number = 1) {
        super(dxService, frontPriority, false);

        this.textHeight = this.getTextAbsoluteHeight(1, scale, font, relativePos);
        if (relativePos) {
            this.textHeight = this.getRelativeY(this.textHeight, false);
        }

        this.textRectangle = new DxTextRectangle(dxService, text, grid.x, 0, grid.width, height, textColor, backColor, scale, font, 5,
            grid.alignmentX, Alignment.Start, textAlignmentX, Alignment.Center, relativePos, frontPriority, false);

        grid.addRow(this);
    } 

    addCell(cell: DxGridCell, setPriority: boolean = true) {
        this.cells.push(cell);
        if (setPriority) {
            cell.frontPriority = this.frontPriority + 1;
        }
        if (cell.backColor && cell.backColor != this.backColor) {
            this.useColorForWholeRow = false;
        }
    }

    draw(): void {
        if (this.useColorForWholeRow) {
            this.textRectangle.y = this.y;
            this.textRectangle.draw();
        }
        for (const cell of this.cells) {
            if (this.useColorForWholeRow) {
                cell.noBackgroundNextCall();
            }
            
            cell.draw();
        }
    }

    getDxType(): DxType {
        return DxType.GridRow;
    }
}
