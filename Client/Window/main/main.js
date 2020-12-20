let bloodscreen = $("#bloodscreen");
let bloodscreenDom;
let language = 9;
let ordersDiv = $("#orders");
let bloodscreentimeout;

let hitsound = $("#audio_hit");
let hitsounds = [
    hitsound, hitsound.clone(true),
    hitsound.clone(true), hitsound.clone(true),
    hitsound.clone(true), hitsound.clone(true),
    hitsound.clone(true), hitsound.clone(true),
    hitsound.clone(true), hitsound.clone(true)];
let hitsoundcounter = 0;
let hitsoundsamount = hitsounds.length;

let bombTickSound = $("#audio_bombTick");
let bombTickSounds = [
    bombTickSound, bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true),
    bombTickSound.clone(true), bombTickSound.clone(true)];
let bombTickCounter = 0;
let bombTickAmount = bombTickSounds.length;
let bombTickTimeout;
let volume = 0.05;

let killstreakSoundPlaying = false;
let nextKillstreakSoundNames = [];

// playSound
mp.events.add("a", (soundName) => {
    $("#audio_" + soundName).trigger("play").prop("volume", volume);
});

// playHitsound
mp.events.add("b", () => {
	hitsounds[hitsoundcounter++].trigger("play").prop("volume", volume);
    if (hitsoundcounter == hitsoundsamount)
        hitsoundcounter = 0;
});


function playBombTickSound() {
    bombTickSounds[bombTickCounter++].trigger("play").prop("volume", volume);
    if (bombTickCounter == bombTickAmount)
        bombTickCounter = 0;
}

function playKillstreakSound(soundName) {
    if (killstreakSoundPlaying) {
        nextKillstreakSoundNames.push(soundName);
    } else {
        $("#audio_" + soundname).trigger("play").prop("volume", volume);
        killstreakSoundPlaying = true;
    }
}

function onKillstreakSoundEnded() {
    killstreakSoundPlaying = false;
    if (nextKillstreakSoundNames.length == 0) {
        return;
    }
    let newSoundName = nextKillstreakSoundNames.shift();
    playKillstreakSound(newSoundName);
}

function startBombTickSound(msToEnd, startAtMs) {
    playBombTickSound();
    let atPercentage = startAtMs / msToEnd;
    let currentSpeed = 3000 - 2950 * atPercentage;
    if (bombTickTimeout != null)
        clearTimeout(bombTickTimeout);
    bombTickTimeout = setTimeout(() => startBombTickSound(msToEnd, startAtMs + currentSpeed), currentSpeed);
}

function stopBombTickSound() {
    if (bombTickTimeout != null) {
        clearTimeout(bombTickTimeout);
        bombTickTimeout = null;
    }
}

mp.events.add("c", () => {
    if (bloodscreentimeout) {
        clearTimeout(bloodscreentimeout);

        bloodscreenDom.style.animation = "none";
        bloodscreenDom.offsetHeight;
        bloodscreenDom.style.animation = "BloodscreenAnim 2.5s";
    } else {
        bloodscreen.css({
            "animation": "BloodscreenAnim 2.5s",
            "animation-fill-mode": "forwards"
        });
    }
    bloodscreentimeout = setTimeout(function () {
        bloodscreentimeout = null;
        bloodscreen.css({
            "animation": ""
        });
        //bloodscreen.removeClass("bloodscreen_show").removeClass("bloodscreen_hide_transition");
    }, 2500);
});

function toggleOrders(bool) {
    if (bool)
        ordersDiv.show(1000);
    else
        ordersDiv.hide(1000);
}


$(document).ready(() => {
    bloodscreenDom = bloodscreen.get()[0];
});
