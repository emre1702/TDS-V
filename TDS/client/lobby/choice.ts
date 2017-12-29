/// <reference path="../types-ragemp/index.d.ts" />

let lobbychoicedata = {
	browser: null as MpBrowser
};


mp.events.add( "joinArena", function ( isspectator ) {
	mp.events.callRemote( "joinLobby", 1, isspectator );
} );

mp.events.add( "getLobbyChoiceLanguage", function () {
	log( "getLobbyChoiceLanguage" );
	lobbychoicedata.browser.execute( "getLobbyChoiceLanguage ("+ JSON.stringify( getLang( "lobby_choice" ) )+")" );
} );

mp.events.add( "createLobby", function () {

} );

mp.events.add( "onClientJoinMainMenu", () => {
	log( "onClientJoinMainMenu" );
	lobbychoicedata.browser = mp.browsers.new( "package://TDS-V/window/lobby/choice.html" );
	mp.events.add( 'browserDomReady', ( browser ) => {
		if ( browser == lobbychoicedata.browser ) {
			lobbychoicedata.browser.execute( "getLobbyChoiceLanguage (" + JSON.stringify( getLang( "lobby_choice" ) ) + ")" );
		}
	} );
	mp.gui.cursor.visible = true;
	nothidecursor++;
} );

function destroyLobbyChoiceBrowser() {
	lobbychoicedata.browser.destroy();
	nothidecursor--;
	if ( nothidecursor == 0 )
		mp.gui.cursor.visible = false;
}

mp.events.add( "onClientPlayerJoinLobby", () => {
	log( "onClientPlayerJoinLobby" );
	destroyLobbyChoiceBrowser();
} );

mp.events.add( "onClientPlayerJoinRoundlessLobby", () => {
	log( "onClientPlayerJoinRoundlessLobby" );
	destroyLobbyChoiceBrowser();
} );
	
