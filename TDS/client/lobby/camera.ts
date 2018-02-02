/// <reference path="../types-ragemp/index.d.ts" />

let cameradata = {
	camera: mp.cameras.new( "mapview" ) as MpCamera,
	moving: false,
	timer: null as Timer,
}


function loadMapMiddleForCamera( mapmiddle ) {
	log( "loadMapMiddleForCamera" );
	cameradata.camera.setCoord( mapmiddle.x, mapmiddle.y, mapmiddle.z + 110 );
	cameradata.camera.pointAtCoord( mapmiddle.x, mapmiddle.y, mapmiddle.z );
	cameradata.camera.setActive( true );
	mp.game.cam.renderScriptCams( true, true, 3000, true, true );
	
}

function setCameraGoTowardsPlayer( time = -1 ) {
	log( "setCameraGoTowardsPlayer " + time );
    cameradata.camera.setActive( false );
    mp.game.cam.renderScriptCams( false, true, time == -1 ? ( lobbysettings.countdowntime * 0.9 ) : time, true, true );
}


function stopCountdownCamera() {
	mp.game.cam.renderScriptCams( false, false, 0, true, true );

}