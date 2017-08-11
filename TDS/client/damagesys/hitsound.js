"use strict";
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "onClientPlayerHittedOpponent") {
        API.startAudio(soundspath + "hit.wav", false);
        API.setAudioVolume(0.5);
    }
});
API.onResourceStart.connect(function () {
    API.preloadAudio(soundspath + "hit.wav");
});
