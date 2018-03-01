/// <reference path="../types-ragemp/index.d.ts" />

let lobbychoicedata = {
    browser: null as BrowserMp
};


mp.events.add( "joinArena", function ( isspectator ) {
    callRemoteCooldown( "joinLobby", 1, isspectator );
} );

mp.events.add( "joinMapCreatorLobby", function () {
    callRemoteCooldown( "joinMapCreatorLobby" );
} );

mp.events.add( "getLobbyChoiceLanguage", function () {
	log( "getLobbyChoiceLanguage" );
    lobbychoicedata.browser.execute( "setLobbyChoiceLanguage (`" + JSON.stringify( getLang( "lobby_choice" ) ) +"`)" );
} );

mp.events.add( "createLobby", function () {

} );

function startLobbyChoiceBrowser() {
	lobbychoicedata.browser = mp.browsers.new( "package://TDS-V/window/choice/index.html" );
    toggleCursor( true );
}

function destroyLobbyChoiceBrowser() {
    if ( lobbychoicedata.browser === null )
        return;
    lobbychoicedata.browser.destroy();
    lobbychoicedata.browser = null;
    toggleCursor( false );
}