let userpaneldata = {
    open: false
}

function openUserpanel() {
    mainbrowserdata.angular.call( "openUserpanel();" );
    toggleCursor( true );
    userpaneldata.open = true;
}

function closeUserpanel() {
    mainbrowserdata.angular.call( "closeUserpanel();" );

    if ( reportsdata.inreport ) {
        reportsdata.inreport = false;
        mp.events.callRemote( "onPlayerCloseReport" );
    }
    if ( reportsdata.inreportsmenu ) {
        reportsdata.inreportsmenu = false;
        mp.events.callRemote( "onPlayerCloseReportsMenu" );
    }

    toggleCursor( false );
    userpaneldata.open = false;
}

function addUserpanelFunctionsToAngular() {
    // userpanel //
    mainbrowserdata.angular.listen( "closeUserpanel", closeUserpanel );
    mainbrowserdata.angular.listen( "requestLanguage", getLanguage );

    // reports //
    mainbrowserdata.angular.listen( "createReport", createReport );
    mainbrowserdata.angular.listen( "openReportsMenu", openReportsMenu );
    mainbrowserdata.angular.listen( "closeReportsMenu", closeReportsMenu );
    mainbrowserdata.angular.listen( "openReport", openReport );
    mainbrowserdata.angular.listen( "closeReport", closeReport );
    mainbrowserdata.angular.listen( "toggleReportState", toggleReportState );
    mainbrowserdata.angular.listen( "removeReport", removeReport );
    mainbrowserdata.angular.listen( "addTextToReport", addTextToReport );

    // suggestions //
    mainbrowserdata.angular.listen( "createSuggestion", createSuggestion );
    mainbrowserdata.angular.listen( "openSuggestionsMenu", openSuggestionsMenu );
    mainbrowserdata.angular.listen( "closeSuggestionsMenu", closeSuggestionsMenu );
    mainbrowserdata.angular.listen( "openSuggestion", openSuggestion );
    mainbrowserdata.angular.listen( "closeSuggestion", closeSuggestion );
    mainbrowserdata.angular.listen( "toggleSuggestionState", toggleSuggestionState );
    mainbrowserdata.angular.listen( "removeSuggestion", removeSuggestion );
    mainbrowserdata.angular.listen( "addTextToSuggestion", addTextToSuggestion );
}

mp.keys.bind( Keys.U, false, () => {
    if ( ischatopen )
        return; 
    if ( !userpaneldata.open )
        openUserpanel();
    else
        closeUserpanel();
} );