/// <reference path="../types-gt-mp/index.d.ts" />

let spectatedata = {
	event: null,
}


function pressSpectateKey( sender, e ) {
	if ( e.KeyCode == Keys.Left || e.KeyCode == Keys.A ) {
		API.triggerServerEvent( "spectateNext", false );
	} else if ( e.KeyCode == Keys.Right || e.KeyCode == Keys.D ) {
		API.triggerServerEvent( "spectateNext", true );
	}
}


function startSpectate() {
	if ( spectatedata.event == null )
		spectatedata.event = API.onKeyDown.connect( pressSpectateKey );
}


function stopSpectate() {
	if ( spectatedata.event != null ) {
		spectatedata.event.disconnect();
		spectatedata.event = null;
	}
}