/// <reference path="../types-ragemp/index.d.ts" />

let lobbychoicedata = {
	browser: null as MpBrowser
};


function joinArena( isspectator ) {
	mp.events.callRemote( "joinLobby", 1, isspectator );
}

function getLobbyChoiceLanguage() {
	log( "getLobbyChoiceLanguage start" );
	lobbychoicedata.browser.execute( "getLobbyChoiceLanguage ("+ JSON.stringify( getLang( "lobby_choice" ) )+")" );
	log( "getLobbyChoiceLanguage end" );
}

function createLobby() {

}

mp.events.add( "onClientJoinMainMenu", ( args ) => {
	log( "onClientJoinMainMenu start" );
	lobbychoicedata.browser = mp.browsers.new( "client/window/lobby/choice.html" );
	mp.gui.cursor.visible = true;
	nothidecursor++;
	log( "onClientJoinMainMenu end" );
} );

function destroyLobbyChoiceBrowser() {
	lobbychoicedata.browser.destroy();
	nothidecursor--;
	if ( nothidecursor == 0 )
		mp.gui.cursor.visible = false;
}

mp.events.add( "onClientPlayerJoinLobby", ( args ) => {
	log( "onClientPlayerJoinLobby start" );
	destroyLobbyChoiceBrowser();
	log( "onClientPlayerJoinLobby end" );
} );

mp.events.add( "onClientPlayerJoinRoundlessLobby", ( args ) => {
	log( "onClientPlayerJoinRoundlessLobby start" );
	destroyLobbyChoiceBrowser();
	log( "onClientPlayerJoinRoundlessLobby end" );
} );
	
