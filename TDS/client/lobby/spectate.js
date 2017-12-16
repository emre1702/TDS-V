"use strict";
let spectatedata = {
    binded: false
};
function pressSpectateKeyLeft(sender, e) {
    mp.events.callRemote("spectateNext", false);
}
function pressSpectateKeyRight(sender, e) {
    mp.events.callRemote("spectateNext", true);
}
function startSpectate() {
    if (!spectatedata.binded) {
        mp.keys.bind(Keys.LeftArrow, true, pressSpectateKeyLeft);
        mp.keys.bind(Keys.A, true, pressSpectateKeyLeft);
        mp.keys.bind(Keys.RightArrow, true, pressSpectateKeyRight);
        mp.keys.bind(Keys.D, true, pressSpectateKeyRight);
        spectatedata.binded = true;
    }
}
function stopSpectate() {
    if (spectatedata.binded) {
        mp.keys.unbind(Keys.LeftArrow, true, pressSpectateKeyLeft);
        mp.keys.unbind(Keys.A, true, pressSpectateKeyLeft);
        mp.keys.unbind(Keys.RightArrow, true, pressSpectateKeyRight);
        mp.keys.unbind(Keys.D, true, pressSpectateKeyRight);
        spectatedata.binded = false;
    }
}
