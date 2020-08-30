import DxBase from "../base/dx-base.entity";
import DxType from "../../../../datas/enums/draw/dx-type.enum";
import DxService from "../../../../services/draw/dx.service";
import DxGrid from "./dx-grid.entity";
import Alignment from "../../../../datas/enums/draw/alignment.enum";

export default class DxGridColumn extends DxBase {
    x: number;

    constructor(dxService: DxService, grid: DxGrid,
        public width: number, relativePos: boolean = true, public relativeWidth: boolean = true,
        frontPriority: number) {
        super(dxService, frontPriority, false);

        this.width = relativeWidth ? width * grid.width : width;

        this.x = grid.x + this.getGridColumnsWidthSum(grid);
        if (grid.alignmentX == Alignment.Center)
            this.x -= grid.width / 2;
        else if (grid.alignmentX == Alignment.End)
            this.x -= grid.width;

        grid.addColumn(this);
    }

    draw(): void { }

    getDxType(): DxType {
        return DxType.GridColumn;
    }

    private getGridColumnsWidthSum(grid: DxGrid): number {
        let sum = 0;
        for (const column of grid.columns) {
            sum += column.relativeWidth ? column.width * grid.width : column.width;
        }
        return sum;
    } 
}
