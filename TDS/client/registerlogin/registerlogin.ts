/// <reference path="../types-ragemp/index.d.ts" />

let loginpanel = {
	loginbrowser: null as MpBrowser,
	name: null,
	isregistered: null
}


function loginFunc( password ) {
	mp.events.callRemote( "onPlayerTryLogin", password );
}

function registerFunc( password, email ) {
	mp.events.callRemote( "onPlayerTryRegister", password, email );
}

function getLoginPanelData() {
	loginpanel.loginbrowser.execute( "getLoginPanelData ( "+ loginpanel.name+", "+loginpanel.isregistered+", "+JSON.stringify( getLang( "loginregister" ) )+");" );
}

mp.events.add( "startRegisterLogin", function ( eventName, args ) {
	log( "startRegisterLogin registerlogin start" );
	loginpanel.name = args[0];
	loginpanel.isregistered = args[1];
	loginpanel.loginbrowser = mp.browsers.new( "client/window/registerlogin/registerlogin.html" );
	mp.gui.chat.activate( false );
	mp.game.ui.displayHud( false );
	mp.gui.cursor.visible = true;
	nothidecursor++;
	log( "startRegisterLogin registerlogin end" );
} );

mp.events.add( "registerLoginSuccessful", function ( eventName, args ) {
	log( "registerLoginSuccessful registerlogin start" );
	loginpanel.loginbrowser.destroy();
	loginpanel.loginbrowser = null;
	mp.gui.chat.activate( true );
	nothidecursor--;
	log( "registerLoginSuccessful registerlogin end" );
} );

mp.events.callRemote ( "onPlayerJoin" );

