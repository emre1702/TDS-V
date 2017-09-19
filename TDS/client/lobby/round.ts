/// <reference path="../types-gt-mp/index.d.ts" />


let rounddata = {
	mapinfo: null,
	isspectator: true,
	infight: false
}


function setMapInfo ( mapname ) {
	rounddata.mapinfo = new cText( mapname, res.Width * 0.5, res.Height * 0.95, 0.5, 255, 255, 255, 255, 0, 1, true );
}


API.onUpdate.connect( function () {
	if ( !rounddata.infight ) {
		API.disableControlThisFrame( 24 );
		API.disableControlThisFrame( 257 );
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
		API.forceSendAimData( true ); 
	} else {
		rounddata.infight = false;
		API.forceSendAimData( false );
		stopMapLimitCheck();
	}
}