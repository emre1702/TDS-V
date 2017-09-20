"use strict";
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "onClientPlayerJoinLobby":
            log("onClientPlayerJoinLobby start");
            rounddata.isspectator = args[0];
            setMapInfo(args[1]);
            addTeamInfos(args[2][0], args[2][1]);
            lobbysettings.countdowntime = args[2][2];
            roundinfo.roundtime = args[2][3];
            lobbysettings.bombdetonatetime = parseInt(args[2][4]);
            lobbysettings.bombplanttime = parseInt(args[2][5]);
            lobbysettings.bombdefusetime = parseInt(args[2][6]);
            log("onClientPlayerJoinLobby end");
            break;
        case "onClientPlayerLeaveLobby":
            log("onClientPlayerLeaveLobby start");
            if (API.getLocalPlayer() == args[0]) {
                toggleFightMode(false);
                removeBombThings();
                removeRoundThings(true);
                stopCountdownCamera();
                localPlayerLeftLobbyMapVoting();
                removeRoundInfo();
            }
            else {
                removeTeammateFromTeamBlips(API.getPlayerName(args[0]));
            }
            log("onClientPlayerLeaveLobby end");
            break;
    }
});
