﻿/// <reference path="../types-ragemp/index.d.ts" />


let rounddata = {
	mapinfo: null,
	isspectator: true,
	infight: false
}


function setMapInfo ( mapname ) {
	rounddata.mapinfo = new cText( mapname, res.x * 0.5, res.y * 0.95, 1, { r: 255, g: 255, b: 255, a: 255 }, 0.5, 0.5, true );
}


mp.events.add ( "render", () => {
	if ( !rounddata.infight ) {
		//API.disableControlThisFrame( 24 );
		//API.disableControlThisFrame( 257 );
	}
} );


function removeMapInfo() {
	log( "removeMapInfo start" );
	if ( rounddata.mapinfo != null ) {
		rounddata.mapinfo.remove();
		rounddata.mapinfo = null;
	}
	log( "removeMapInfo end" );
}


function removeRoundThings( removemapinfo ) {
	log( "removeRoundThings start" );
	stopSpectate();
	stopCountdown();
	if ( removemapinfo ) {
		removeMapInfo();
	}
	log( "removeRoundThings end" );
}


function toggleFightMode( bool ) {
	if ( bool ) {
		rounddata.infight = true;
		//API.forceSendAimData( true ); 
	} else {
		rounddata.infight = false;
		//API.forceSendAimData( false );
		stopMapLimitCheck();
	}
}