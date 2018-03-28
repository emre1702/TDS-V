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

mp.keys.bind( Keys.U, false, () => {
    if ( ischatopen )
        return; 
    if ( !userpaneldata.open )
        openUserpanel();
    else
        closeUserpanel();
} );