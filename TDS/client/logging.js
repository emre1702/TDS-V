"use strict";
let activatedlogging = false;
mp.events.addCommand("activatedalogging", (player, text) => {
    activatedlogging = !activatedlogging;
});
function log(message) {
    if (activatedlogging) {
        mp.gui.chat.push(message);
    }
}
