"use strict";
let bloodscreenbrowser;
mp.events.add("playerWeaponShoot", (shotPosition, target) => {
    if (target != null) {
        let weapon = mp.players.local.weapon;
        var hithead = false;
        mp.events.callRemote("onPlayerHitOtherPlayer", target, weapon, hithead);
    }
});
mp.players.local.setCanAttackFriendly(false, false);
