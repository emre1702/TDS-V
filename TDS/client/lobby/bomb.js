"use strict";
let bombdata = {
    changed: false,
    gotbomb: false,
    placestoplant: [],
    plantdefuseevent: false,
    isplanting: false,
    isdefusing: false,
    plantdefusestarttick: 0,
    plantedpos: null
};
function drawPlant() {
    let tickswasted = getTick() - bombdata.plantdefusestarttick;
    if (tickswasted < lobbysettings.bombplanttime) {
        mp.game.graphics.drawRect(res.x * 0.46, res.y * 0.7, res.x * 0.08, res.y * 0.02, 0, 0, 0, 187);
        let progress = tickswasted / lobbysettings.bombplanttime;
        mp.game.graphics.drawRect(res.x * 0.461, res.y * 0.701, res.x * 0.078 * progress, res.y * 0.018, 0, 180, 0, 187);
        mp.game.graphics.drawText(getLang("round", "planting"), 1, { r: 255, g: 255, b: 255, a: 255 }, 0.4, 0.4, true, res.x * 0.5, res.y * 0.71);
    }
}
function checkPlant() {
    let isonplacetoplant = false;
    let playerpos = mp.players.local.position;
    for (let i = 0; i < bombdata.placestoplant.length && !isonplacetoplant; i++) {
        let pos = bombdata.placestoplant[i];
        if (mp.game.gameplay.getDistanceBetweenCoords(playerpos.x, playerpos.y, playerpos.z, pos.x, pos.y, pos.z, true) <= 5)
            isonplacetoplant = true;
    }
    if (isonplacetoplant) {
        if (bombdata.isplanting) {
            drawPlant();
        }
        else {
            bombdata.plantdefusestarttick = getTick();
            bombdata.isplanting = true;
            mp.events.callRemote("onPlayerStartPlanting");
        }
    }
    else
        checkPlantDefuseStop();
}
function drawDefuse() {
    let tickswasted = getTick() - bombdata.plantdefusestarttick;
    if (tickswasted < lobbysettings.bombdefusetime) {
        mp.game.graphics.drawRect(res.x * 0.46, res.y * 0.7, res.x * 0.08, res.y * 0.02, 0, 0, 0, 187);
        let progress = tickswasted / lobbysettings.bombdefusetime;
        mp.game.graphics.drawRect(res.x * 0.461, res.y * 0.701, res.x * 0.078 * progress, res.y * 0.018, 180, 0, 0, 187);
        mp.game.graphics.drawText(getLang("round", "defusing"), 1, { r: 255, g: 255, b: 255, a: 255 }, 0.4, 0.4, true, res.x * 0.5, res.y * 0.71);
    }
}
function checkDefuse() {
    let playerpos = mp.players.local.position;
    if (mp.game.gameplay.getDistanceBetweenCoords(playerpos.x, playerpos.y, playerpos.z, bombdata.plantedpos.x, bombdata.plantedpos.y, bombdata.plantedpos.z, true) <= 5) {
        if (bombdata.isdefusing) {
            drawDefuse();
        }
        else {
            bombdata.plantdefusestarttick = getTick();
            bombdata.isdefusing = true;
            mp.events.callRemote("onPlayerStartDefusing");
        }
    }
    else
        checkPlantDefuseStop();
}
function checkPlantDefuseStop() {
    if (bombdata.isplanting) {
        bombdata.isplanting = false;
        mp.events.callRemote("onPlayerStopPlanting");
    }
    else if (bombdata.isdefusing) {
        bombdata.isdefusing = false;
        mp.events.callRemote("onPlayerStopDefusing");
    }
}
function checkPlantDefuse() {
    if (mp.players.local.weapon == WeaponHash.Unarmed) {
        if (!mp.players.local.isDeadOrDying(true)) {
            if (bombdata.gotbomb) {
                checkPlant();
                return;
            }
            else {
                checkDefuse();
                return;
            }
        }
        else
            checkPlantDefuseStop();
    }
    else
        checkPlantDefuseStop();
}
function localPlayerGotBomb(placestoplant) {
    bombdata.changed = true;
    bombdata.gotbomb = true;
    let i = placestoplant.Count;
    while (i--)
        bombdata.placestoplant[i] = placestoplant[i];
    bombdata.plantdefuseevent = true;
}
function localPlayerPlantedBomb() {
    bombdata.gotbomb = false;
    bombdata.plantdefuseevent = false;
    bombdata.isplanting = false;
}
function bombPlanted(pos, candefuse) {
    if (candefuse) {
        bombdata.changed = true;
        bombdata.plantedpos = pos;
        bombdata.plantdefuseevent = true;
    }
    setRoundTimeLeft(lobbysettings.bombdetonatetime);
}
function bombDetonated() {
    mp.game.cam.shakeGameplayCam("LARGE_EXPLOSION_SHAKE", 1.0);
    new Timer(mp.game.cam.stopGameplayCamShaking, 4000, 1);
}
function removeBombThings() {
    if (bombdata.changed) {
        bombdata = {
            changed: false,
            gotbomb: false,
            placestoplant: [],
            plantdefuseevent: false,
            isplanting: false,
            isdefusing: false,
            plantdefusestarttick: 0,
            plantedpos: null
        };
    }
}
