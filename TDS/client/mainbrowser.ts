let mainbrowserdata = {
    browser: mp.browsers.new( "package://TDS-V/window/main/index.html" ) as MpBrowser,
}

mp.events.add( "onClientMoneyChange", money => {
    currentmoney = money;
    mainbrowserdata.browser.execute( "setMoney ( " + money + " );" ); 

     //   moneydata.text = new cText( "$" + currentmoney, 0.99, 0.01, 7, [115, 186, 131, 255], [1.0, 1.0], true, Alignment.RIGHT, true );
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
    mainbrowserdata.browser.execute( "addVoteToMapVoting ( '" + mapname + "', '" + oldvotemapname + "' ); " );
}

function loadMapFavouritesInBrowser( mapfavourites ) {
    mainbrowserdata.browser.execute( "loadFavouriteMaps ( '" + mapfavourites + "');" );
}