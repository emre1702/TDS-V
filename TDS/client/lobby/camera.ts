/// <reference path="../types-ragemp/index.d.ts" />

let cameradata = {
	camera: mp.cameras.new( "default" ) as MpCamera,
	moving: false,
	timer: null as Timer,

}


function loadMapMiddleForCamera( mapmiddle ) {
	log( "loadMapMiddleForCamera " + String( mapmiddle ) );
	cameradata.camera.setCoord( mapmiddle.x, mapmiddle.y, mapmiddle.z + 80 );
	cameradata.camera.pointAtCoord( mapmiddle.x, mapmiddle.y, mapmiddle.z );
	cameradata.camera.setActive( true );
	mp.game.cam.renderScriptCams( true, true, 3000, true, true );
	
}


function setCameraGoTowardsPlayer( time = -1 ) {
	log( "setCameraGoTowardsPlayer " + time );
	let pos = gameplayCam.getCoord();
	cameradata.camera.setCoord( pos.x, pos.y, pos.z + 80 );
	mp.game.cam.renderScriptCams( true, true, time == -1 ? ( lobbysettings.countdowntime * 0.9 ) : time, true, true )
}


function stopCountdownCamera() {
	log( "stopCountdownCamera" );
	cameradata.camera.setActive( false );
	//gameplayCam.setActive( true );
}