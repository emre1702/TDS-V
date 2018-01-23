let mainbrowserdata = {
    browser: mp.browsers.new( "package://TDS-V/window/main/index.html" ) as MpBrowser,
}

mp.events.add( "onClientMoneyChange", money => {
    currentmoney = money;
    mainbrowserdata.browser.execute( "setMoney ( " + money + " );" ); 
} );

mp.events.add( "browserDomReady", ( browser ) => {
    if ( browser == mainbrowserdata.browser ) {
        loadOrderNamesInBrowser( JSON.stringify( getLang( "orders" ) ) );
    }
})

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
    mainbrowserdata.browser.execute( "addVoteToMapVoting ( '" + mapname + "', '" + oldvotemapname + "' ); " );
}

function loadMapFavouritesInBrowser( mapfavouritesjson: string ) {
    mainbrowserdata.browser.execute( "loadFavouriteMaps ( '" + mapfavouritesjson + "');" );
}

function toggleCanVoteForMapWithNumpadInBrowser( bool: boolean ) {
    mainbrowserdata.browser.execute( "toggleCanVoteForMapWithNumpad ( " + bool + " )" );
}

function loadOrderNamesInBrowser( ordernamesjson: string ) {
    mainbrowserdata.browser.execute( "loadOrderNames ( '" + ordernamesjson + "');" );
}