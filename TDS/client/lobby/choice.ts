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
	mp.events.add( 'browserDomReady', ( browser ) => {
		if ( browser == lobbychoicedata.browser ) {
            lobbychoicedata.browser.execute( "setLobbyChoiceLanguage (`" + JSON.stringify( getLang( "lobby_choice" ) ) + "`)" );
		}
	} );
	mp.gui.cursor.visible = true;
	nothidecursor++;
} );

function destroyLobbyChoiceBrowser() {
    if ( lobbychoicedata.browser === null )
        return;
    lobbychoicedata.browser.destroy();
    lobbychoicedata.browser = null;
	nothidecursor--;
	if ( nothidecursor == 0 )
		mp.gui.cursor.visible = false;
}

mp.events.add( "onClientPlayerJoinLobby", ( lobbyid ) => {
    if ( lobbyid != 0 ) // mainmenu
	    destroyLobbyChoiceBrowser();
} );