/// <reference path="../types-ragemp/index.d.ts" />
// Körperteile: https://pastebin.com/AGQWgCct

let damagesysdata = {
    lastarmorhp: 200,
    shotsdoneinround: 0,
    shooting: false
}

var getCurrentWeapon = () => mp.game.invoke('0x6678C142FAC881BA', localPlayer.handle);


mp.events.add("render", () => {
    if (!rounddata.infight)
        return;
    if (settingsdata.bloodscreen) {
        let armorhp = mp.players.local.getHealth() + mp.players.local.getArmour();
        if (armorhp != damagesysdata.lastarmorhp) {
            damagesysdata.lastarmorhp = armorhp;
            if (armorhp < damagesysdata.lastarmorhp)
                showBloodscreen();
        }
    }
    //checkShooting();
});

/*function checkShooting() {
    if ( localPlayer.isShooting() ) {
        damagesysdata.shooting = true;
    } else if ( damagesysdata.shooting ) {
        damagesysdata.shooting = false;
        let ammo = getWeaponAmmo( currentWeapon );
        damagesysdata.shotsdoneinround += ( currentAmmo - ammo );
        currentAmmo = ammo;
    }
}*/

mp.events.add("playerWeaponShot", (hitpos, hitentity) => {
    mp.gui.chat.push("" + hitentity + " - " + typeof hitentity);
    //++damagesysdata.shotsdoneinround;
    let startpos = localPlayer.getBoneCoords(6286, 0, 0, 0);
    let endpos = vector3Lerp(startpos, hitpos, 1.02) as Vector3Mp;
    let raycast = mp.raycasting.testPointToPoint(startpos, endpos, localPlayer.handle, 8) as RaycastResult;
    if (typeof raycast !== "undefined") { // hit nothing {
        let player = mp.players.atHandle(raycast.entity);
        if (player !== null) {
            mp.gui.chat.push(player.name);
            callRemote(ECustomEvents.PlayerHitOtherPlayer, player, false);
            return true;
        }
    }
});

mp.players.local.setCanAttackFriendly(false, false);