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
	loginpanel.isregistered = isregistered == 1;
	loginpanel.loginbrowser = mp.browsers.new( "package://TDS-V/window/registerlogin/registerlogin.html" );
	mp.events.add( 'browserDomReady', ( browser: MpBrowser ) => {
		if ( browser == loginpanel.loginbrowser )
            browser.execute( "getLoginPanelData ( `" + loginpanel.name + "`, `" + loginpanel.isregistered + "`, `" + JSON.stringify( getLang( "loginregister" ) ) + "` );" );
    } );
    mp.gui.chat.show( false );
	mp.game.ui.displayHud( false );
	mp.gui.cursor.visible = true;
	nothidecursor++;
} );

mp.events.add( "registerLoginSuccessful", function () {
	log( "registerLoginSuccessful registerlogin" );
	loginpanel.loginbrowser.destroy();
	loginpanel.loginbrowser = null;
    mp.gui.chat.show( true );
    mp.game.ui.displayHud( true );
	nothidecursor--;
} );