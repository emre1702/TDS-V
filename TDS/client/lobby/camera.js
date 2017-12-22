"use strict";
let cameradata = {
    camera: null,
    tocamera: null,
    moving: false,
    timer: null,
};
function loadMapMiddleForCamera(mapmiddle) {
    let camerapos = { x: mapmiddle.x, y: mapmiddle.x, z: mapmiddle.z + 80 };
    cameradata.camera = mp.cameras.new(camerapos, { x: 270, y: 0, z: 0 });
    cameradata.camera.pointAtCoord(mapmiddle.x, mapmiddle.y, mapmiddle.z);
    cameradata.camera.setActive(true);
}
function setCameraGoTowardsPlayer(time = -1) {
    cameradata.tocamera = mp.cameras.new(gameplayCam.getCoord(), mp.game.cam.getGameplayCamRot(0));
    cameradata.camera.setActiveWithInterp(cameradata.tocamera, time == -1 ? (lobbysettings.countdowntime * 0.9) : time, true, true);
}
function stopCountdownCamera() {
    cameradata.camera.destroy();
    cameradata.camera = null;
    cameradata.tocamera.destroy();
    cameradata.tocamera = null;
    cameradata.camera.setActive(false);
}
