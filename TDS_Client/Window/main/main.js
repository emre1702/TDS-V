let bloodscreen = $("#bloodscreen");
let bloodscreenDom;
let killmessagesBox = $("#kill_messages_box");
let voiceChatPlayerNamesBox = $("#voice_chat_player_names_box");
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

function playSound(soundname) {
    $("#audio_" + soundname).trigger("play").prop("volume", volume);
}

function playHitsound() {
    hitsounds[hitsoundcounter++].trigger("play").prop("volume", volume);
    if (hitsoundcounter == hitsoundsamount)
        hitsoundcounter = 0;
}

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

function showBloodscreen() {
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
}

function formatMsgKill(input) {
    var start = '<span style="color: white;">';

    let replaced = input;
    if (input.indexOf("~") !== -1) {
        replaced = replaced.replace(/~r~/g, '</span><span style="color: rgb(222, 50, 50);">')
            .replace(/~b~/g, '</span><span style="color: rgb(92, 180, 227);">')
            .replace(/~g~/g, '</span><span style="color: rgb(113, 202, 113);">')
            .replace(/~y~/g, '</span><span style="color: rgb(238, 198, 80);">')
            //  .replace( /~p~/g, '</span><span style="color: rgb(131, 101, 224);">' )
            //  .replace( /~q~/g, '</span><span style="color: rgb(226, 79, 128);">' )
            .replace(/~o~/g, '</span><span style="color: rgb(253, 132, 85);">')
            //  .replace( /~c~/g, '</span><span style="color: rgb(139, 139, 139);">' )
            //  .replace( /~m~/g, '</span><span style="color: rgb(99, 99, 99);">' )
            //  .replace( /~u~/g, '</span><span style="color: rgb(0, 0, 0);">' )
            .replace(/~s~/g, '</span><span style="color: rgb(220, 220, 220);">')
            .replace(/~w~/g, '</span><span style="color: white;">')		// white
            .replace(/~dr~/g, '</span><span style="color: rgb( 169, 25, 25 );">')		// dark red
            .replace(/~n~/g, '<br>');
    }

    return start + replaced + "</span>";
}

function removeThis(element) {
    element.remove();
}

function addKillMessage(msg) {
    let child = $("<text>" + formatMsgKill(msg) + "<br></text>");
    killmessagesBox.append(child);
    child.delay(11000).fadeOut(4000, child.remove);
}

function toggleOrders(bool) {
    if (bool)
        ordersDiv.show(1000);
    else
        ordersDiv.hide(1000);
}

function addPlayerTalking(name) {
	let id = "voice-chat-" + name;
	let img = "<img src='../pic/speaker.png'/>";
	let child = $("<div id='" + id + "'>" + img + "<span>" + name + "</span></div>");
    voiceChatPlayerNamesBox.append(child);
}

function removePlayerTalking(name) {
	let id = "voice-chat-" + name;
	let child = voiceChatPlayerNamesBox.find("#" + id);
	if (child) {
		child.remove();
	}
}

$(document).ready(() => {
    bloodscreenDom = bloodscreen.get()[0];
});