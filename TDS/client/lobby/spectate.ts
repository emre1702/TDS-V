/// <reference path="../types-ragemp/index.d.ts" />

let spectatedata = {
	binded: false
}


function pressSpectateKeyLeft() {
    callRemoteCooldown( "spectateNext", false );
}


function pressSpectateKeyRight() {
    callRemoteCooldown( "spectateNext", true );
}


function startSpectate() {
	if ( !spectatedata.binded ) {
		mp.keys.bind( Keys.LeftArrow, false, pressSpectateKeyLeft );
		mp.keys.bind( Keys.A, false, pressSpectateKeyLeft );
		mp.keys.bind( Keys.RightArrow, false, pressSpectateKeyRight );
		mp.keys.bind( Keys.D, false, pressSpectateKeyRight );
		spectatedata.binded = true;
	}
}


function stopSpectate() {
	if ( spectatedata.binded ) {
		mp.keys.unbind( Keys.LeftArrow, pressSpectateKeyLeft );
		mp.keys.unbind( Keys.A, pressSpectateKeyLeft );
		mp.keys.unbind( Keys.RightArrow, pressSpectateKeyRight );
		mp.keys.unbind( Keys.D, pressSpectateKeyRight );
		spectatedata.binded = false;
	}
}