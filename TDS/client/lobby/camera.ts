/// <reference path="../types-gt-mp/index.d.ts" />

let cameradata = {
	camera: null,
	tocamera: null,
	moving: false,
	timer: null,

}


function loadMapMiddleForCamera( mapmiddle ) {
	let camerapos = new Vector3( mapmiddle.X, mapmiddle.Y, mapmiddle.Z + 70 );
	cameradata.camera = API.createCamera( camerapos, new Vector3( 270, 0, 0 ) );
	API.pointCameraAtPosition( cameradata.camera, camerapos );
	API.setActiveCamera( cameradata.camera );
}


function setCameraGoTowardsPlayer() {
	cameradata.tocamera = API.createCamera( API.getGameplayCamPos(), API.getGameplayCamRot() );
	API.interpolateCameras( cameradata.camera, cameradata.tocamera, lobbysettings.countdowntime * 1000 * 0.9, true, true );
}


function stopCountdownCamera() {
	cameradata.tocamera = null;
	cameradata.camera = null;
	API.setActiveCamera( null );
}
