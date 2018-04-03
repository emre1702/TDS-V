mp.gui.chat.show( false );

let mainbrowserdata = {
    angular: new Angular(),
    browser: null as BrowserMp,
    roundendreasonshowing: false
}

function addAngularListeners() {
    // browser //
    mainbrowserdata.angular.listen( "requestAngularBrowserData", requestAngularBrowserData );

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
}
addAngularListeners();

function requestAngularBrowserData() {
    return { adminlvl: currentadminlvl, language: getLanguage() }; 
}

mp.events.add( "onClientMoneyChange", money => {
    currentmoney = money;
    mainbrowserdata.browser.execute( "setMoney ( " + money + " );" ); 
} );

mp.events.add( "onClientAdminLvlChange", adminlvl => {
    currentadminlvl = adminlvl;
} );

mp.events.add( "registerLoginSuccessful", ( adminlvl ) => {
    currentadminlvl = adminlvl;
    mainbrowserdata.angular.load( "package://TDS-V/window/mainangular/index.html" );
    mainbrowserdata.browser = mp.browsers.new( "package://TDS-V/window/main/index.html" );
    mainbrowserdata.browser.markAsChat();
} );

function playSound( soundname: string ) {
    mainbrowserdata.browser.execute( "playSound ( '" + soundname + "' );" );
}

// own function because getting called many times at hitsound = on //
function playHitsound() {
    mainbrowserdata.browser.execute( "playHitsound();" );
}

function showBloodscreen() {
    mainbrowserdata.browser.execute( "showBloodscreen ();" );
}

function addKillMessage( msg: string ) {
    mainbrowserdata.browser.execute( "addKillMessage ('" + msg + "');" );
}

function sendAlert ( msg: string ) {
    mainbrowserdata.browser.execute( "alert ('" + msg + "');" );
}

function openMapMenuInBrowser( mapslistjson: string ) {
    mainbrowserdata.browser.execute( "openMapMenu ( '" + settingsdata.language + "', '" + mapslistjson + "');" );
}

function closeMapMenuInBrowser() {
    mainbrowserdata.browser.execute( "closeMapMenu();" );
}

function loadMapVotingsForMapBrowser( mapvotesjson: string ) {
    mainbrowserdata.browser.execute( "loadMapVotings ('" + mapvotesjson + "');" );
}

function clearMapVotingsInBrowser() {
    mainbrowserdata.browser.execute( "clearMapVotings();" );
}

function addVoteToMapInMapMenuBrowser( mapname: string, oldvotemapname: string ) {
    mainbrowserdata.browser.execute( "addVoteToMapVoting('" + mapname + "', '" + oldvotemapname + "');" );
}

function loadMapFavouritesInBrowser( mapfavouritesjson: string ) {
    mainbrowserdata.browser.execute( "loadFavouriteMaps('" + mapfavouritesjson + "');" );
}

function toggleCanVoteForMapWithNumpadInBrowser( bool: boolean ) {
    mainbrowserdata.browser.execute( "toggleCanVoteForMapWithNumpad(" + bool + ");" );
}

function loadOrderNamesInBrowser( ordernamesjson: string ) {
    mainbrowserdata.browser.execute( "loadOrderNames('" + ordernamesjson + "');" );
}

function showRoundEndReason( reason: string, currentmap: string ) {
    mainbrowserdata.roundendreasonshowing = true;
    mainbrowserdata.browser.execute( "showRoundEndReason(`" + reason + "`, `" + currentmap + "`);" );
}

function hideRoundEndReason() {
    if ( mainbrowserdata.roundendreasonshowing ) {
        mainbrowserdata.browser.execute( "hideRoundEndReason();" );
        mainbrowserdata.roundendreasonshowing = false;
    }
}

mp.events.add( "onClientLoadOwnMapRatings", ( data ) => {
    mainbrowserdata.browser.execute( "loadMyMapRatings(`" + data + "`);" );
} );

//from roundend.js//
mp.events.add( "sendMapRating", ( currentmap: string, rating: number ) => {
    callRemoteCooldown( "addRatingToMap", currentmap, rating );
} );