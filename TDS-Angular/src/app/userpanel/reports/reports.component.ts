import { Component, ChangeDetectorRef, OnInit, OnDestroy, Input, ViewChild, ElementRef } from "@angular/core";
import { UserpanelComponent } from "../userpanel.component";
import { RAGE } from "../../rageconnector/rageconnector.service";
import { FormGroup, FormBuilder } from "@angular/forms";
import { MatInput } from "@angular/material";

@Component({
  selector: "app-userpanel-reports",
  templateUrl: "./reports.component.html",
  styleUrls: ["./reports.component.css"]
})

export class UserpanelReportsComponent implements OnInit, OnDestroy {
    language: {}; 
    myAdminlvl = 0;
    neededAdminlvls: { removeReport: number };
    inReport: number;
    inCreate = false;
    private selectedRow: HTMLElement;
    private selectedRowIndex: number;
    private sortingsSmallToBig = {
        ID: true,
        open: false,
        title: false
    };
    rowDatas: { ID: number, Author: String, Open: boolean, Title: string }[] = [
        {
            ID: 1337,
            Author: "Solid_Snake",
            Open: true,
            Title: "Wie werde ich so cool wie Bonus? Solid_Snake suckt hart!",
        },
        {
          ID: 1702,
          Author: "Bonus",
          Open: false,
          Title: "Wie kann Solid_Snake so schwul sein?",
      }
    ];
    rowDataTexts: { [ReportID: number]: { ID: number, Author: string, Text: string, Date: string }[]} = {
        1337: [{
            ID: 1,
            Author: "Bonus", 
            Text: "Gar nicht, lel",
            Date: "17:02:14 - 24.07.2018",
        },
        {
            ID: 2,
            Author: "Solid_Snake",
            Text: "spdafkp sdakpofsa opdfkpa sdkfpoasdkpo kaspof kopsda kpofdskpoa fpodspfads kfpodas kfposdapok faspokasodp kopfaksdopf oipdsakjioasdjiof jioasdjoifjasdiodjfoi sdajoif as",
            Date: "17:02:15 - 24.07.2018",
        }]
    };
    forMinAdminlvl = 4;
    static instance: UserpanelReportsComponent;
    formOptions: FormGroup;
    @ViewChild("titleInput", { read: MatInput } ) titleInput: MatInput;
    @ViewChild("textArea", { read: MatInput } ) textArea: MatInput;
    @ViewChild("reportTextArea", { read: MatInput } ) reportAnswerArea: MatInput;

    constructor( private rage: RAGE, private fb: FormBuilder ) {
        UserpanelReportsComponent.instance = this;
        UserpanelReportsComponent.instance.rage = rage;
        this.formOptions = fb.group( {
            "color": "accent"
        } );
    }

    ngOnInit() {
        this.rage.Client.call ( {
            fn: "openReportsMenu",
            args: []
        } );  
    }

    ngOnDestroy() {
        this.rage.Client.call( {
            fn: "closeReportsMenu",
            args: []
        } );

        if ( this.inReport !== undefined ) {
            this.rage.Client.call( {
                fn: "closeReport",
                args: []
            } ); 
        }
    }

    static syncReport( data: string ) {
        let _this = UserpanelReportsComponent.instance;
        _this.rowDatas.unshift ( JSON.parse( data ) ); 
        if ( _this.selectedRowIndex !== undefined )
            ++_this.selectedRowIndex;   
        if ( _this.inReport !== undefined )
            ++_this.inReport;
    }

    static syncReports( report: string ) {
        UserpanelReportsComponent.instance.rowDatas = JSON.parse( report );
        UserpanelReportsComponent.instance.selectedRowIndex = undefined;
        UserpanelReportsComponent.instance.inReport = undefined;
    }

    static syncReportText( reporttext: string ) {
        UserpanelReportsComponent.instance.rowDataTexts[UserpanelReportsComponent.instance.inReport].push( JSON.parse( reporttext ) );    
    }

    static syncReportTexts( reporttexts: string ) {
        UserpanelReportsComponent.instance.rowDataTexts[UserpanelReportsComponent.instance.inReport] = JSON.parse( reporttexts );
    }

    static syncReportState( reportid: number, state: boolean ) {
        for ( let report of UserpanelReportsComponent.instance.rowDatas ) {
            if ( report.ID === reportid ) {
                report.Open = state;
                break;
            }
        }
    }

    static syncReportRemove( reportid: number ) {
        for ( let index in UserpanelReportsComponent.instance.rowDatas ) {
            if ( UserpanelReportsComponent.instance.rowDatas[index].ID === reportid ) {
                let intindex = parseInt( index, 10 );
                if ( UserpanelReportsComponent.instance.inReport === intindex )
                    UserpanelReportsComponent.instance.inReport = undefined;
                if ( UserpanelReportsComponent.instance.selectedRowIndex === intindex )
                    UserpanelReportsComponent.instance.unselectRow();
                UserpanelReportsComponent.instance.rowDatas.splice( intindex, 1 );    
            }
        }
    }

    toggleRowState() {
        UserpanelReportsComponent.instance.rowDatas[this.selectedRowIndex].Open = !UserpanelReportsComponent.instance.rowDatas[this.selectedRowIndex].Open;
        this.rage.Client.call( {
            fn: "toggleReportState",
            args: [this.selectedRowIndex, UserpanelReportsComponent.instance.rowDatas[this.selectedRowIndex].Open ? 1 : 0]
        } );
    }

    selectRow( event: any, index: number ) {
        let selectedrow = this.selectedRow;
        this.unselectRow();
        if ( <HTMLElement>event.target.parentElement !== selectedrow ) {
            this.selectedRow = (<HTMLElement>event.target.parentElement);
            this.selectedRow.setAttribute( "selected", "" );
            this.selectedRowIndex = index;
        }
    }

    unselectRow() {
        if ( this.selectedRow ) {
            this.selectedRow.removeAttribute( "selected" );
            this.selectedRow = undefined;
            this.selectedRowIndex = undefined; 
        }
    }

    sortID() {
        this.sortingsSmallToBig.ID = !this.sortingsSmallToBig.ID;
        if ( this.sortingsSmallToBig.ID )
            UserpanelReportsComponent.instance.rowDatas.sort ( ( a, b ) => ( a.ID > b.ID ) ? 1 : ( a.ID < b.ID ) ? -1 : 0 );
        else 
            UserpanelReportsComponent.instance.rowDatas.sort ( ( a, b ) => ( a.ID > b.ID ) ? -1 : ( a.ID < b.ID ) ? 1 : 0 ); 
        this.unselectRow();
    }

    sortState() {
        this.sortingsSmallToBig.open = !this.sortingsSmallToBig.open;
        if ( this.sortingsSmallToBig.open )
            UserpanelReportsComponent.instance.rowDatas.sort ( ( a, b ) => a.Open ? 1 : b.Open ? -1 : 0 );
        else 
            UserpanelReportsComponent.instance.rowDatas.sort ( ( a, b ) => a.Open ? -1 : b.Open ? 1 : 0 ); 
        this.unselectRow();
    }

    sortTitle() {
        this.sortingsSmallToBig.title = !this.sortingsSmallToBig.title;
        if ( this.sortingsSmallToBig.title )
            UserpanelReportsComponent.instance.rowDatas.sort ( ( a, b ) => ( a.Title > b.Title ) ? 1 : ( a.Title < b.Title ) ? -1 : 0 );
        else 
            UserpanelReportsComponent.instance.rowDatas.sort ( ( a, b ) => ( a.Title > b.Title ) ? -1 : ( a.Title < b.Title ) ? 1 : 0 ); 
        this.unselectRow();
    }

    switchToCreation() {
        this.inCreate = true;
    }

    switchToReport() {
        this.inReport = this.selectedRowIndex;
    }

    removeReport() {   
        this.rage.Client.call( {
            fn: "removeReport",
            args: [this.rowDatas[this.selectedRowIndex].ID]
        } );
        this.rowDatas.splice ( this.selectedRowIndex, 1 ); 
    }

    setReportFor( minadminlvl: number ) {
        this.forMinAdminlvl = minadminlvl;
    }

    createReport() {
        let title = this.titleInput.value || "" as string;
        if ( title.length < 5 || title.length > 50 ) {
            this.titleInput.errorState = true;
            return;
        } else 
            this.titleInput.errorState = false;

        let text = this.textArea.value || "" as string;
        if ( text.length < 5 || text.length > 200 ) {
            this.textArea.errorState = true;
            return;
        } else 
            this.textArea.errorState = false;

        this.rage.Client.call( {
            fn: "createReport",
            args: [title, text, this.forMinAdminlvl]
        } );

        this.inCreate = false;
    }

    sendReportAnswer() {
        let text = this.reportAnswerArea.value || "" as string;
        if ( text.length < 5 || text.length > 250 ) {
            this.reportAnswerArea.errorState = true;
            return;
        } else 
            this.reportAnswerArea.errorState = false;

        this.rage.Client.call( {
            fn: "addTextToReport",
            args: [this.rowDatas[this.inReport].ID, text]
        } );

        this.reportAnswerArea.value = "";
    }

}