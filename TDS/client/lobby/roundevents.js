"use strict";
mp.events.add("onClientMapChange", function (maplimit, mapmidx, mapmidy, mapmidz) {
    log("onClientMapChange start");
    mp.game.cam.doScreenFadeIn(lobbysettings.roundendtime / 2);
    maplimit = JSON.parse(maplimit);
    if (maplimit.length > 0)
        loadMapLimitData(maplimit);
    mp.gui.chat.push(mapmidx + " - " + mapmidy + " - " + mapmidz);
    loadMapMiddleForCamera(new mp.Vector3(mapmidx, mapmidy, mapmidz));
    log("onClientMapChange end");
});
mp.events.add("onClientCountdownStart", function (mapname, resttime) {
    log("onClientCountdownStart start ");
    if (cameradata.timer != null)
        cameradata.timer.kill();
    if (resttime == null) {
        startCountdown();
        cameradata.timer = new Timer(setCameraGoTowardsPlayer, lobbysettings.countdowntime * 0.1, 1);
    }
    else {
        startCountdownAfterwards(Math.ceil((lobbysettings.countdowntime - resttime) / 1000));
        if (resttime > lobbysettings.countdowntime * 0.1) {
            setCameraGoTowardsPlayer(lobbysettings.countdowntime - resttime);
        }
        else {
            cameradata.timer = new Timer(setCameraGoTowardsPlayer, lobbysettings.countdowntime * 0.1 - resttime, 1);
        }
    }
    if (rounddata.isspectator)
        startSpectate();
    rounddata.mapinfo.setText(mapname);
    log("onClientCountdownStart end");
});
mp.events.add("onClientRoundStart", function (isspectator, _, wastedticks) {
    log("onClientRoundStart start");
    mp.game.cam.doScreenFadeIn(50);
    stopCountdownCamera();
    endCountdown();
    rounddata.isspectator = isspectator;
    if (!rounddata.isspectator) {
        startMapLimit();
        toggleFightMode(true);
    }
    roundStartedRoundInfo(wastedticks);
    log("onClientRoundStart end");
});
mp.events.add("onClientRoundEnd", function () {
    log("onClientRoundEnd start");
    mp.game.cam.doScreenFadeOut(lobbysettings.roundendtime / 2);
    toggleFightMode(false);
    removeBombThings();
    emptyMapLimit();
    removeRoundThings(false);
    stopCountdown();
    stopCountdownCamera();
    removeRoundInfo();
    log("onClientRoundEnd end");
});
mp.events.add("onClientPlayerSpectateMode", function () {
    log("onClientPlayerSpectateMode start");
    rounddata.isspectator = true;
    startSpectate();
    log("onClientPlayerSpectateMode end");
});
mp.events.add("onClientPlayerDeath", function (player, teamID, killstr) {
    log("onClientPlayerDeath start");
    if (mp.players.local == player) {
        toggleFightMode(false);
        removeBombThings();
    }
    playerDeathRoundInfo(teamID, killstr);
    log("onClientPlayerDeath end");
});
mp.events.add("PlayerQuit", function (player, exitType, reason) {
    log("onClientPlayerQuit start");
    log("onClientPlayerQuit end");
});
mp.events.add("onClientPlayerGotBomb", function (placestoplant) {
    localPlayerGotBomb(placestoplant);
});
mp.events.add("onClientPlayerPlantedBomb", function () {
    localPlayerPlantedBomb();
});
mp.events.add("onClientBombPlanted", function (pos, candefuse) {
    bombPlanted(pos, candefuse);
});
mp.events.add("onClientBombDetonated", function () {
    bombDetonated();
});
