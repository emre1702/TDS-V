"use strict";
let roundinfo = {
    amountinteams: [],
    aliveinteams: [],
    roundtime: 0,
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
            showtick: 15000,
            fadeaftertick: 11000,
            xpos: res.Width * 0.99,
            ypos: res.Height * 0.45,
            scale: 0.3,
            height: res.Height * 0.04
        }
    }
};
function drawRoundInfo() {
    let tick = API.getGlobalTime();
    let fullseconds = Math.ceil((roundinfo.roundtime - (tick - roundinfo.starttick)) / 1000);
    let minutes = Math.floor(fullseconds / 60);
    let seconds = fullseconds % 60;
    let tdata = roundinfo.drawdata.time.text;
    let trdata = roundinfo.drawdata.time.rectangle;
    API.drawText(minutes + ":" + (seconds >= 10 ? seconds : "0" + seconds), trdata.xpos + trdata.width / 2, tdata.ypos, tdata.scale, tdata.r, tdata.g, tdata.b, tdata.a, 0, 1, true, true, 0);
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
function setRoundTimeLeft(lefttime) {
    roundinfo.starttick = API.getGlobalTime() - lefttime;
}
API.onUpdate.connect(function () {
    let length = roundinfo.killinfo.length;
    if (length > 0) {
        let tick = API.getGlobalTime();
        for (let i = length - 1; i >= 0; i--) {
            let tickwasted = tick - roundinfo.killinfo[i].starttick;
            let data = roundinfo.drawdata.kills;
            if (tickwasted < data.showtick) {
                let alpha = tickwasted <= data.fadeaftertick ? 255 : Math.ceil((data.showtick - tickwasted) / (data.showtick - data.fadeaftertick) * 255);
                let counter = length - i - 1;
                API.drawText(roundinfo.killinfo[i].killstr, data.xpos, data.ypos + counter * data.height, data.scale, 255, 255, 255, alpha, 0, 2, true, true, 0);
            }
            else {
                roundinfo.killinfo.splice(0, i + 1);
                break;
            }
        }
    }
});
function removeRoundInfo() {
    roundinfo.amountinteams = [];
    roundinfo.aliveinteams = [];
    if (roundinfo.drawevent != null) {
        roundinfo.drawevent.disconnect();
        roundinfo.drawevent = null;
    }
}
function roundStartedRoundInfo(args) {
    roundinfo.starttick = API.getGlobalTime();
    if (2 in args)
        roundinfo.starttick -= args[2];
    roundinfo.drawevent = API.onUpdate.connect(drawRoundInfo);
}
function addTeamInfos(teamnames, teamcolors) {
    for (let i = 1; i < teamnames.Count; i++) {
        roundinfo.teamnames[i - 1] = teamnames[i];
    }
    for (let i = 3; i < teamnames.Count; i++) {
        roundinfo.teamcolors[i - 3] = teamnames[i];
    }
}
function playerDeathRoundInfo(teamID, killstr) {
    roundinfo.aliveinteams[teamID]--;
    roundinfo.killinfo.push({ "killstr": killstr, "starttick": API.getGlobalTime() });
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
    }
});
