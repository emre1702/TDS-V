"use strict";
let cameradata = {
    camera: null,
    moving: false,
    timer: null,
};
function loadMapMiddleForCamera(mapmiddle) {
    log("loadMapMiddleForCamera " + String(mapmiddle));
    cameradata.camera = mp.cameras.new("mapview");
    cameradata.camera.setCoord(mapmiddle.x, mapmiddle.y, mapmiddle.z + 80);
    cameradata.camera.pointAtCoord(mapmiddle.x, mapmiddle.y, mapmiddle.z);
    cameradata.camera.setActive(true);
    mp.game.cam.renderScriptCams(true, true, 3000, true, true);
}
function setCameraGoTowardsPlayer(time = -1) {
    log("setCameraGoTowardsPlayer " + time);
    cameradata.camera.setActiveWithInterp(gameplayCam, time == -1 ? (lobbysettings.countdowntime * 0.9) : time, true, true);
}
function stopCountdownCamera() {
    log("stopCountdownCamera");
    cameradata.camera.setActive(false);
    cameradata.camera.destroy();
    cameradata.camera = null;
}
