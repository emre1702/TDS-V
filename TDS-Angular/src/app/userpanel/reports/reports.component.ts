import { Component, ChangeDetectorRef } from "@angular/core";
import { UserpanelComponent } from "../userpanel.component";
import { RAGE } from "../../rageconnector/rageconnector.service";
import { UserpanelContentComponent } from "../content/userpanelcontent.component";
import { UserpanelContentService } from "../content/userpanelcontent.service";

@Component({
  selector: "app-userpanel-reports",
  templateUrl: "./reports.component.html",
  styleUrls: ["./reports.component.css"]
})

export class UserpanelReportsComponent implements UserpanelContentComponent {
    language = {};
    inReport = 0;
    opened = false;
    private selectedRow: HTMLElement;
    private selectedRowIndex: number;
    private sortingsSmallToBig = {
        ID: true,
        open: false,
        title: false
    };
    rowDatas: { ID: number, open: boolean, title: string }[] = [
        {
            ID: 1337,
            open: true,
            title: "Wie werde ich so cool wie Bonus? Solid_Snake suckt hart!"
        },
        {
          ID: 1702,
          open: false,
          title: "Wie kann Solid_Snake so schwul sein?"
      }
    ];

    constructor( private rage: RAGE, private userpanelContentService: UserpanelContentService, private cdRef: ChangeDetectorRef ) {
        this.rage.Client.listen( this, this.syncReports, "syncReports" );
        this.rage.Client.listen( this, this.syncReportState, "syncReportState" );
    }

    onOpen() {
        if ( this.language === {} )
            this.language = this.userpanelContentService.getLang( "reports" );
        this.opened = true;

        this.rage.Client.call ( {
            fn: "openReportsMenu",
            args: []
        } );  
        
        this.cdRef.detectChanges();
    }

    onClose() {
        this.opened = false;
        this.rage.Client.call( {
            fn: "closeReportsMenu",
            args: []
        } );

        if ( this.inReport ) {
            this.rage.Client.call( {
                fn: "closeReport",
                args: []
            } ); 
        }

        this.cdRef.detectChanges();
    }

    syncReports( data: string ) {
        this.rowDatas = JSON.parse( data );
        this.cdRef.detectChanges();
    }

    syncReportState( reportid: number, state: boolean ) {
        for ( let report of this.rowDatas ) {
            if ( report.ID === reportid ) {
                report.open = state;
                break;
            }
        }
        this.cdRef.detectChanges();
    }

    toggleRowState() {
        this.rowDatas[this.selectedRowIndex].open = !this.rowDatas[this.selectedRowIndex].open;
        this.rage.Client.call( {
            fn: "toggleReportState",
            args: [this.selectedRowIndex, this.rowDatas[this.selectedRowIndex].open ? 1 : 0]
        } );
    }

    selectRow( event: any, index: number ) {
        if ( this.selectedRow )
            this.selectedRow.removeAttribute( "selected" );
        if ( <HTMLElement>event.target.parentElement !== this.selectedRow ) {
            this.selectedRow = (<HTMLElement>event.target.parentElement);
            this.selectedRow.setAttribute( "selected", "" );
            this.selectedRowIndex = index;
        } else {
            this.selectedRow = undefined;
            this.selectedRowIndex = undefined;
        }
    }

    sortID() {
        this.sortingsSmallToBig.ID = !this.sortingsSmallToBig.ID;
        if ( this.sortingsSmallToBig.ID )
            this.rowDatas.sort ( ( a, b ) => ( a.ID > b.ID ) ? 1 : ( a.ID < b.ID ) ? -1 : 0 );
        else 
            this.rowDatas.sort ( ( a, b ) => ( a.ID > b.ID ) ? -1 : ( a.ID < b.ID ) ? 1 : 0 );  
    }

    sortState() {
        this.sortingsSmallToBig.open = !this.sortingsSmallToBig.open;
        if ( this.sortingsSmallToBig.open )
            this.rowDatas.sort ( ( a, b ) => a.open ? 1 : b.open ? -1 : 0 );
        else 
            this.rowDatas.sort ( ( a, b ) => a.open ? -1 : b.open ? 1 : 0 ); 
    }

    sortTitle() {
        this.sortingsSmallToBig.title = !this.sortingsSmallToBig.title;
        if ( this.sortingsSmallToBig.title )
            this.rowDatas.sort ( ( a, b ) => ( a.title > b.title ) ? 1 : ( a.title < b.title ) ? -1 : 0 );
        else 
            this.rowDatas.sort ( ( a, b ) => ( a.title > b.title ) ? -1 : ( a.title < b.title ) ? 1 : 0 ); 
    }
}