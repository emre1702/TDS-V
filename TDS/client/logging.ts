/// <reference path="types-ragemp/index.d.ts" />

let activatedlogging = true;

//mp.events.addCommand ( "activatedalogging", ( player, text ) => {
//	activatedlogging = !activatedlogging;
//} );

function log ( message ) {
	if ( activatedlogging ) {
		mp.gui.chat.push( message );
	}
}