"use strict";
function createScoreboard() {
    var playertable = [];
    var playertablelength = 0;
    var scroll = 0;
    var lastplayerlisttrigger = 0;
    var playerlistevent = null;
    var playerlistopenkey = null;
    var playerlistdata = {
        maxplayers: 25,
        completewidth: res.Width * 0.4,
        scrollbarwidth: res.Width * 0.02,
        columnheight: res.Height * 0.025,
        columnwidthpercent: {
            name: 0.3,
            playtime: 0.15,
            kills: 0.1,
            assists: 0.1,
            deaths: 0.1,
            teamorlobby: 0.25
        },
        titleheight: res.Height * 0.03,
        bottomheight: 0,
        titlerectanglecolor: [20, 20, 20, 187],
        bottomrectanglecolor: [20, 20, 20, 187],
        rectanglecolor: [10, 10, 10, 187],
        titlefontcolor: [255, 255, 255, 255],
        bottomfontcolor: [255, 255, 255, 255],
        fontcolor: [255, 255, 255, 255],
        scrollbarbackcolor: [30, 30, 30, 187],
        scrollbarcolor: [120, 120, 120, 187],
        titlefontscale: 0.35,
        fontscale: 0.28,
        bottomfontscale: 0.35,
        titlefont: 0,
        font: 0,
        bottomfont: 0
    };
    var playerlisttitles = ["name", "playtime", "kills", "assists", "deaths", "team", "lobby"];
    var playerlisttitlesindex = { name: "name", playtime: "playtime", kills: "kills", assists: "assists", deaths: "deaths", team: "teamorlobby", lobby: "teamorlobby" };
    var otherlobbytable = [];
    var teamnames = [];
    var inmainmenu = false;
    function drawPlayerList() {
        API.disableControlThisFrame(16);
        API.disableControlThisFrame(17);
        var language = getLang("scoreboard");
        var v = playerlistdata;
        var len = playertablelength;
        if (len > v.maxplayers)
            len = v.maxplayers;
        var startX = res.Width * 0.5 - v.completewidth / 2;
        var startY = res.Height * 0.5 - len * v.columnheight / 2 + v.titleheight / 2 - v.bottomheight / 2;
        var titleStartY = startY - v.titleheight;
        var bottomStartY = startY + len * v.columnheight;
        API.drawRectangle(startX, titleStartY, v.completewidth, v.titleheight, v.titlerectanglecolor[0], v.titlerectanglecolor[1], v.titlerectanglecolor[2], v.titlerectanglecolor[3]);
        var lastwidthstitle = 0;
        var titleslength = playerlisttitles.length;
        for (let i = 0; i < titleslength - 1; i++) {
            if (playerlisttitles[i] == "team" && inmainmenu)
                API.drawText(language[playerlisttitles[i + 1]], startX + lastwidthstitle + v.columnwidthpercent[playerlisttitlesindex[playerlisttitles[i]]] * v.completewidth / 2, titleStartY, v.titlefontscale, v.titlefontcolor[0], v.titlefontcolor[1], v.titlefontcolor[2], v.titlefontcolor[3], v.titlefont, 1, false, false, 0);
            else
                API.drawText(language[playerlisttitles[i]], startX + lastwidthstitle + v.columnwidthpercent[playerlisttitlesindex[playerlisttitles[i]]] * v.completewidth / 2, titleStartY, v.titlefontscale, v.titlefontcolor[0], v.titlefontcolor[1], v.titlefontcolor[2], v.titlefontcolor[3], v.titlefont, 1, false, false, 0);
            lastwidthstitle += v.columnwidthpercent[playerlisttitlesindex[playerlisttitles[i]]] * v.completewidth;
        }
        API.drawRectangle(startX, startY, v.completewidth, len * v.columnheight, v.rectanglecolor[0], v.rectanglecolor[1], v.rectanglecolor[2], v.rectanglecolor[3]);
        let notshowcounter = 0;
        for (var i = 0 + scroll; i < len + scroll; i++) {
            if (playertable[i] !== undefined) {
                var lastwidths = 0;
                for (let j = 0; j < titleslength - 1; j++) {
                    var index = playerlisttitlesindex[playerlisttitles[j]];
                    API.drawText(playertable[i][index], startX + lastwidths + v.columnwidthpercent[index] * v.completewidth / 2, startY + (i - scroll) * v.columnheight, v.fontscale, v.fontcolor[0], v.fontcolor[1], v.fontcolor[2], v.fontcolor[3], v.font, 1, false, false, 0);
                    lastwidths += v.columnwidthpercent[index] * v.completewidth;
                }
            }
            else {
                API.drawText(otherlobbytable[notshowcounter].name + " (" + otherlobbytable[notshowcounter].amount + ")", startX + v.completewidth / 2, startY + (i - scroll) * v.columnheight, v.fontscale, v.fontcolor[0], v.fontcolor[1], v.fontcolor[2], v.fontcolor[3], v.font, 1, false, false, 0);
                notshowcounter++;
            }
        }
        if (len < playertablelength) {
            API.drawRectangle(startX + lastwidthstitle, titleStartY, v.scrollbarwidth, len * v.columnheight + v.titleheight, v.scrollbarbackcolor[0], v.scrollbarbackcolor[1], v.scrollbarbackcolor[2], v.scrollbarbackcolor[3]);
            var amountnotshown = playertablelength - len;
            var scrollbarheight = len * v.columnheight / (amountnotshown + 1);
            API.drawRectangle(startX + lastwidthstitle, startY + scroll * scrollbarheight, v.scrollbarwidth, scrollbarheight, v.scrollbarcolor[0], v.scrollbarcolor[1], v.scrollbarcolor[2], v.scrollbarcolor[3]);
        }
        API.drawRectangle(startX, bottomStartY, v.completewidth, v.bottomheight, v.bottomrectanglecolor[0], v.bottomrectanglecolor[1], v.bottomrectanglecolor[2], v.bottomrectanglecolor[3]);
        if (playertablelength - playerlistdata.maxplayers > 0) {
            if (API.isControlJustPressed(16)) {
                var plus = Math.ceil((playertablelength - playerlistdata.maxplayers) / 10);
                scroll = scroll + plus >= playertablelength - playerlistdata.maxplayers ? playertablelength - playerlistdata.maxplayers : scroll + plus;
            }
            else if (API.isControlJustPressed(17)) {
                var minus = Math.ceil((playertablelength - playerlistdata.maxplayers) / 10);
                scroll = scroll - minus <= 0 ? 0 : scroll - minus;
            }
        }
        else
            scroll = 0;
    }
    API.onKeyDown.connect(function (sender, e) {
        if (!API.isChatOpen()) {
            if (API.isControlJustPressed(20) && playerlistevent === null) {
                playerlistopenkey = e.KeyCode;
                scroll = 0;
                var tick = new Date().getTime();
                if (playerlistevent !== null)
                    playerlistevent.disconnect();
                if (tick - lastplayerlisttrigger >= 5000) {
                    lastplayerlisttrigger = tick;
                    playertablelength = 0;
                    playertable = [];
                    API.triggerServerEvent("onClientRequestPlayerListDatas");
                    playerlistevent = API.onUpdate.connect(drawPlayerList);
                }
                else {
                    playerlistevent = API.onUpdate.connect(drawPlayerList);
                }
            }
        }
    });
    API.onKeyUp.connect(function (sender, e) {
        if (playerlistevent !== null && e.KeyCode === playerlistopenkey) {
            playerlistevent.disconnect();
            playerlistevent = null;
        }
    });
    function sortArray(a, b) {
        if (a.teamorlobby !== b.teamorlobby) {
            if (a.teamorlobby === 0)
                return -1;
            else if (b.teamorlobby === 0)
                return 1;
            else
                return a.teamorlobby < b.teamorlobby ? -1 : 1;
        }
        else {
            if (a.playtime === b.playtime)
                return a.name < b.name ? -1 : 1;
            else
                return a.playtime > b.playtime ? -1 : 1;
        }
    }
    function sortArrayMainmenu(a, b) {
        if (a.teamorlobby !== b.teamorlobby) {
            if (a.playtime === "-")
                return 1;
            else if (b.playtime === "-")
                return -1;
            else if (a.teamorlobby === "mainmenu")
                return 1;
            else if (b.teamorlobby === "mainmenu")
                return -1;
            else
                return a.teamorlobby < b.teamorlobby ? -1 : 1;
        }
        else {
            return a.name < b.name ? -1 : 1;
        }
    }
    API.onServerEventTrigger.connect(function (eventName, args) {
        if (eventName === "giveRequestedPlayerListDatas") {
            playertable = [];
            inmainmenu = false;
            for (let i = 0; i < args[1].Count; i++) {
                playertable[i] = { name: args[0][i], playtime: args[1][i], kills: args[2][i], assists: args[3][i], deaths: args[4][i], teamorlobby: args[5][i] };
            }
            playertablelength = playertable.length;
            playertable.sort(sortArray);
            otherlobbytable = [];
            for (let i = 0; i < args[6].Count; i++) {
                otherlobbytable[i] = { name: args[6][i], amount: args[7][i] };
                playertablelength++;
            }
        }
        else if (eventName === "giveRequestedPlayerListDatasMainmenu") {
            playertable = [];
            inmainmenu = true;
            for (let i = 0; i < args[1].Count; i++) {
                playertable[i] = { name: args[0][i], playtime: args[1][i], kills: args[2][i], assists: args[3][i], deaths: args[4][i], teamorlobby: args[5][i] };
            }
            playertablelength = playertable.length;
            playertable.sort(sortArrayMainmenu);
            otherlobbytable = [];
            for (let i = 0; i < args[6].Count; i++) {
                otherlobbytable[i] = { name: args[6][i], amount: args[7][i] };
                playertablelength++;
            }
        }
    });
}
createScoreboard();
