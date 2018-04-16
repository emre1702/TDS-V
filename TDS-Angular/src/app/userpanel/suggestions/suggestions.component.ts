import { Component, Input, ViewChild, OnDestroy, OnInit, Output, EventEmitter } from "@angular/core";
import { UserpanelComponent } from "../userpanel.component";
import { RAGE } from "../../rageconnector/rageconnector.service";
import { MatInput, MatChipListChange, MatChip, MatChipList, MatSelect, MatIconRegistry } from "@angular/material";
import { FormBuilder, FormGroup } from "@angular/forms";
import { DomSanitizer } from "@angular/platform-browser";

enum SuggestionState {
    OPEN, ACCEPTED, DONE, REJECTED
}

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
    inSuggestionID: number;
    inCreate = false;
    selectedBadge: string;
    selectedState = "opened";
    lastSelectedState = "opened";
    loadingSuggestions = false;
    myVote: boolean;
    private selectedRow: HTMLElement;
    private selectedRowIndex: number;
    private sortingsSmallToBig = {
        ID: true,
        open: false,
        title: false
    };
    static instance: UserpanelSuggestionsComponent;

    static badgeNames = [ "suggestion", "bug" ];
    get badgeNames() {
        return UserpanelSuggestionsComponent.badgeNames;
    }
    shownBadges = { "suggestion": true, "bug": true };
    static stateNames = [ "opened", "accepted", "done", "rejected" ];
    get stateNames() {
        return UserpanelSuggestionsComponent.stateNames;
    }
    static stateNumbers = { opened: 0, accepted: 1, done: 2, rejected: 3 };

    rowDatas: { ID: number, Author: String, State: SuggestionState, Title: string, Topic: string }[] = [
        {
            ID: 1337,
            Author: "Solid_Snake",
            State: SuggestionState.OPEN,
            Title: "Wie werde ich so cool wie Bonus? Solid_Snake suckt hart!",
            Topic: "bug"
        },
        {
            ID: 1702,
            Author: "Bonus",
            State: SuggestionState.ACCEPTED,
            Title: "Wie kann Solid_Snake so schwul sein?",
            Topic: "suggestion"
      }
    ];
    rowDataTexts: { [SuggestionID: number]: { ID: number, Author: string, Text: string, Date: string }[]} = {
        1337: [
        {
            ID: 1,
            Author: "Solid_Snake",
            Text: "spdafkp sdakpofsa opdfkpa sdkfpoasdkpo kaspof kopsda kpofdskpoa fpodspfads kfpodas kfposdapok faspokasodp kopfaksdopf oipdsakjioasdjiof jioasdjoifjasdiodjfoi sdajoif as",
            Date: "17:02:15 - 24.07.2018",
        },
        {
            ID: 2,
            Author: "Bonus", 
            Text: "Gar nicht, lel",
            Date: "17:02:14 - 24.07.2018",
        }]
    };
    suggestionVotes: { [SuggestionID: number]: { [Author: string]: number } } = { 
        1337: {
            "Solid_Snake": 1,
            "Bonus": 0,
        }
    };
    formOptions: FormGroup;
    @ViewChild("titleInput", { read: MatInput } ) titleInput: MatInput;
    @ViewChild("textArea", { read: MatInput } ) textArea: MatInput;
    @ViewChild("suggestionTextArea", { read: MatInput } ) suggestionAnswerArea: MatInput;
    @ViewChild("suggestionBadgeList", { read: MatChipList } ) suggestionBadgeList: MatChipList;

    constructor( private rage: RAGE, private fb: FormBuilder, iconRegistry: MatIconRegistry, sanitizer: DomSanitizer ) {
        UserpanelSuggestionsComponent.instance = this;
        UserpanelSuggestionsComponent.instance.rage = rage;
        this.formOptions = fb.group( {
            "color": "accent"
        } );
        iconRegistry.addSvgIcon( "thumb-up", sanitizer.bypassSecurityTrustResourceUrl( "assets/img/thumb_up.svg" ) );
        iconRegistry.addSvgIcon( "thumb-down", sanitizer.bypassSecurityTrustResourceUrl( "assets/img/thumb_down.svg" ) );
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
        let parseddata = JSON.parse( data );
        if ( parseddata.State === _this.selectedState ) {
            _this.rowDatas.unshift (  ); 
            if ( _this.selectedRowIndex !== undefined )
                ++_this.selectedRowIndex;   
            if ( _this.inSuggestion !== undefined )
                ++_this.inSuggestion;
        } // else 
        // maybe inform the user that a new suggestion came in
    }

    static syncSuggestions( suggestions: string ) {
        UserpanelSuggestionsComponent.instance.rowDatas = JSON.parse( suggestions );
        UserpanelSuggestionsComponent.instance.selectedRowIndex = undefined;
        UserpanelSuggestionsComponent.instance.inSuggestion = undefined;
        UserpanelSuggestionsComponent.instance.inSuggestionID = undefined;

        UserpanelSuggestionsComponent.instance.loadingSuggestions = false;
    }

    static syncSuggestionText( suggestiontext: string ) {
        UserpanelSuggestionsComponent.instance.rowDataTexts[UserpanelSuggestionsComponent.instance.inSuggestionID].push( JSON.parse( suggestiontext ) );    
    }

    static syncSuggestionTexts( suggestiontexts: string ) {
        UserpanelSuggestionsComponent.instance.rowDataTexts[UserpanelSuggestionsComponent.instance.inSuggestionID] = JSON.parse( suggestiontexts );
    }

    static syncSuggestionVote( username: string, vote: number ) {
        UserpanelSuggestionsComponent.instance.suggestionVotes[UserpanelSuggestionsComponent.instance.inSuggestionID][username] = vote;
    }

    static syncSuggestionVotes( suggestionvotes: string ) {
        UserpanelSuggestionsComponent.instance.suggestionVotes[UserpanelSuggestionsComponent.instance.inSuggestionID] = JSON.parse( suggestionvotes );
    }

    static syncSuggestionState( suggestionid: number, state: SuggestionState ) {
        for ( let suggestion of UserpanelSuggestionsComponent.instance.rowDatas ) {
            if ( suggestion.ID === suggestionid ) {
                suggestion.State = state;
                break;
            }
        }
    }

    static syncSuggestionRemove( suggestionid: number ) {
        for ( let index in UserpanelSuggestionsComponent.instance.rowDatas ) {
            if ( UserpanelSuggestionsComponent.instance.rowDatas[index].ID === suggestionid ) {
                let intindex = parseInt( index, 10 );
                if ( UserpanelSuggestionsComponent.instance.inSuggestion === intindex ) {
                    UserpanelSuggestionsComponent.instance.inSuggestion = undefined;
                    UserpanelSuggestionsComponent.instance.inSuggestionID = undefined;
                }
                if ( UserpanelSuggestionsComponent.instance.selectedRowIndex === intindex )
                    UserpanelSuggestionsComponent.instance.unselectRow();
                UserpanelSuggestionsComponent.instance.rowDatas.splice( intindex, 1 );    
            }
        }
    }

    toggleRowState( state: SuggestionState ) {
        UserpanelSuggestionsComponent.instance.rowDatas[this.selectedRowIndex].State = state;
        this.rage.Client.call( {
            fn: "changeSuggestionState",
            args: [this.selectedRowIndex, state]
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

    sortTopic() {
        this.sortingsSmallToBig.open = !this.sortingsSmallToBig.open;
        if ( this.sortingsSmallToBig.open )
            UserpanelSuggestionsComponent.instance.rowDatas.sort ( ( a, b ) => a.Topic === "bug" ? 1 : b.Topic === "bug" ? -1 : 0 );
        else 
            UserpanelSuggestionsComponent.instance.rowDatas.sort ( ( a, b ) => a.Topic === "bug"  ? -1 : b.Topic === "bug" ? 1 : 0 ); 
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
        this.inSuggestionID = this.rowDatas[this.inSuggestion].ID;
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
            args: [this.inSuggestionID, text]
        } );

        this.suggestionAnswerArea.value = "";
    }

    onSuggestionsShowBadgeChange( badge: string ) {
        this.shownBadges[badge] = !this.shownBadges[badge];
    }

    getBadgeNameByLanguage( langstr: string ) {
        return Object.keys(this.language).find(key => this.language[key] === langstr);
    }

    onStateSelectionChange() {
        if ( !UserpanelSuggestionsComponent.instance.loadingSuggestions ) {
            this.lastSelectedState = this.selectedState;
            this.rage.Client.call( {
                fn: "requestSuggestionsByState",
                args: [UserpanelSuggestionsComponent.stateNumbers[this.selectedState]]
            } );
            UserpanelSuggestionsComponent.instance.loadingSuggestions = true; 
        } else 
            this.selectedState = this.lastSelectedState;    
    }

    getVotes( val ) {
        let amount = 0;
        for ( let author in this.suggestionVotes[this.inSuggestionID] )
            if ( this.suggestionVotes[this.inSuggestionID][author] === val )
                ++amount;
        return amount;
    }

    toggleVote( bool: boolean ) {
        if ( this.myVote === bool )
            this.myVote = undefined;
        else 
            this.myVote = bool;

        this.rage.Client.call( {
            fn: "toggleSuggestionVote",
            args: [this.inSuggestionID, this.myVote ? 1 : ( this.myVote === false ? 0 : -1 )]
        } );
    }
} 