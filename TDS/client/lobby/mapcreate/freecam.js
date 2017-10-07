"use strict";
let freecamdata = {
    CameraObject: null,
    cam: null,
    wdown: false,
    sdown: false,
    adown: false,
    ddown: false,
    shiftdown: false,
    altdown: false,
    freecamMode: false,
    toggleControl: true,
    lastPos: null,
    keydownevent: null,
    keyupevent: null,
    onupdateevent: null
};
API.onKeyDown.connect(function (sender, e) {
    if (e.KeyCode === Keys.W)
        freecamdata.wdown = true;
    if (e.KeyCode === Keys.A)
        freecamdata.adown = true;
    if (e.KeyCode === Keys.S)
        freecamdata.sdown = true;
    if (e.KeyCode === Keys.D)
        freecamdata.ddown = true;
    if (e.KeyCode === Keys.RShiftKey || e.KeyCode === Keys.ShiftKey)
        freecamdata.shiftdown = true;
    if (e.KeyCode === Keys.Menu || e.KeyCode === Keys.RMenu)
        freecamdata.altdown = true;
});
API.onKeyUp.connect(function (sender, e) {
    if (e.KeyCode === Keys.W)
        freecamdata.wdown = false;
    if (e.KeyCode === Keys.A)
        freecamdata.adown = false;
    if (e.KeyCode === Keys.S)
        freecamdata.sdown = false;
    if (e.KeyCode === Keys.D)
        freecamdata.ddown = false;
    if (e.KeyCode === Keys.RShiftKey || e.KeyCode === Keys.ShiftKey)
        freecamdata.shiftdown = false;
    if (e.KeyCode === Keys.Menu || e.KeyCode === Keys.RMenu)
        freecamdata.altdown = false;
});
API.onUpdate.connect(function () {
    if (freecamdata.freecamMode && freecamdata.toggleControl) {
        API.disableControlThisFrame(16);
        API.disableControlThisFrame(17);
        API.disableControlThisFrame(26);
        API.disableControlThisFrame(24);
        var camRot = API.getGameplayCamRot();
        var camDir = API.getGameplayCamDir();
        if (freecamdata.cam != null) {
            var to = null;
            var pos2 = null;
            var camPos = API.getEntityPosition(freecamdata.CameraObject);
            var multiply = 1;
            if (freecamdata.shiftdown)
                multiply = 3;
            if (freecamdata.altdown)
                multiply = 0.5;
            if (freecamdata.wdown) {
                to = Vector3Lerp(camPos, camPos.Add(camDir.Multiply(multiply)), 1.0);
            }
            if (freecamdata.sdown) {
                if (to != null)
                    camPos = to;
                to = Vector3Lerp(camPos, camPos.Subtract(camDir.Multiply(multiply)), 1.0);
            }
            if (freecamdata.adown) {
                if (to != null)
                    camPos = to;
                pos2 = getPositionInFront(multiply, camPos, camRot.Z, 90);
                to = Vector3Lerp(camPos, pos2, 1.0);
            }
            if (freecamdata.ddown) {
                if (to != null)
                    camPos = to;
                pos2 = getPositionInFront(multiply, camPos, camRot.Z, -90);
                to = Vector3Lerp(camPos, pos2, 1.0);
            }
            if (to != null && freecamdata.CameraObject != null) {
                API.triggerServerEvent("setFreecamObjectPositionTo", to);
            }
            API.setEntityRotation(API.getLocalPlayer(), new Vector3(0.0, 0.0, camRot.Z));
            if (camRot != API.getCameraRotation(freecamdata.cam)) {
                API.setCameraRotation(freecamdata.cam, camRot);
            }
            if (freecamdata.lastPos != null && camPos != null && freecamdata.lastPos.DistanceTo(camPos) > 100.0)
                API.callNative("_SET_FOCUS_AREA", camPos.X, camPos.Y, camPos.Z, freecamdata.lastPos.X, freecamdata.lastPos.Y, freecamdata.lastPos.Z);
            freecamdata.lastPos = camPos;
        }
    }
});
function startFreecam(object) {
    freecamdata.CameraObject = object;
    freecamdata.cam = API.createCamera(API.getEntityPosition(API.getLocalPlayer()), new Vector3(0.0, 0.0, 0.0));
    API.attachCameraToEntity(freecamdata.cam, freecamdata.CameraObject, new Vector3(0.0, 0.0, 0.0));
    API.setEntityCollisionless(freecamdata.CameraObject, true);
    API.setActiveCamera(freecamdata.cam);
    API.callNative("DISPLAY_RADAR", false);
    freecamdata.freecamMode = true;
}
function stopFreecam() {
    if (freecamdata.keydownevent != null) {
        freecamdata.keydownevent.disconnect();
        freecamdata.keydownevent = null;
    }
    if (freecamdata.keyupevent != null) {
        freecamdata.keyupevent.disconnect();
        freecamdata.keyupevent = null;
    }
    if (freecamdata.onupdateevent != null) {
        freecamdata.onupdateevent.disconnect();
        freecamdata.onupdateevent = null;
    }
    freecamdata = {
        CameraObject: null,
        cam: null,
        wdown: false,
        sdown: false,
        adown: false,
        ddown: false,
        shiftdown: false,
        altdown: false,
        freecamMode: false,
        toggleControl: true,
        lastPos: null,
        keydownevent: null,
        keyupevent: null,
        onupdateevent: null
    };
    API.callNative("DISPLAY_RADAR", true);
    API.callNative("SET_FOCUS_ENTITY", API.getLocalPlayer());
}
API.onServerEventTrigger.connect(function (name, args) {
    if (name == "startFreecam") {
        startFreecam(args[0]);
    }
    else if (name == "stopFreecam") {
        stopFreecam();
    }
});
function clampAngle(angle) {
    return (angle + Math.ceil(-angle / 360) * 360);
}
function getPositionInFront(range, pos, zrot, plusangle) {
    var angle = clampAngle(zrot) * (Math.PI / 180);
    plusangle = (clampAngle(plusangle) * (Math.PI / 180));
    pos.X += (range * Math.sin(-angle - plusangle));
    pos.Y += (range * Math.cos(-angle - plusangle));
    return pos;
}
