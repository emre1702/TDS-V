/// <reference path="../types-gt-mp/index.d.ts" />

API.onServerEventTrigger.connect( function ( eventName, args ) {
	if ( eventName == "onClientPlayerHittedOpponent" ) {
		API.startAudio( soundspath+"hit.wav" );
		API.setAudioVolume( 0.5 );
	}
} );