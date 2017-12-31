/// <reference path="../types-ragemp/index.d.ts" />

let cameradata = {
	camera: mp.cameras.new( "mapview" ) as MpCamera,
	moving: false,
	timer: null as Timer,

}


function loadMapMiddleForCamera( mapmiddle ) {
	log( "loadMapMiddleForCamera" );
	cameradata.camera.setCoord( mapmiddle.x, mapmiddle.y, mapmiddle.z + 80 );
	cameradata.camera.pointAtCoord( mapmiddle.x, mapmiddle.y, mapmiddle.z );
	cameradata.camera.setActive( true );
	mp.game.cam.renderScriptCams( true, true, 3000, true, true );
	
}


function setCameraGoTowardsPlayer( time = -1 ) {
	log( "setCameraGoTowardsPlayer " + time );
	let pos = gameplayCam.getCoord();
    //cameradata.camera.setCoord( pos.x, pos.y, pos.z );
    let rot = gameplayCam.getRot ( 2 );
    //cameradata.camera.setRot( rot.x, rot.y, rot.z, 2 );
    //cameradata.camera.setFov( gameplayCam.getFov() );
    //cameradata.camera.setActive( true );
    //cameradata.camera.setActiveWithInterp( gameplayCam.handle, time == -1 ? ( lobbysettings.countdowntime * 0.9 ) : time, 1, 1 );
    cameradata.camera.setParams( pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, gameplayCam.getFov(), time == -1 ? ( lobbysettings.countdowntime * 0.9 ) : time, 1, 1, 2 );
	//mp.game.cam.renderScriptCams( true, true, time == -1 ? ( lobbysettings.countdowntime * 0.9 ) : time, true, true );
}


function stopCountdownCamera() {
	log( "stopCountdownCamera" );
	cameradata.camera.setActive( false );
	mp.game.cam.renderScriptCams( false, false, 0, true, true );
}