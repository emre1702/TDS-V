"use strict";
let blipsdata = {
    teamblips: {},
    updateteamblipsevent: null
};
function updateTeamBlipPositions() {
    for (var playername in blipsdata.teamblips) {
        var player = API.getPlayerByName(playername);
        if (player != null) {
            let pos = API.getEntityPosition(player);
            API.setBlipPosition(blipsdata.teamblips[playername], pos);
        }
    }
}
function createTeamBlips(playerlist) {
    log("createTeamBlips start");
    blipsdata.teamblips = {};
    let localplayer = API.getLocalPlayer();
    let dimension = API.getEntityDimension(localplayer);
    for (let i = 0; i < playerlist.Count; i++) {
        if (!localplayer.Equals(playerlist[i])) {
            let blip = API.createBlip(API.getEntityPosition(playerlist[i]));
            API.setEntityDimension(blip, dimension);
            API.setBlipSprite(blip, 0);
            blipsdata.teamblips[API.getPlayerName(playerlist[i])] = blip;
        }
    }
    log("createTeamBlips updateteamblipsevent");
    blipsdata.updateteamblipsevent = API.onUpdate.connect(updateTeamBlipPositions);
    log("createTeamBlips end");
}
function removeTeammateFromTeamBlips(playername) {
    log("removeTeammateFromTeamBlips start");
    if (blipsdata.teamblips[playername] != undefined) {
        API.deleteEntity(blipsdata.teamblips[playername]);
        delete blipsdata.teamblips[playername];
    }
    log("removeTeammateFromTeamBlips end");
}
function stopTeamBlips() {
    log("stopTeamBlips start");
    if (blipsdata.updateteamblipsevent != null) {
        blipsdata.updateteamblipsevent.disconnect();
        blipsdata.updateteamblipsevent = null;
    }
    for (var playername in blipsdata.teamblips) {
        API.deleteEntity(blipsdata.teamblips[playername]);
    }
    blipsdata.teamblips = {};
    log("stopTeamBlips end");
}
