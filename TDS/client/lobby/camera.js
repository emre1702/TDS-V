"use strict";
let cameradata = {
    camera: null,
    tocamera: null,
    moving: false,
    timer: null,
};
function loadMapMiddleForCamera(mapmiddle) {
    let camerapos = new Vector3(mapmiddle.X, mapmiddle.Y, mapmiddle.Z + 80);
    cameradata.camera = API.createCamera(camerapos, new Vector3(270, 0, 0));
    API.pointCameraAtPosition(cameradata.camera, camerapos);
    API.setActiveCamera(cameradata.camera);
}
function setCameraGoTowardsPlayer(time = -1) {
    cameradata.tocamera = API.createCamera(API.getGameplayCamPos(), API.getGameplayCamRot());
    API.interpolateCameras(cameradata.camera, cameradata.tocamera, time == -1 ? (lobbysettings.countdowntime * 1000 * 0.9) : time, true, true);
}
function stopCountdownCamera() {
    cameradata.tocamera = null;
    cameradata.camera = null;
    API.setActiveCamera(null);
}
