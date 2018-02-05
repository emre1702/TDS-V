/// <reference path="../types-ragemp/index.d.ts" />

let lobbychoicedata = {
	browser: null as MpBrowser
};


mp.events.add( "joinArena", function ( isspectator ) {
	mp.events.callRemote( "joinLobby", 1, isspectator );
} );

mp.events.add( "joinMapCreatorLobby", function () {
    mp.events.callRemote( "joinMapCreatorLobby" );
} );

mp.events.add( "getLobbyChoiceLanguage", function () {
	log( "getLobbyChoiceLanguage" );
    lobbychoicedata.browser.execute( "setLobbyChoiceLanguage (`" + JSON.stringify( getLang( "lobby_choice" ) ) +"`)" );
} );

mp.events.add( "createLobby", function () {

} );

mp.events.add( "onClientJoinMainMenu", () => {
	log( "onClientJoinMainMenu" );
	lobbychoicedata.browser = mp.browsers.new( "package://TDS-V/window/choice/index.html" );
    toggleCursor( true );
} );

function destroyLobbyChoiceBrowser() {
    if ( lobbychoicedata.browser === null )
        return;
    lobbychoicedata.browser.destroy();
    lobbychoicedata.browser = null;
    toggleCursor( false );
}

mp.events.add( "onClientPlayerJoinLobby", ( lobbyid ) => {
    if ( lobbyid != 0 ) // mainmenu
	    destroyLobbyChoiceBrowser();
} );