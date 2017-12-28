"use strict";
mp.events.add("onClientPlayerJoinLobby", (isspectator, mapname, teamnames, teamcolors, countdowntime, roundtime, bombdetonatetime, bombplanttime, bombdefusetime, roundendtime) => {
    log("onClientPlayerJoinLobby start");
    rounddata.isspectator = isspectator;
    setMapInfo(mapname);
    teamnames = JSON.parse(teamnames);
    teamcolors = JSON.parse(teamcolors);
    addTeamInfos(teamnames, teamcolors);
    lobbysettings.countdowntime = countdowntime;
    roundinfo.roundtime = roundtime;
    lobbysettings.bombdetonatetime = bombdetonatetime;
    lobbysettings.bombplanttime = bombplanttime;
    lobbysettings.bombdefusetime = bombdefusetime;
    lobbysettings.roundendtime = roundendtime;
    log("onClientPlayerJoinLobby end");
});
mp.events.add("onClientPlayerLeaveLobby", (player) => {
    log("onClientPlayerLeaveLobby start");
    if (mp.players.local == player) {
        toggleFightMode(false);
        removeBombThings();
        removeRoundThings(true);
        stopCountdownCamera();
        removeRoundInfo();
    }
    log("onClientPlayerLeaveLobby end");
});
mp.events.add("onClientJoinMainMenu", () => {
    mp.game.cam.doScreenFadeIn(100);
});
