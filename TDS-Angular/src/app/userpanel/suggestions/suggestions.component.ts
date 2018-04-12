import { Component, Input, ViewChild, OnDestroy, OnInit, Output, EventEmitter } from "@angular/core";
import { UserpanelComponent } from "../userpanel.component";
import { RAGE } from "../../rageconnector/rageconnector.service";
import { MatInput, MatChipListChange, MatChip, MatChipList } from "@angular/material";
import { FormBuilder, FormGroup } from "@angular/forms";

@Component({
    selector: "app-userpanel-suggestions",
    templateUrl: "./suggestions.component.html",
    styleUrls: [ "./suggestions.component.css" ]
})

export class UserpanelSuggestionsComponent implements OnInit, OnDestroy {
    language: {}; 
    myAdminlvl = 0;
    neededAdminlvls: { removeSuggestion: number };
    inSuggestion: number;
    inCreate = false;
    selectedBadge: string;
    private selectedRow: HTMLElement;
    private selectedRowIndex: number;
    private sortingsSmallToBig = {
        ID: true,
        open: false,
        title: false
    };
    static instance: UserpanelSuggestionsComponent;

    badgeNames = [ "suggestion", "bug" ];
    shownBadges = { "suggestion": true, "bug": true };

    rowDatas: { ID: number, Author: String, Open: boolean, Title: string, Topic: string }[] = [
        {
            ID: 1337,
            Author: "Solid_Snake",
            Open: true,
            Title: "Wie werde ich so cool wie Bonus? Solid_Snake suckt hart!",
            Topic: "bug"
        },
        {
            ID: 1702,
            Author: "Bonus",
            Open: false,
            Title: "Wie kann Solid_Snake so schwul sein?",
            Topic: "suggestion"
      }
    ];
    rowDataTexts: { [SuggestionID: number]: { ID: number, Author: string, Text: string, Date: string }[]} = {
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
    formOptions: FormGroup;
    @ViewChild("titleInput", { read: MatInput } ) titleInput: MatInput;
    @ViewChild("textArea", { read: MatInput } ) textArea: MatInput;
    @ViewChild("suggestionTextArea", { read: MatInput } ) suggestionAnswerArea: MatInput;
    @ViewChild("suggestionBadgeList", { read: MatChipList } ) suggestionBadgeList: MatChipList;

    constructor( private rage: RAGE, private fb: FormBuilder ) {
        UserpanelSuggestionsComponent.instance = this;
        UserpanelSuggestionsComponent.instance.rage = rage;
        this.formOptions = fb.group( {
            "color": "accent"
        } );
    }

    ngOnInit() {
        this.rage.Client.call ( {
            fn: "openSuggestionsMenu",
            args: []
        } );  
    }

    ngOnDestroy() {
        this.rage.Client.call( {
            fn: "closeSuggestionsMenu",
            args: []
        } );

        if ( this.inSuggestion !== undefined ) {
            this.rage.Client.call( {
                fn: "closeSuggestion",
                args: []
            } ); 
        }
    }

    static syncSuggestion( data: string ) {
        let _this = UserpanelSuggestionsComponent.instance;
        _this.rowDatas.unshift ( JSON.parse( data ) ); 
        if ( _this.selectedRowIndex !== undefined )
            ++_this.selectedRowIndex;   
        if ( _this.inSuggestion !== undefined )
            ++_this.inSuggestion;
    }

    static syncSuggestions( suggestion: string ) {
        UserpanelSuggestionsComponent.instance.rowDatas = JSON.parse( suggestion );
        UserpanelSuggestionsComponent.instance.selectedRowIndex = undefined;
        UserpanelSuggestionsComponent.instance.inSuggestion = undefined;
    }

    static syncSuggestionText( suggestiontext: string ) {
        UserpanelSuggestionsComponent.instance.rowDataTexts[UserpanelSuggestionsComponent.instance.inSuggestion].push( JSON.parse( suggestiontext ) );    
    }

    static syncSuggestionTexts( suggestiontexts: string ) {
        UserpanelSuggestionsComponent.instance.rowDataTexts[UserpanelSuggestionsComponent.instance.inSuggestion] = JSON.parse( suggestiontexts );
    }

    static syncSuggestionState( suggestionid: number, state: boolean ) {
        for ( let suggestion of UserpanelSuggestionsComponent.instance.rowDatas ) {
            if ( suggestion.ID === suggestionid ) {
                suggestion.Open = state;
                break;
            }
        }
    }

    static syncSuggestionRemove( suggestionid: number ) {
        for ( let index in UserpanelSuggestionsComponent.instance.rowDatas ) {
            if ( UserpanelSuggestionsComponent.instance.rowDatas[index].ID === suggestionid ) {
                let intindex = parseInt( index, 10 );
                if ( UserpanelSuggestionsComponent.instance.inSuggestion === intindex )
                    UserpanelSuggestionsComponent.instance.inSuggestion = undefined;
                if ( UserpanelSuggestionsComponent.instance.selectedRowIndex === intindex )
                    UserpanelSuggestionsComponent.instance.unselectRow();
                UserpanelSuggestionsComponent.instance.rowDatas.splice( intindex, 1 );    
            }
        }
    }

    toggleRowState() {
        UserpanelSuggestionsComponent.instance.rowDatas[this.selectedRowIndex].Open = !UserpanelSuggestionsComponent.instance.rowDatas[this.selectedRowIndex].Open;
        this.rage.Client.call( {
            fn: "toggleSuggestionState",
            args: [this.selectedRowIndex, UserpanelSuggestionsComponent.instance.rowDatas[this.selectedRowIndex].Open ? 1 : 0]
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
            UserpanelSuggestionsComponent.instance.rowDatas.sort ( ( a, b ) => ( a.ID > b.ID ) ? 1 : ( a.ID < b.ID ) ? -1 : 0 );
        else 
            UserpanelSuggestionsComponent.instance.rowDatas.sort ( ( a, b ) => ( a.ID > b.ID ) ? -1 : ( a.ID < b.ID ) ? 1 : 0 ); 
        this.unselectRow();
    }

    sortState() {
        this.sortingsSmallToBig.open = !this.sortingsSmallToBig.open;
        if ( this.sortingsSmallToBig.open )
            UserpanelSuggestionsComponent.instance.rowDatas.sort ( ( a, b ) => a.Open ? 1 : b.Open ? -1 : 0 );
        else 
            UserpanelSuggestionsComponent.instance.rowDatas.sort ( ( a, b ) => a.Open ? -1 : b.Open ? 1 : 0 ); 
        this.unselectRow();
    }

    sortTitle() {
        this.sortingsSmallToBig.title = !this.sortingsSmallToBig.title;
        if ( this.sortingsSmallToBig.title )
            UserpanelSuggestionsComponent.instance.rowDatas.sort ( ( a, b ) => ( a.Title > b.Title ) ? 1 : ( a.Title < b.Title ) ? -1 : 0 );
        else 
            UserpanelSuggestionsComponent.instance.rowDatas.sort ( ( a, b ) => ( a.Title > b.Title ) ? -1 : ( a.Title < b.Title ) ? 1 : 0 ); 
        this.unselectRow();
    }
    
    switchToCreation() {
        this.inCreate = true;
    }

    switchToSuggestion() {
        this.inSuggestion = this.selectedRowIndex;
    }

    removeSuggestion() {   
        this.rage.Client.call( {
            fn: "removeSuggestion",
            args: [this.rowDatas[this.selectedRowIndex].ID]
        } );
        this.rowDatas.splice ( this.selectedRowIndex, 1 ); 
    }

    createSuggestion() {
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

        let chosenbadge = this.suggestionBadgeList.selected;
        if ( !chosenbadge ) {
            this.suggestionBadgeList.errorState = true;
            return;
        } else 
            this.suggestionBadgeList.errorState = false;

        let chosenbadgestr = this.getBadgeNameByLanguage( (<MatChip>chosenbadge).value );

        this.rage.Client.call( {
            fn: "createSuggestion",
            args: [title, text, chosenbadgestr]
        } );

        this.inCreate = false;
    }

    sendSuggestionAnswer() {
        let text = this.suggestionAnswerArea.value || "" as string;
        if ( text.length < 5 || text.length > 250 ) {
            this.suggestionAnswerArea.errorState = true;
            return;
        } else 
            this.suggestionAnswerArea.errorState = false;

        this.rage.Client.call( {
            fn: "addTextToSuggestion",
            args: [this.rowDatas[this.inSuggestion].ID, text]
        } );

        this.suggestionAnswerArea.value = "";
    }

    onSuggestionsShowBadgeChange( badge: string ) {
        this.shownBadges[badge] = !this.shownBadges[badge];
    }

    getBadgeNameByLanguage( langstr: string ) {
        return Object.keys(this.language).find(key => this.language[key] === langstr);
    }
} 