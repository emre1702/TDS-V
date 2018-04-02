var reportsdata = {
    inreportsmenu: false,
    inreport: false,
}

// from serverside //
mp.events.add( "syncReports", ( reports: string ) => {
    mainbrowserdata.angular.call( "syncReports(`" + reports + "`);" );
} );

mp.events.add( "syncReportText", ( reporttext: string ) => {
    mainbrowserdata.angular.call( "syncReportText(`" + reporttext + "`);" );
} );

mp.events.add( "syncReportTexts", ( reporttexts: string ) => {
    mainbrowserdata.angular.call( "syncReportTexts(`"+ reporttexts +"`);" );
} );

mp.events.add( "syncReportState", ( reportid: number, state: boolean ) => {
    mainbrowserdata.angular.call( `syncReportState(${reportid}, ${state});` );
} );

mp.events.add( "syncReport", ( report: string ) => {
    mainbrowserdata.angular.call( `syncReport('${report}');` );
} );

mp.events.add( "syncReportRemove", ( reportid: number ) => {
    mainbrowserdata.angular.call( `syncReportRemove (${reportid});` );
} );


// from browser //
function addTextToReport ( reportid: number, text: string ) {
    mp.events.callRemote( "onClientAddTextToReport", reportid, text );
}

function createReport ( title: string, text: string, forminadminlvl: number ) {
    mp.events.callRemote( "onClientCreateReport", title, text, forminadminlvl );
}

function openReportsMenu() {
    reportsdata.inreportsmenu = true;
    mp.events.callRemote( "onClientOpenReportsMenu" );
}

function closeReportsMenu() {
    reportsdata.inreport = false;
    mp.events.callRemote( "onPlayerCloseReportsMenu" );
}

function openReport( index: number ) {
    reportsdata.inreport = true;
    mp.events.callRemote( "onClientOpenReport", index );
}

function closeReport() {
    reportsdata.inreport = false;
    mp.events.callRemote( "onClientCloseReport" );
}

function toggleReportState( reportid: number, state: number ) {
    mp.events.callRemote( "onClientChangeReportState", reportid, state == 1 );
}

function removeReport( reportid: number ) {
    mp.events.callRemote( "onClientRemoveReport" );
}