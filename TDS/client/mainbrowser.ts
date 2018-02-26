mp.gui.chat.show( false );

let mainbrowserdata = {
    browser: mp.browsers.new( "package://TDS-V/window/main/index.html" ) as BrowserMp,
    roundendreasonshowing: false
}
mainbrowserdata.browser.markAsChat();

mp.events.add( "onClientMoneyChange", money => {
    currentmoney = money;
    mainbrowserdata.browser.execute( "setMoney ( " + money + " );" ); 
} );

function playSound( soundname: string ) {
    mainbrowserdata.browser.execute( "playSound ( '" + soundname + "' );" );
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

function openMapMenuInBrowser ( mapslistjson: string ) {
    mainbrowserdata.browser.execute( "openMapMenu ( '" + getLanguage() + "', '" + mapslistjson + "');" );
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
    mp.events.callRemote( "addRatingToMap", currentmap, rating );
} );