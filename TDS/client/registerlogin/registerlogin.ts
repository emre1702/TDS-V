/// <reference path="../types-gt-mp/index.d.ts" />

let loginpanel = {
	loginbrowser: null,
	name: null,
	isregistered: null
}


function loginFunc( password ) {
	API.triggerServerEvent( "onPlayerTryLogin", password );
}

function registerFunc( password, email ) {
	API.triggerServerEvent( "onPlayerTryRegister", password, email );
}

function getLoginPanelData() {
	loginpanel.loginbrowser.call( "getLoginPanelData", loginpanel.name, loginpanel.isregistered, JSON.stringify ( languagelist[languagesetting].loginregister ) );
}

API.onServerEventTrigger.connect( function ( eventName, args ) {
	if ( eventName == "startRegisterLogin" ) {
		log( "startRegisterLogin registerlogin start" );
		loginpanel.name = args[0];
		loginpanel.isregistered = args[1];
		loginpanel.loginbrowser = API.createCefBrowser( res.Width, res.Height );
		API.waitUntilCefBrowserInit( loginpanel.loginbrowser );
		API.setCefBrowserPosition( loginpanel.loginbrowser, 0, 0 );
		API.setCefBrowserHeadless( loginpanel.loginbrowser, false );
		API.loadPageCefBrowser( loginpanel.loginbrowser, "client/window/registerlogin/registerlogin.html" );
		API.setCanOpenChat( false );
		API.setHudVisible( false );
		API.showCursor( true );
		nothidecursor++;
		log( "startRegisterLogin registerlogin end" );
	} else if ( eventName == "registerLoginSuccessful" ) {
		log( "registerLoginSuccessful registerlogin start" );
		API.destroyCefBrowser( loginpanel.loginbrowser );
		API.setCanOpenChat( true );
		nothidecursor--;
		log( "registerLoginSuccessful registerlogin end" );
	}
} );

API.onResourceStart.connect( function () {
	API.triggerServerEvent( "onPlayerJoin" );
} );



		//if ( eventName == "removeLoginRegisterDraw" ) {
		
