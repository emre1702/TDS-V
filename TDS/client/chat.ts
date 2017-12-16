/// <reference path="types-ragemp/index.d.ts" />
 /*
let chatdata = {
	chat: null,
	browser: mp.browsers.new( "client/window/chat/chat.html" ),
	chatopen: false,
}
chatdata.browser.markAsChat();


API.onKeyDown.connect( (sender, e) => {
	if ( chatdata.chatopen == false ) {
		if ( API.getCanOpenChat() ) {
			if ( e.KeyCode == Keys.Z ) {
				onFocusChange( true, false, "/globalsay " );
			}
		}
	}
} );

API.onResourceStop.connect( () => {
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
}*/