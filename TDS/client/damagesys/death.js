"use strict";
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "onClientPlayerRespawn") {
        API.fadeScreenIn(2000);
        let localPlayer = API.getLocalPlayer();
        API.callNative("_RESET_LOCALPLAYER_STATE", localPlayer);
        API.callNative("NETWORK_REQUEST_CONTROL_OF_ENTITY", localPlayer);
        let rotation = API.getEntityRotation(localPlayer);
        API.callNative("NETWORK_RESURRECT_LOCAL_PLAYER", 0, 0, 2000, rotation.Z, false, false);
        API.callNative("RESURRECT_PED", localPlayer);
    }
    else if (eventName == "clientPlayerDeathNatives") {
        API.fadeScreenOut(2000);
        API.callNative("_DISABLE_AUTOMATIC_RESPAWN", true);
        API.callNative("IGNORE_NEXT_RESTART", true);
        API.callNative("SET_FADE_OUT_AFTER_DEATH", false);
    }
});
