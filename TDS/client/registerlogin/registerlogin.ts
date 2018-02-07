/// <reference path="../types-ragemp/index.d.ts" />

let loginpanel = {
	loginbrowser: null as MpBrowser,
	name: null as string,
	isregistered: null
}


mp.events.add( "loginFunc", function ( password ) {
	mp.events.callRemote( "onPlayerTryLogin", password );
} );

mp.events.add( "registerFunc", function ( password, email ) {
	mp.events.callRemote( "onPlayerTryRegister", password, email );
} );

mp.events.add( "getRegisterLoginLanguage", () => {
    loginpanel.loginbrowser.execute( "loadLanguage ( `" + JSON.stringify( getLang( "loginregister" ) ) + "` );" );
} );

mp.events.add( "startRegisterLogin", function ( name: string, isregistered ) {
	log( "startRegisterLogin registerlogin" );
	loginpanel.name = name;
    loginpanel.isregistered = isregistered;
    loginpanel.loginbrowser = mp.browsers.new( "package://TDS-V/window/registerlogin/index.html" );
    mp.gui.chat.activate( false );
    toggleCursor( true );
} );

mp.events.add( "registerLoginSuccessful", function () {
    log( "registerLoginSuccessful registerlogin" );
	loginpanel.loginbrowser.destroy();
	loginpanel.loginbrowser = null;
    mp.gui.chat.activate( true );
    toggleCursor( false );
} );