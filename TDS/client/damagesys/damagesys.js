"use strict";
let damagesysdata = {
    lasthparmor: 0,
};
mp.events.add("render", () => {
    if (mp.players.local.health + mp.players.local.armour < damagesysdata.lasthparmor) {
    }
});
