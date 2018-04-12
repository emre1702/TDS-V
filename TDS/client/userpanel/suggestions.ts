var suggestionsdata = {
    insuggestionsmenu: false,
    insuggestion: false,
}

// from serverside //
mp.events.add( "syncSuggestions", ( suggestions: string ) => {
    mainbrowserdata.angular.call( "syncSuggestions(`" + suggestions + "`);" );
} );

mp.events.add( "syncSuggestionText", ( suggestiontext: string ) => {
    mainbrowserdata.angular.call( "syncSuggestionText(`" + suggestiontext + "`);" );
} );

mp.events.add( "syncSuggestionTexts", ( suggestiontexts: string ) => {
    mainbrowserdata.angular.call( "syncSuggestionTexts(`" + suggestiontexts + "`);" );
} );

mp.events.add( "syncSuggestionState", ( suggestionid: number, state: boolean ) => {
    mainbrowserdata.angular.call( `syncSuggestionState(${suggestionid}, ${state});` );
} );

mp.events.add( "syncSuggestion", ( suggestion: string ) => {
    mainbrowserdata.angular.call( `syncSuggestion('${suggestion}');` );
} );

mp.events.add( "syncSuggestionRemove", ( suggestionid: number ) => {
    mainbrowserdata.angular.call( `syncSuggestionRemove (${suggestionid});` );
} );


// from browser //
function addTextToSuggestion( suggestionid: number, text: string ) {
    mp.events.callRemote( "onClientAddTextToSuggestion", suggestionid, text );
}

function createSuggestion( title: string, text: string, topic: string ) {
    mp.events.callRemote( "onClientCreateSuggestion", title, text, topic );
}

function openSuggestionsMenu() {
    suggestionsdata.insuggestionsmenu = true;
    mp.events.callRemote( "onClientOpenSuggestionsMenu" );
}

function closeSuggestionsMenu() {
    suggestionsdata.insuggestion = false;
    mp.events.callRemote( "onPlayerCloseSuggestionsMenu" );
}

function openSuggestion( index: number ) {
    suggestionsdata.insuggestion= true;
    mp.events.callRemote( "onClientOpenSuggestion", index );
}

function closeSuggestion() {
    suggestionsdata.insuggestion = false;
    mp.events.callRemote( "onClientCloseSuggestion" );
}

function toggleSuggestionState( suggestionid: number, state: number ) {
    mp.events.callRemote( "onClientChangeSuggestionState", suggestionid, state == 1 );
}

function removeSuggestion( suggestionid: number ) {
    mp.events.callRemote( "onClientRemoveSuggestion" );
}