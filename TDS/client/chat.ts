﻿/// <reference path="types-gt-mp/index.d.ts" />

let chatdata = {
	chat: null,
	browser: null,
	chatopen: false,
}


API.onResourceStart.connect( function () {
	chatdata.browser = API.createCefBrowser( res.Width, res.Height );
	API.waitUntilCefBrowserInit( chatdata.browser );
	API.setCefBrowserPosition( chatdata.browser, 0, 0 );
	API.loadPageCefBrowser( chatdata.browser, "client/window/chat/chat.html" );

	chatdata.chat = API.registerChatOverride();

	chatdata.chat.onAddMessageRequest.connect( addMessage );
	chatdata.chat.onChatHideRequest.connect( onChatHide );
	chatdata.chat.onFocusChange.connect( onFocusChange );

	chatdata.chat.SanitationLevel = 2;
} );


API.onKeyDown.connect( function ( sender, e ) {
	if ( chatdata.chatopen == false ) {
		if ( API.getCanOpenChat() ) {
			if ( e.KeyCode == Keys.Z ) {
				onFocusChange( true, false, "/globalsay " );
			}
		}
	}
} );

API.onResourceStop.connect( function () {
	if ( chatdata.browser != null ) {
		var localCopy = chatdata.browser;
		chatdata.browser = null;
		API.destroyCefBrowser( localCopy );
	}
} );

function commitMessage( msg ) {
	chatdata.chat.sendMessage( msg );
}

function addMessage( msg, hasColor, r, g, b ) {
	if ( chatdata.browser != null ) {
		//if (!hasColor) {
		chatdata.browser.call( "addMessage", msg );
		//} else {

		//}
	}
}

function onFocusChange( focus, fromcef = false, cmd = "" ) {
	if ( chatdata.browser != null ) {
		if ( fromcef == false )
			chatdata.browser.call( "setFocus", focus, true, cmd );
	}
	if ( !focus ) {
		if ( chatdata.chatopen ) {
			chatdata.chat.sendMessage( "" );
			chatdata.chatopen = false;
			if ( API.isCursorShown() ) {
				nothidecursor--;
				if ( nothidecursor == 0 )
					API.showCursor( false );
			}
		}

	} else {
		chatdata.chatopen = true;
		nothidecursor++;
		API.showCursor( true );
	}
}

function onChatHide( hide ) {
	if ( chatdata.browser != null ) {
		API.setCefBrowserHeadless( chatdata.browser, hide );
	}
}

function onChatLoad() {
	API.triggerServerEvent( "onPlayerChatLoad", languagesetting );
}