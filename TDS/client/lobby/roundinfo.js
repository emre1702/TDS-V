"use strict";
let roundinfo = {
    amountinteams: [],
    aliveinteams: [],
    roundtime: 0,
    roundtimeleft: null,
    starttick: 0,
    teamnames: [],
    teamcolors: [],
    drawevent: null,
    killinfo: [],
    drawdata: {
        time: {
            text: {
                ypos: res.Height * 0.005,
                scale: 0.5,
                r: 255,
                g: 255,
                b: 255,
                a: 255,
            },
            rectangle: {
                xpos: res.Width * 0.47,
                ypos: 0,
                width: res.Width * 0.06,
                height: res.Height * 0.05,
                r: 20,
                g: 20,
                b: 20,
                a: 180
            }
        },
        team: {
            text: {
                ypos: -res.Height * 0.002,
                scale: 0.41,
                r: 255,
                g: 255,
                b: 255,
                a: 255
            },
            rectangle: {
                ypos: 0,
                width: res.Width * 0.13,
                height: res.Height * 0.06,
                a: 180
            }
        },
        kills: {
            showtick: 5000
        }
    }
};
function drawRoundInfo() {
    let tick = API.getGlobalTime();
    let fullseconds = Math.ceil((roundinfo.roundtime * 1000 - (tick - roundinfo.starttick)) / 1000);
    if (roundinfo.roundtimeleft != null)
        fullseconds = Math.ceil((roundinfo.roundtimeleft * 1000 - (tick - roundinfo.starttick)) / 1000);
    let minutes = Math.floor(fullseconds / 60);
    let seconds = fullseconds % 60;
    let tdata = roundinfo.drawdata.time.text;
    let trdata = roundinfo.drawdata.time.rectangle;
    API.drawText(minutes + ":" + (seconds >= 10 ? seconds : "0" + seconds), trdata.xpos + trdata.width / 2, tdata.ypos, tdata.scale, tdata.r, tdata.g, tdata.b, tdata.a, 0, 1, true, false, 0);
    API.drawRectangle(trdata.xpos, trdata.ypos, trdata.width, trdata.height, trdata.r, trdata.g, trdata.b, trdata.a);
    let teamdata = roundinfo.drawdata.team.text;
    let teamrdata = roundinfo.drawdata.team.rectangle;
    let leftteamamount = Math.ceil(roundinfo.teamnames.length / 2);
    for (let i = 0; i < leftteamamount; i++) {
        let startx = trdata.xpos - teamrdata.width * (i + 1);
        API.drawText(roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i], startx + teamrdata.width / 2, teamdata.ypos, teamdata.scale, teamdata.r, teamdata.g, teamdata.b, teamdata.a, 0, 1, true, true, 0);
        API.drawRectangle(startx, teamrdata.ypos, teamrdata.width, teamrdata.height, roundinfo.teamcolors[0 + i * 3], roundinfo.teamcolors[1 + i * 3], roundinfo.teamcolors[2 + i * 3], teamrdata.a);
    }
    for (let j = 0; j < roundinfo.teamnames.length - leftteamamount; j++) {
        let startx = trdata.xpos + trdata.width + teamrdata.width * j;
        let i = leftteamamount + j;
        API.drawText(roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i], startx + teamrdata.width / 2, teamdata.ypos, teamdata.scale, teamdata.r, teamdata.g, teamdata.b, teamdata.a, 0, 1, true, true, 0);
        API.drawRectangle(startx, teamrdata.ypos, teamrdata.width, teamrdata.height, roundinfo.teamcolors[0 + i * 3], roundinfo.teamcolors[1 + i * 3], roundinfo.teamcolors[2 + i * 3], teamrdata.a);
    }
}
API.onUpdate.connect(function () {
    if (roundinfo.killinfo[0] != undefined) {
        let tick = API.getGlobalTime();
        for (let i = roundinfo.killinfo.length - 1; i >= 0; i++) {
            if (tick - roundinfo.starttick >= roundinfo.drawdata.kills.showtick) {
            }
            else {
                roundinfo.killinfo.splice(0, i + 1);
                break;
            }
        }
    }
});
function removeRoundInfo() {
    roundinfo.roundtimeleft = null;
    roundinfo.amountinteams = [];
    roundinfo.aliveinteams = [];
    if (roundinfo.drawevent != null) {
        roundinfo.drawevent.disconnect();
        roundinfo.drawevent = null;
    }
}
API.onServerEventTrigger.connect(function (eventName, args) {
    switch (eventName) {
        case "onClientPlayerAmountInFightSync":
            log("onClientPlayerAmountInFightSync start");
            roundinfo.amountinteams = [];
            roundinfo.aliveinteams = [];
            for (let i = 0; i < args[0].Count; i++) {
                roundinfo.amountinteams[i] = args[0][i];
                if (args[1] == false)
                    roundinfo.aliveinteams[i] = args[0][i];
                else
                    roundinfo.aliveinteams[i] = args[2][i];
            }
            log("onClientPlayerAmountInFightSync end");
            break;
        case "onClientPlayerDeath":
            log("onClientPlayerDeath start");
            roundinfo.aliveinteams[args[1]]--;
            roundinfo.killinfo.push({ "player": API.getPlayerName(args[1]), "killer": API.getPlayerName(args[2]), "weapon": args[3], "starttick": API.getGlobalTime() });
            log("onClientPlayerDeath end");
            break;
        case "onClientRoundStart":
            log("onClientRoundStart start");
            roundinfo.starttick = API.getGlobalTime();
            roundinfo.drawevent = API.onUpdate.connect(drawRoundInfo);
            log("onClientRoundStart end");
            break;
        case "onClientPlayerLeaveLobby":
            log("onClientPlayerLeaveLobby start");
            removeRoundInfo();
            log("onClientPlayerLeaveLobby end");
            break;
        case "onClientRoundEnd":
            log("onClientRoundEnd start");
            removeRoundInfo();
            log("onClientRoundEnd end");
            break;
        case "onClientPlayerJoinLobby":
            log("onClientPlayerJoinLobby start");
            roundinfo.roundtime = args[2];
            for (let i = 1; i < args[4].Count; i++) {
                roundinfo.teamnames[i - 1] = args[4][i];
            }
            for (let i = 3; i < args[5].Count; i++) {
                roundinfo.teamcolors[i - 3] = args[5][i];
            }
            log("onClientPlayerJoinLobby end");
            break;
    }
});
