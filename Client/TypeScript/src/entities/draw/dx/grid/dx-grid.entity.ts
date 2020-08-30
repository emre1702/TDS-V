import DxBase from "../base/dx-base.entity";
import DxType from "../../../../datas/enums/draw/dx-type.enum";
import DxService from "../../../../services/draw/dx.service";
import alt from "alt-client";
import Font from "../../../../datas/enums/draw/font.enum";
import Alignment from "../../../../datas/enums/draw/alignment.enum";
import DxGridRow from "./dx-grid-row.entity";
import game from "natives";
import InputGroup from "../../../../datas/enums/input/input-group.enum";
import Control from "../../../../datas/enums/input/control.enum";
import DxGridColumn from "./dx-grid-column.entity";

export default class DxGrid extends DxBase {

    private rowHeight: number;
    private scrollIndex: number;

    private header: DxGridRow;
    private rows: DxGridRow[];
    columns: DxGridColumn[];

    /**
     * alignmentY is always center, but rows one is start!
     */
    constructor(dxService: DxService,
        public x: number, private y: number, public width: number, bodyHeight: number, bodyBackColor: alt.RGBA, bodyTextScale: number = 1,
        bodyFont: Font = Font.ChaletLondon, public alignmentX: Alignment = Alignment.Center, private maxRows: number = 25,
        frontPriority: number = 0, activated: boolean = true) {
        super(dxService, frontPriority, activated);

        this.rowHeight = bodyHeight / maxRows;
    }

    addRow(row: DxGridRow, setPriority: boolean = true) {
        this.rows.push(row);
        row.height = this.rowHeight;
        //row.grid = this;
        this.children.push(row);
        if (setPriority) {
            row.frontPriority = this.frontPriority + 1;
        }
    }

    addColumn(column: DxGridColumn) {
        this.columns.push(column);
        this.children.push(column);
    }

    clearRows() {
        for (const row of this.rows) {
            row.remove();
            const index = this.children.indexOf(row);
            if (index >= 0) {
                this.children.splice(index, 1);
            }
        }
        this.rows.length = 0;
    }

    setHeader(row: DxGridRow, setPriority: boolean = true) {
        this.header = row;
        if (row.height === undefined) {
            row.height = this.rowHeight;
        } 
        
        //row.grid = this;
        this.children.push(row);
        if (setPriority) {
            row.frontPriority = this.frontPriority + 1;
        }
    }

    draw(): void {
        this.checkScroll();

        const amountRowsToShow = Math.min(this.rows.length, this.maxRows);

        let startY = this.y - (amountRowsToShow * this.rowHeight) / 2;
        if (this.header && this.header.activated) {
            this.header.y = startY - this.header.height / 2;
            startY += this.header.height / 2;
            this.header.draw();
        }

        for (let i = 0; i < amountRowsToShow; ++i) {
            const index = i + this.scrollIndex;
            const row = this.rows[index];
            row.y = startY;
            startY += this.rowHeight;
            row.draw();
        }
    }

    getDxType(): DxType {
        return DxType.Grid;
    }


    private checkScroll() {
        const rowsCount = this.rows.length;
        if (rowsCount <= this.maxRows) {
            this.scrollIndex = 0;
            return;
        }

        const change = Math.ceil((rowsCount - this.maxRows) / 10);
        if (game.isControlJustPressed(InputGroup.Move, Control.SelectNextWeapon)) {
            this.checkScrollDown(rowsCount, change);
        } else if (game.isControlJustPressed(InputGroup.Move, Control.SelectPrevWeapon)) {
            this.checkScrollUp(change);
        }
    }

    private checkScrollDown(rowsCount: number, change: number) {
        if (this.scrollIndex + change < rowsCount - this.maxRows) {
            this.scrollIndex += change;
        } else {
            this.scrollIndex = rowsCount - this.maxRows;
        }
    }

    private checkScrollUp(change: number) {
        if (this.scrollIndex - change > 0) {
            this.scrollIndex -= change;
        } else {
            this.scrollIndex = 0;
        }
    }

}
