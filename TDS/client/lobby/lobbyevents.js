"use strict";
mp.events.add("onClientPlayerJoinLobby", (args) => {
    log("onClientPlayerJoinLobby start");
    rounddata.isspectator = args[0];
    setMapInfo(args[1]);
    addTeamInfos(args[2], args[3]);
    lobbysettings.countdowntime = args[4];
    roundinfo.roundtime = args[5];
    lobbysettings.bombdetonatetime = args[6];
    lobbysettings.bombplanttime = args[7];
    lobbysettings.bombdefusetime = args[8];
    lobbysettings.roundendtime = args[9];
    log("onClientPlayerJoinLobby end");
});
mp.events.add("onClientPlayerLeaveLobby", (args) => {
    log("onClientPlayerLeaveLobby start");
    if (mp.players.local == args[0]) {
        toggleFightMode(false);
        removeBombThings();
        removeRoundThings(true);
        stopCountdownCamera();
        removeRoundInfo();
    }
    log("onClientPlayerLeaveLobby end");
});
mp.events.add("onClientJoinMainMenu", (args) => {
    mp.game.cam.doScreenFadeIn(100);
});
