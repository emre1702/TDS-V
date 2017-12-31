/// <reference path="../types-ragemp/index.d.ts" />


let rounddata = {
	mapinfo: null as cText,
	isspectator: true,
	infight: false
}


function setMapInfo ( mapname ) {
    rounddata.mapinfo = new cText( mapname, 0.5, 0.95, 1, [255, 255, 255, 255], [0.5, 0.5], true, Alignment.CENTER, true );
}


mp.events.add ( "render", () => {
	if ( !rounddata.infight ) {
		//API.disableControlThisFrame( 24 );
		//API.disableControlThisFrame( 257 );
	}
} );


function removeMapInfo() {
	log( "removeMapInfo" );
	if ( rounddata.mapinfo != null ) {
		rounddata.mapinfo.remove();
		rounddata.mapinfo = null;
	}
}


function removeRoundThings( removemapinfo ) {
	log( "removeRoundThings" );
	stopSpectate();
	stopCountdown();
	if ( removemapinfo ) {
		removeMapInfo();
	}
}


function toggleFightMode( bool ) {
	log( "toggleFightMode " + bool );
	if ( bool ) {
		rounddata.infight = true;
		//API.forceSendAimData( true ); 
	} else {
		rounddata.infight = false;
		//API.forceSendAimData( false );
		stopMapLimitCheck();
	}
}