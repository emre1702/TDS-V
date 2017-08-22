/// <reference path="types-gt-mp/index.d.ts" />

let activatedlogging = false;

API.onChatCommand.connect( function ( message ) {
	if ( message == "/activatedalogging" )
		activatedlogging = !activatedlogging;
} );

function log ( message ) {
	if ( activatedlogging ) {
		API.sendChatMessage( message.toString() );
	}
}