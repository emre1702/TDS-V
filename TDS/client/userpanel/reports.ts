var reportsdata = {
    inreportsmenu: false,
    inreport: false,
}

// from serverside //
mp.events.add( "syncReports", ( reports: string ) => {
    mainbrowserdata.angular.call( "syncReports(`" + reports + "`);" );
} );

mp.events.add( "syncReportTexts", ( reporttexts: string ) => {
    // SEND TO ANGULAR
} );

mp.events.add( "syncReportState", ( reportid: number, state: boolean ) => {
    mainbrowserdata.angular.call( `syncReportState(${reportid}, ${state});` );
} );

mp.events.add( "syncReport", ( report: string ) => {
    // SEND TO ANGULAR
} );

mp.events.add( "syncReportText", ( reporttext: string ) => {
    // SEND TO ANGULAR
} );


// from browser //


mp.events.add( "addTextToReport", ( reportid: number, text: string ) => {
    mp.events.callRemote( "onPlayerAddTextToReport", reportid, text );
} );

mp.events.add( "createReport", ( json: string, text: string ) => {
    mp.events.callRemote( "onPlayerCreateReport", json, text );
} );

function openReportsMenu() {
    reportsdata.inreportsmenu = true;
    mp.events.callRemote( "onPlayerOpenReportsMenu" );
}

function closeReportsMenu() {
    reportsdata.inreport = false;
    mp.events.callRemote( "onPlayerCloseReportsMenu" );
}

function openReport( index: number ) {
    reportsdata.inreport = true;
    mp.events.callRemote( "onPlayerOpenReport", index );
}

function closeReport() {
    reportsdata.inreport = false;
    mp.events.callRemote( "onPlayerCloseReport" );
}

function toggleReportState( reportid: number, state: number ) {
    mp.events.callRemote( "onPlayerChangeReportState", reportid, state == 1 );
}