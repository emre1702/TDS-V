"use strict";
function playHitSound() {
    API.startAudio(soundspath + "hit.wav", false);
    API.setAudioVolume(0.5);
}
API.onServerEventTrigger.connect(function (eventName, args) {
    if (eventName == "onClientPlayerHittedOpponent") {
        playHitSound();
    }
});
API.onResourceStart.connect(function () {
    API.preloadAudio(soundspath + "hit.wav");
});
