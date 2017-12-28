"use strict";
let activatedlogging = true;
function log(message) {
    if (activatedlogging) {
        mp.gui.chat.push(message);
    }
}
