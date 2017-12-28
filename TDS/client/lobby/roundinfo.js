"use strict";
let roundinfo = {
    amountinteams: [],
    aliveinteams: [],
    roundtime: 0,
    starttick: 0,
    teamnames: [],
    teamcolors: [],
    drawevent: false,
    killinfo: [],
    drawdata: {
        time: {
            text: {
                ypos: res.y * 0.005,
                scale: 0.5,
                r: 255,
                g: 255,
                b: 255,
                a: 255,
            },
            rectangle: {
                xpos: res.x * 0.47,
                ypos: 0,
                width: res.x * 0.06,
                height: res.y * 0.05,
                r: 20,
                g: 20,
                b: 20,
                a: 180
            }
        },
        team: {
            text: {
                ypos: -res.y * 0.002,
                scale: 0.41,
                r: 255,
                g: 255,
                b: 255,
                a: 255
            },
            rectangle: {
                ypos: 0,
                width: res.x * 0.13,
                height: res.y * 0.06,
                a: 180
            }
        },
        kills: {
            showtick: 15000,
            fadeaftertick: 11000,
            xpos: res.x * 0.99,
            ypos: res.y * 0.45,
            scale: 0.3,
            height: res.y * 0.04
        }
    }
};
function drawRoundInfo() {
    if (roundinfo.drawevent) {
        let tick = getTick();
        let fullseconds = Math.ceil((roundinfo.roundtime - (tick - roundinfo.starttick)) / 1000);
        let minutes = Math.floor(fullseconds / 60);
        let seconds = fullseconds % 60;
        let tdata = roundinfo.drawdata.time.text;
        let trdata = roundinfo.drawdata.time.rectangle;
        mp.game.graphics.drawText(minutes + ":" + (seconds >= 10 ? seconds : "0" + seconds), 1, { r: tdata.r, g: tdata.g, b: tdata.b, a: tdata.a }, tdata.scale, tdata.scale, trdata.xpos + trdata.width / 2, tdata.ypos, true);
        mp.game.graphics.drawRect(trdata.xpos, trdata.ypos, trdata.width, trdata.height, trdata.r, trdata.g, trdata.b, trdata.a);
        let teamdata = roundinfo.drawdata.team.text;
        let teamrdata = roundinfo.drawdata.team.rectangle;
        let leftteamamount = Math.ceil(roundinfo.teamnames.length / 2);
        for (let i = 0; i < leftteamamount; i++) {
            let startx = trdata.xpos - teamrdata.width * (i + 1);
            mp.game.graphics.drawText(roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i], 1, { r: teamdata.r, g: teamdata.g, b: teamdata.b, a: teamdata.a }, teamdata.scale, teamdata.scale, startx + teamrdata.width / 2, teamdata.ypos, true);
            mp.game.graphics.drawRect(startx, teamrdata.ypos, teamrdata.width, teamrdata.height, roundinfo.teamcolors[0 + i * 3], roundinfo.teamcolors[1 + i * 3], roundinfo.teamcolors[2 + i * 3], teamrdata.a);
        }
        for (let j = 0; j < roundinfo.teamnames.length - leftteamamount; j++) {
            let startx = trdata.xpos + trdata.width + teamrdata.width * j;
            let i = leftteamamount + j;
            mp.game.graphics.drawText(roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i], { r: teamdata.r, g: teamdata.g, b: teamdata.b, a: teamdata.a }, teamdata.scale, teamdata.scale, startx + teamrdata.width / 2, 1, teamdata.ypos, true);
            mp.game.graphics.drawRect(startx, teamrdata.ypos, teamrdata.width, teamrdata.height, roundinfo.teamcolors[0 + i * 3], roundinfo.teamcolors[1 + i * 3], roundinfo.teamcolors[2 + i * 3], teamrdata.a);
        }
    }
}
mp.events.add("render", drawRoundInfo);
function setRoundTimeLeft(lefttime) {
    roundinfo.starttick = getTick() - (roundinfo.roundtime - lefttime);
}
mp.events.add("render", function () {
    let length = roundinfo.killinfo.length;
    if (length > 0) {
        let tick = getTick();
        for (let i = length - 1; i >= 0; i--) {
            let tickwasted = tick - roundinfo.killinfo[i].starttick;
            let data = roundinfo.drawdata.kills;
            if (tickwasted < data.showtick) {
                let alpha = tickwasted <= data.fadeaftertick ? 255 : Math.ceil((data.showtick - tickwasted) / (data.showtick - data.fadeaftertick) * 255);
                let counter = length - i - 1;
                mp.game.graphics.drawText(roundinfo.killinfo[i].killstr, 2, { r: 255, g: 255, b: 255, a: alpha }, data.scale, data.scale, data.xpos, data.ypos + counter * data.height, true);
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
    roundinfo.drawevent = false;
}
function roundStartedRoundInfo(wastedticks) {
    roundinfo.starttick = getTick();
    if (wastedticks != null)
        roundinfo.starttick -= wastedticks;
    roundinfo.drawevent = true;
}
function addTeamInfos(teamnames, teamcolors) {
    for (let i = 1; i < teamnames.length; i++) {
        roundinfo.teamnames[i - 1] = teamnames[i];
    }
    for (let i = 3; i < teamcolors.length; i++) {
        roundinfo.teamcolors[i - 3] = teamcolors[i];
    }
}
function playerDeathRoundInfo(teamID, killstr) {
    roundinfo.aliveinteams[teamID]--;
    roundinfo.killinfo.push({ "killstr": killstr, "starttick": getTick() });
}
mp.events.add("onClientPlayerAmountInFightSync", (amountinteam, isroundstarted, amountaliveinteam) => {
    log("onClientPlayerAmountInFightSync start");
    roundinfo.amountinteams = [];
    roundinfo.aliveinteams = [];
    amountinteam = JSON.parse(amountinteam);
    if (isroundstarted)
        amountaliveinteam = JSON.parse(amountaliveinteam);
    for (let i = 0; i < amountinteam.length; i++) {
        roundinfo.amountinteams[i] = Number.parseInt(amountinteam[i]);
        if (!isroundstarted)
            roundinfo.aliveinteams[i] = Number.parseInt(amountinteam[i]);
        else
            roundinfo.aliveinteams[i] = Number.parseInt(amountaliveinteam[i]);
    }
    log("onClientPlayerAmountInFightSync end");
});
