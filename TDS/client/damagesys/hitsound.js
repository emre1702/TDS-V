"use strict";
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "onClientPlayerHittedOpponent") {
        API.startAudio(soundspath + "hit.wav");
        API.setAudioVolume(0.5);
    }
});
