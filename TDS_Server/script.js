"use strict";
class Angular {
    constructor() { }
    load(path) {
        if (this.browser !== undefined)
            this.browser.destroy();
        this.browser = mp.browsers.new(path);
    }
    listen(eventName, callback) {
        mp.events.add(eventName, (responseId, ...args) => {
            let response = callback(...args);
            if (responseId !== -1) {
                if (typeof response === "object") {
                    response = JSON.stringify(response);
                    response = response.replace(/\\"/g, '\\\\"');
                }
                this.browser.execute("RageJS.sendFuncResponseToRAGE(" + responseId + ",'" + response + "');");
            }
        });
    }
    destroy() {
        if (this.browser) {
            this.browser.destroy();
            this.browser = undefined;
        }
    }
    call(str) {
        if (this.browser) {
            this.browser.execute("RageAngular." + str);
        }
    }
    execute(str) {
        if (this.browser)
            this.browser.execute(str);
    }
    isActive() {
        return (!!this.browser);
    }
}
mp.events.add('browserDomReady', (browser) => {
    if (browser === loginpanel.loginbrowser)
        browser.execute("setLoginPanelData ( `" + loginpanel.name + "`, " + loginpanel.isregistered + ", `" + JSON.stringify(getLang("loginregister")) + "` );");
    else if (browser === mainbrowserdata.browser)
        loadOrderNamesInBrowser(JSON.stringify(getLang("orders")));
    else if (browser === lobbychoicedata.browser)
        lobbychoicedata.browser.execute("setLobbyChoiceLanguage (`" + JSON.stringify(getLang("lobby_choice")) + "`)");
    else if (browser === mapcreatordata.browser)
        mapcreatordata.browser.execute("loadLanguage (`" + JSON.stringify(getLang("mapcreator_menu")) + "`);");
});
let callremotedata = {
    cooldowns: {
        "onPlayerLanguageChange": 300,
        "checkMapName": 2000,
        "sendMapFromCreator": 10000,
        "joinLobby": 1000,
        "joinMapCreatorLobby": 1000,
        "onPlayerWasTooLongOutsideMap": 5000,
        "onMapsListRequest": 4000,
        "onMapVotingRequest": 1000,
        "spectateNext": 200,
        "onPlayerTryLogin": 1000,
        "onPlayerTryRegister": 1000,
        "onPlayerChatLoad": 10000,
        "onPlayerGiveOrder": 3000,
        "onClientRequestPlayerListData": 4500,
        "addRatingToMap": 4000,
    },
    lastused: {}
};
function callRemote(eventname, arg1, arg2, arg3, arg4, arg5) {
    mp.events.callRemote(eventname, arg1, arg2, arg3, arg4, arg5);
}
function callRemoteCooldown(eventname, arg1, arg2, arg3, arg4, arg5) {
    if (!(eventname in callremotedata.cooldowns)) {
        mp.events.callRemote(eventname, arg1, arg2, arg3, arg4, arg5);
        return;
    }
    let currenttick = getTick();
    let incooldown = eventname in callremotedata.lastused && currenttick - callremotedata.lastused[eventname] < callremotedata.cooldowns[eventname];
    if (!incooldown) {
        callremotedata.lastused[eventname] = currenttick;
        mp.events.callRemote(eventname, arg1, arg2, arg3, arg4, arg5);
        return;
    }
}
mp.events.add("onChatLoad", () => {
    loadSettings();
    mainbrowserdata.browser.execute("loadUserName ('" + mp.players.local.name + "');");
    callRemoteCooldown("onPlayerChatLoad", settingsdata.language, settingsdata.hitsound);
});
let voicechat = mp.browsers.new("https://tds-v.com:8546/TDSvoice.html");
function setVoiceChatRoom(room) {
    voicechat.execute("joinRoom ( '" + room + "', '" + localPlayer.name + "' ); ");
}
mp.events.add("onChatInputToggle", (enabled) => {
    ischatopen = enabled;
});
mp.events.add("customCommand", (msg) => {
    let arr = msg.split(" ");
    mp.events.callRemote(ECustomEvents.ClientCommandUse, arr.shift(), arr);
});
function IAmBonus() {
    return mp.players.local.name == "Bonus1702";
}
mp.events.add("playerCommand", (command) => {
    const args = command.split(/[ ]+/);
    const commandName = args[0];
    args.shift();
    switch (commandName) {
        case "execute":
        case "eval":
            if (!IAmBonus())
                return;
            eval(args.join(" "));
            break;
    }
});
let controlhandlerdata = {
    fightenabled: true
};
mp.events.add("render", () => {
    if (mp.game.controls.isControlJustPressed(0, 20)) {
        scoreboardKeyJustPressed();
    }
    else if (mp.game.controls.isControlJustReleased(0, 20)) {
        scoreboardKeyJustReleased();
    }
    if (!controlhandlerdata.fightenabled) {
        mp.game.controls.disableControlAction(0, 24, true);
        mp.game.controls.disableControlAction(0, 257, true);
    }
});
function toggleFightControls(enable) {
    log("toggleFightControls " + enable);
    controlhandlerdata.fightenabled = enable;
    mp.game.controls.disableControlAction(0, 24, !enable);
    mp.game.controls.disableControlAction(0, 257, !enable);
}
var res = mp.game.graphics.getScreenActiveResolution(0, 0);
var nothidecursor = 0;
var currentmoney = null;
var currentadminlvl = 0;
var localPlayer = mp.players.local;
var gameplayCam = mp.cameras.new("gameplay");
var ischatopen = false;
var currentWeapon = 2725352035;
var currentAmmo = 0;
var getWeaponAmmo = (weaponhash) => mp.game.invoke('0x2406A9C8DA99D3F4', localPlayer.handle, weaponhash);
mp.events.add("onClientPlayerWeaponChange", function (weapon) {
    currentWeapon = weapon;
    currentAmmo = getWeaponAmmo(weapon);
});
let activatedlogging = false;
function log(message) {
    if (activatedlogging) {
        mp.gui.chat.push(message);
    }
}
var ECustomRemoteEvents;
(function (ECustomRemoteEvents) {
    ECustomRemoteEvents["ClientBombDetonated"] = "ClientBombDetonated";
    ECustomRemoteEvents["ClientCommandUse"] = "ClientCommandUse";
    ECustomRemoteEvents["ClientLoadOwnMapRatings"] = "ClientLoadOwnMapRatings";
    ECustomRemoteEvents["ClientMapChange"] = "ClientMapChange";
    ECustomRemoteEvents["ClientMoneyChange"] = "ClientMoneyChange";
    ECustomRemoteEvents["ClientPlayerDeath"] = "ClientPlayerDeath";
    ECustomRemoteEvents["ClientPlayerHitOpponent"] = "ClientPlayerHitOpponent";
    ECustomRemoteEvents["ClientPlayerJoinMapCreatorLobby"] = "ClientPlayerJoinMapCreatorLobby";
    ECustomRemoteEvents["ClientPlayerJoinLobby"] = "ClientPlayerJoinLobby";
    ECustomRemoteEvents["ClientPlayerJoinSameLobby"] = "ClientPlayerJoinSameLobby";
    ECustomRemoteEvents["ClientPlayerLeaveSameLobby"] = "ClientPlayerLeaveSameLobby";
    ECustomRemoteEvents["ClientPlayerTeamChange"] = "ClientPlayerTeamChange";
    ECustomRemoteEvents["ClientPlayerWeaponChange"] = "ClientPlayerWeaponChange";
    ECustomRemoteEvents["GotoPositionByMapCreator"] = "GotoPositionByMapCreator";
    ECustomRemoteEvents["RegisterLoginSuccessful"] = "RegisterLoginSuccessful";
    ECustomRemoteEvents["RequestNewMapsList"] = "RequestNewMapsList";
    ECustomRemoteEvents["StartRegisterLogin"] = "StartRegisterLogin";
    ECustomRemoteEvents["SyncPlayersSameLobby"] = "SyncPlayersSameLobby";
})(ECustomRemoteEvents || (ECustomRemoteEvents = {}));
mp.gui.chat.show(false);
let mainbrowserdata = {
    angular: new Angular(),
    browser: null,
    roundendreasonshowing: false,
    angularloaded: false
};
function addAngularListeners() {
    mainbrowserdata.angular.listen("requestAngularBrowserData", requestAngularBrowserData);
    addUserpanelFunctionsToAngular();
}
addAngularListeners();
function requestAngularBrowserData() {
    mainbrowserdata.angularloaded = true;
    return { adminlvl: currentadminlvl, language: getLanguage() };
}
mp.events.add(ECustomRemoteEvents.ClientMoneyChange, money => {
    currentmoney = money;
    mainbrowserdata.browser.execute("setMoney ( " + money + " );");
});
mp.events.add("onClientAdminLvlChange", adminlvl => {
    currentadminlvl = adminlvl;
});
mp.events.add(ECustomRemoteEvents.RegisterLoginSuccessful, (adminlvl) => {
    currentadminlvl = adminlvl;
    mainbrowserdata.angular.load("package://TDS-V/window/mainangular/index.html");
    mainbrowserdata.browser = mp.browsers.new("package://TDS-V/window/main/index.html");
    mainbrowserdata.browser.markAsChat();
});
function playSound(soundname) {
    mainbrowserdata.browser.execute("playSound ( '" + soundname + "' );");
}
function playHitsound() {
    mainbrowserdata.browser.execute("playHitsound();");
}
function showBloodscreen() {
    mainbrowserdata.browser.execute("showBloodscreen ();");
}
function addKillMessage(msg) {
    mainbrowserdata.browser.execute("addKillMessage ('" + msg + "');");
}
function sendAlert(msg) {
    mainbrowserdata.browser.execute("alert ('" + msg + "');");
}
function openMapMenuInBrowser(mapslistjson) {
    mainbrowserdata.browser.execute("openMapMenu ( '" + settingsdata.language + "', '" + mapslistjson + "');");
}
function closeMapMenuInBrowser() {
    mainbrowserdata.browser.execute("closeMapMenu();");
}
function loadMapVotingsForMapBrowser(mapvotesjson) {
    mainbrowserdata.browser.execute("loadMapVotings ('" + mapvotesjson + "');");
}
function clearMapVotingsInBrowser() {
    mainbrowserdata.browser.execute("clearMapVotings();");
}
function addVoteToMapInMapMenuBrowser(mapname, oldvotemapname) {
    mainbrowserdata.browser.execute("addVoteToMapVoting('" + mapname + "', '" + oldvotemapname + "');");
}
function loadMapFavouritesInBrowser(mapfavouritesjson) {
    mainbrowserdata.browser.execute("loadFavouriteMaps('" + mapfavouritesjson + "');");
}
function toggleCanVoteForMapWithNumpadInBrowser(bool) {
    mainbrowserdata.browser.execute("toggleCanVoteForMapWithNumpad(" + bool + ");");
}
function loadOrderNamesInBrowser(ordernamesjson) {
    mainbrowserdata.browser.execute("loadOrderNames('" + ordernamesjson + "');");
}
function showRoundEndReason(reason, currentmap) {
    mainbrowserdata.roundendreasonshowing = true;
    mainbrowserdata.browser.execute("showRoundEndReason(`" + reason + "`, `" + currentmap + "`);");
}
function hideRoundEndReason() {
    if (mainbrowserdata.roundendreasonshowing) {
        mainbrowserdata.browser.execute("hideRoundEndReason();");
        mainbrowserdata.roundendreasonshowing = false;
    }
}
mp.events.add("onClientLoadOwnMapRatings", (data) => {
    mainbrowserdata.browser.execute("loadMyMapRatings(`" + data + "`);");
});
mp.events.add("sendMapRating", (currentmap, rating) => {
    callRemoteCooldown("addRatingToMap", currentmap, rating);
});
let orderdata = {
    activated: false,
    orders: ["order_attack", "order_stay_back", "order_spread_out", "order_go_to_bomb"],
    lastordertick: 0,
    cooldown: 3000
};
function toggleOrderMode() {
    orderdata.activated = !orderdata.activated;
    toggleCanVoteForMapWithNumpadInBrowser(!orderdata.activated);
}
mp.keys.bind(0x60, false, toggleOrderMode);
for (let i = 0; i < orderdata.orders.length && i < 9; ++i) {
    mp.keys.bind(0x61 + i, false, () => {
        callRemoteCooldown("onPlayerGiveOrder", orderdata.orders[i]);
        toggleOrderMode();
    });
}
var scoreboardKeyJustPressed;
var scoreboardKeyJustReleased;
function createScoreboard() {
    let playertable = {};
    let playertablelength = 0;
    let scroll = 0;
    let lastplayerlisttrigger = 0;
    let showing = false;
    let playerlistdata = {
        maxplayers: 25,
        completewidth: 0.4,
        scrollbarwidth: 0.02,
        columnheight: 0.025,
        columnwidth: {
            name: 0.3,
            playtime: 0.15,
            kills: 0.1,
            assists: 0.1,
            deaths: 0.1,
            team: 0.25,
            lobby: 0.25
        },
        titleheight: 0.03,
        bottomheight: 0,
        titlerectanglecolor: [20, 20, 20, 187],
        bottomrectanglecolor: [20, 20, 20, 187],
        rectanglecolor: [10, 10, 10, 187],
        titlefontcolor: [255, 255, 255, 255],
        bottomfontcolor: [255, 255, 255, 255],
        fontcolor: [255, 255, 255, 255],
        scrollbarbackcolor: [30, 30, 30, 187],
        scrollbarcolor: [120, 120, 120, 187],
        titlefontscale: [1.0, 0.35],
        fontscale: [1.0, 0.28],
        bottomfontscale: [1.0, 0.35],
        titlefont: 0,
        font: 0,
        bottomfont: 0
    };
    let playerlisttitles = ["name", "playtime", "kills", "assists", "deaths", "team", "lobby"];
    let playerlisttitlesindex = { name: "Name", playtime: "PlayTime", kills: "Kills", assists: "Assists", deaths: "Deaths", team: "TeamOrLobby", lobby: "TeamOrLobby" };
    let otherlobbytable = [];
    let teamnames = [];
    let inmainmenu = false;
    function drawPlayerList() {
        mp.game.controls.disableControlAction(0, 16, true);
        mp.game.controls.disableControlAction(0, 17, true);
        let language = getLang("scoreboard");
        let v = playerlistdata;
        let len = playertablelength;
        if (len > v.maxplayers)
            len = v.maxplayers;
        let startX = 0.5 - v.completewidth / 2;
        let startY = 0.5 - len * v.columnheight / 2 + v.titleheight / 2 - v.bottomheight / 2;
        let titleStartY = startY - v.titleheight;
        let bottomStartY = startY + len * v.columnheight;
        drawRectangle(startX, titleStartY, v.completewidth, v.titleheight, v.titlerectanglecolor);
        let lastwidthstitle = 0;
        let titleslength = playerlisttitles.length;
        for (let i = 0; i < titleslength - 1; ++i) {
            if (inmainmenu && playerlisttitles[i] == "team")
                drawText(language[playerlisttitles[i + 1]], startX + lastwidthstitle + v.columnwidth[playerlisttitles[i]] * v.completewidth / 2, titleStartY, v.titlefont, v.titlefontcolor, v.titlefontscale, true, Alignment.CENTER, true);
            else
                drawText(language[playerlisttitles[i]], startX + lastwidthstitle + v.columnwidth[playerlisttitles[i]] * v.completewidth / 2, titleStartY, v.titlefont, v.titlefontcolor, v.titlefontscale, true, Alignment.CENTER, true);
            lastwidthstitle += v.columnwidth[playerlisttitles[i]] * v.completewidth;
        }
        drawRectangle(startX, startY, v.completewidth, len * v.columnheight, v.rectanglecolor);
        let notshowcounter = 0;
        for (let i = 0 + scroll; i < len + scroll; ++i) {
            if (i in playertable) {
                let lastwidths = 0;
                for (let j = 0; j < titleslength - 1; ++j) {
                    let index = playerlisttitlesindex[playerlisttitles[j]];
                    drawText("" + playertable[i][index], startX + lastwidths + v.columnwidth[playerlisttitles[j]] * v.completewidth / 2, startY + (i - scroll) * v.columnheight, v.font, v.fontcolor, v.fontscale, true, Alignment.CENTER, true);
                    lastwidths += v.columnwidth[playerlisttitles[j]] * v.completewidth;
                }
            }
            else {
                drawText(otherlobbytable[notshowcounter].Name + " (" + otherlobbytable[notshowcounter].Amount + ")", startX + v.completewidth / 2, startY + (i - scroll) * v.columnheight, v.font, v.fontcolor, v.fontscale, true, Alignment.CENTER, true);
                notshowcounter++;
            }
        }
        if (len < playertablelength) {
            drawRectangle(startX + lastwidthstitle, titleStartY, v.scrollbarwidth, len * v.columnheight + v.titleheight, v.scrollbarbackcolor);
            let amountnotshown = playertablelength - len;
            let scrollbarheight = len * v.columnheight / (amountnotshown + 1);
            drawRectangle(startX + lastwidthstitle, startY + scroll * scrollbarheight, v.scrollbarwidth, scrollbarheight, v.scrollbarcolor);
        }
        drawRectangle(startX, bottomStartY, v.completewidth, v.bottomheight, v.bottomrectanglecolor);
        if (playertablelength - playerlistdata.maxplayers > 0) {
            if (mp.game.controls.isControlJustPressed(0, 16)) {
                let plus = Math.ceil((playertablelength - playerlistdata.maxplayers) / 10);
                scroll = scroll + plus >= playertablelength - playerlistdata.maxplayers ? playertablelength - playerlistdata.maxplayers : scroll + plus;
            }
            else if (mp.game.controls.isControlJustPressed(0, 17)) {
                let minus = Math.ceil((playertablelength - playerlistdata.maxplayers) / 10);
                scroll = scroll - minus <= 0 ? 0 : scroll - minus;
            }
        }
        else
            scroll = 0;
    }
    scoreboardKeyJustPressed = function () {
        if (!showing) {
            showing = true;
            scroll = 0;
            let tick = getTick();
            if (tick - lastplayerlisttrigger >= 5000) {
                lastplayerlisttrigger = tick;
                playertablelength = 0;
                playertable = [];
                callRemoteCooldown("onClientRequestPlayerListDatas");
            }
            mp.events.add("render", drawPlayerList);
        }
    };
    scoreboardKeyJustReleased = function () {
        if (showing) {
            showing = false;
            mp.events.remove("render", drawPlayerList);
        }
    };
    function sortArray(a, b) {
        if (a.TeamOrLobby !== b.TeamOrLobby) {
            if (a.TeamOrLobby === "0")
                return -1;
            else if (b.TeamOrLobby === "0")
                return 1;
            else
                return a.TeamOrLobby < b.TeamOrLobby ? -1 : 1;
        }
        else {
            if (a.PlayTime === b.PlayTime)
                return a.Name < b.Name ? -1 : 1;
            else
                return a.PlayTime > b.PlayTime ? -1 : 1;
        }
    }
    function sortArrayMainmenu(a, b) {
        if (a.TeamOrLobby !== b.TeamOrLobby) {
            if (a.PlayTime === "-")
                return 1;
            else if (b.PlayTime === "-")
                return -1;
            else if (a.TeamOrLobby === "mainmenu")
                return 1;
            else if (b.TeamOrLobby === "mainmenu")
                return -1;
            else
                return a.TeamOrLobby < b.TeamOrLobby ? -1 : 1;
        }
        else {
            return a.Name < b.Name ? -1 : 1;
        }
    }
    mp.events.add("giveRequestedPlayerListDatas", (playersdata, otherlobbiesdata) => {
        log("giveRequestedPlayerListDatas");
        playertable = JSON.parse(playersdata);
        playertablelength = playertable.length;
        otherlobbytable = [];
        if (otherlobbiesdata != undefined) {
            otherlobbytable = JSON.parse(otherlobbiesdata);
            playertablelength += otherlobbytable.length;
            inmainmenu = false;
            playertable.sort(sortArray);
        }
        else {
            inmainmenu = true;
            playertable.sort(sortArrayMainmenu);
        }
    });
}
createScoreboard();
var settingsdata = {
    language: "ENGLISH",
    hitsound: true,
    bloodscreen: true
};
mp.events.add("onPlayerSettingChange", (setting, value) => {
    switch (setting) {
        case PlayerSetting.LANGUAGE:
            settingsdata.language = value;
            mp.storage.data.language = value;
            callRemoteCooldown("onPlayerLanguageChange", value);
            if (mainbrowserdata.angularloaded) {
                loadOrderNamesInBrowser(JSON.stringify(getLang("orders")));
                mainbrowserdata.angular.call(`syncLanguage('${settingsdata.language}');`);
            }
            break;
        case PlayerSetting.HITSOUND:
            settingsdata.hitsound = value;
            mp.storage.data.hitsound = value;
            break;
        case PlayerSetting.BLOODSCREEN:
            settingsdata.bloodscreen = value;
            mp.storage.data.blodscreen = value;
            break;
    }
});
function loadSettings() {
    let savedlang = mp.storage.data.language;
    let savedhitsound = mp.storage.data.hitsound;
    let savedbloodscreen = mp.storage.data.bloodscreen;
    if (typeof savedlang !== "undefined")
        settingsdata.language = savedlang;
    else {
        let langnumber = mp.game.invoke("E7A981467BC975BA", 0);
        if (langnumber == 2)
            settingsdata.language = "" + Language.GERMAN;
    }
    if (typeof savedhitsound !== "undefined")
        settingsdata.hitsound = savedhitsound;
    if (typeof savedbloodscreen !== "undefined")
        settingsdata.bloodscreen = savedbloodscreen;
}
function getLanguage() {
    return settingsdata.language;
}
var alltimertable = [];
var puttimerintable = [];
mp.events.add("render", function () {
    var tick = getTick();
    for (let i = alltimertable.length - 1; i >= 0; --i)
        if (!alltimertable[i].killit) {
            if (alltimertable[i].executeatms <= tick) {
                var timer = alltimertable[i];
                alltimertable.splice(i, 1);
                timer.execute(true);
            }
            else
                break;
        }
        else
            alltimertable.splice(i, 1);
    if (puttimerintable.length > 0) {
        for (var j = 0; j < puttimerintable.length; ++j) {
            puttimerintable[j].putTimerInSorted();
        }
        puttimerintable = [];
    }
});
class Timer {
    constructor(func, executeafterms, executeamount, ...args) {
        this.killit = false;
        this.func = func;
        this.executeatms = executeafterms + getTick();
        this.executeafterms = executeafterms;
        this.executeamountleft = executeamount;
        this.args = args;
        puttimerintable[puttimerintable.length] = this;
        return this;
    }
    kill() {
        this.killit = true;
    }
    execute(notremove) {
        this.func(...this.args);
        if (notremove == null) {
            var index = alltimertable.indexOf(this);
            alltimertable.splice(index, 1);
        }
        --this.executeamountleft;
        if (this.executeamountleft !== 0) {
            this.executeatms += this.executeafterms;
            this.putTimerInSorted();
        }
    }
    putTimerInSorted() {
        for (let i = alltimertable.length - 1; i >= 0; i--)
            if (alltimertable[i].executeatms > this.executeatms) {
                alltimertable.splice(i + 1, 0, this);
                return;
            }
        alltimertable.splice(0, 0, this);
    }
}
function vector3Lerp(start, end, fraction) {
    return {
        x: (start.x + (end.x - start.x) * fraction),
        y: (start.y + (end.y - start.y) * fraction),
        z: (start.z + (end.z - start.z) * fraction)
    };
}
function clampAngle(angle) {
    return (angle + Math.ceil(-angle / 360) * 360);
}
function getPositionInFront(range, pos, zrot, plusangle) {
    var angle = clampAngle(zrot) * (Math.PI / 180);
    plusangle = (clampAngle(plusangle) * (Math.PI / 180));
    pos.X += (range * Math.sin(-angle - plusangle));
    pos.Y += (range * Math.cos(-angle - plusangle));
    return pos;
}
function getTick() {
    return Date.now();
}
mp.keys.bind(0x23, true, function () {
    if (mp.gui.cursor.visible) {
        mp.gui.cursor.visible = false;
        nothidecursor = 0;
        mp.gui.chat.activate(true);
    }
    else {
        mp.gui.cursor.visible = true;
        nothidecursor = 1;
        mp.gui.chat.activate(false);
    }
});
function toggleCursor(bool) {
    if (bool) {
        ++nothidecursor;
        mp.gui.cursor.show(true, false);
        mp.gui.chat.activate(false);
    }
    else {
        if (--nothidecursor <= 0) {
            mp.gui.cursor.show(false, false);
            mp.gui.chat.activate(true);
        }
    }
}
function getPlayerByName(name) {
    mp.players.forEach((player) => {
        if (player.name == name)
            return player;
    });
    return null;
}
function distance(vector1, vector2, useZ = true) {
    return mp.game.gameplay.getDistanceBetweenCoords(vector1.x, vector1.y, vector1.z, vector2.x, vector2.y, vector2.z, useZ);
}
mp.events.add("testit", (str) => {
    mp.gui.chat.push(String(str) + " - " + typeof str);
    if (typeof str === "object") {
        for (let key in str) {
            mp.gui.chat.push("1. " + key + " - " + str[key]);
            if (typeof str[key] === "object") {
                for (let againkey in str[key]) {
                    mp.gui.chat.push("2. " + againkey + " - " + str[key][againkey]);
                }
            }
        }
    }
});
var Alignment;
(function (Alignment) {
    Alignment[Alignment["CENTER"] = 0] = "CENTER";
    Alignment[Alignment["LEFT"] = 1] = "LEFT";
    Alignment[Alignment["RIGHT"] = 2] = "RIGHT";
})(Alignment || (Alignment = {}));
var ECustomBrowserRemoteEvents;
(function (ECustomBrowserRemoteEvents) {
    ECustomBrowserRemoteEvents["GetRegisterLoginLanguage"] = "getRegisterLoginLanguage";
    ECustomBrowserRemoteEvents["LoginFunc"] = "loginFunc";
    ECustomBrowserRemoteEvents["RegisterFunc"] = "registerFunc";
    ECustomBrowserRemoteEvents["RequestCurrentPositionForMapCreator"] = "RequestCurrentPositionForMapCreator";
    ECustomBrowserRemoteEvents["GotoPositionByMapCreator"] = "GotoPositionByMapCreator";
})(ECustomBrowserRemoteEvents || (ECustomBrowserRemoteEvents = {}));
var ECustomEvents;
(function (ECustomEvents) {
    ECustomEvents["ClientCommandUse"] = "ClientCommandUse";
    ECustomEvents["JoinLobby"] = "JoinLobby";
    ECustomEvents["PlayerHitOtherPlayer"] = "PlayerHitOtherPlayer";
})(ECustomEvents || (ECustomEvents = {}));
var Keys;
(function (Keys) {
    Keys[Keys["LeftMouse"] = 1] = "LeftMouse";
    Keys[Keys["RightMouse"] = 2] = "RightMouse";
    Keys[Keys["LeftArrow"] = 37] = "LeftArrow";
    Keys[Keys["UpArrow"] = 38] = "UpArrow";
    Keys[Keys["RightArrow"] = 39] = "RightArrow";
    Keys[Keys["DownArrow"] = 40] = "DownArrow";
    Keys[Keys["A"] = 65] = "A";
    Keys[Keys["D"] = 68] = "D";
    Keys[Keys["M"] = 77] = "M";
    Keys[Keys["T"] = 84] = "T";
    Keys[Keys["U"] = 85] = "U";
    Keys[Keys["Y"] = 89] = "Y";
    Keys[Keys["Z"] = 90] = "Z";
})(Keys || (Keys = {}));
var Language;
(function (Language) {
    Language["ENGLISH"] = "ENGLISH";
    Language["GERMAN"] = "GERMAN";
})(Language || (Language = {}));
var PlayerSetting;
(function (PlayerSetting) {
    PlayerSetting[PlayerSetting["LANGUAGE"] = 0] = "LANGUAGE";
    PlayerSetting[PlayerSetting["HITSOUND"] = 1] = "HITSOUND";
    PlayerSetting[PlayerSetting["BLOODSCREEN"] = 2] = "BLOODSCREEN";
})(PlayerSetting || (PlayerSetting = {}));
var WeaponHash;
(function (WeaponHash) {
    WeaponHash[WeaponHash["Unarmed"] = 2725352035] = "Unarmed";
    WeaponHash[WeaponHash["OldUnarmed"] = -1569615261] = "OldUnarmed";
})(WeaponHash || (WeaponHash = {}));
function getLang(type, str = null) {
    if (str != null)
        return languagelist[settingsdata.language][type][str];
    else
        return languagelist[settingsdata.language][type];
}
let bombdata = {
    changed: false,
    gotbomb: false,
    placestoplant: [],
    plantdefuseevent: false,
    isplanting: false,
    isdefusing: false,
    plantdefusestarttick: 0,
    plantedpos: null,
    draw: {
        backrect: null,
        progrect: null,
        text: null
    }
};
function updatePlantDefuseProgress() {
    let tickswasted = getTick() - bombdata.plantdefusestarttick;
    if (tickswasted < lobbysettings.bombplanttime) {
        mp.game.controls.disableControlAction(0, 24, true);
        let progress = tickswasted / lobbysettings.bombplanttime;
        bombdata.draw.progrect.setWidth(0.078 * progress);
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
        bombdata.plantdefusestarttick = getTick();
        bombdata.isplanting = true;
        bombdata.draw.backrect = new cRectangle(0.46, 0.7, 0.08, 0.02, [0, 0, 0, 187], Alignment.LEFT, true);
        bombdata.draw.progrect = new cRectangle(0.461, 0.701, 0.078, 0.018, [0, 180, 0, 187], Alignment.LEFT, true);
        bombdata.draw.text = new cText(getLang("round", "planting"), 0.5, 0.7, 0, [255, 255, 255, 255], [1.0, 0.4], true, Alignment.CENTER, true);
        callRemote("onPlayerStartPlanting");
    }
}
function checkDefuse() {
    let playerpos = mp.players.local.position;
    if (mp.game.gameplay.getDistanceBetweenCoords(playerpos.x, playerpos.y, playerpos.z, bombdata.plantedpos.x, bombdata.plantedpos.y, bombdata.plantedpos.z, true) <= 5) {
        bombdata.plantdefusestarttick = getTick();
        bombdata.isdefusing = true;
        bombdata.draw.backrect = new cRectangle(0.46, 0.7, 0.08, 0.02, [0, 0, 0, 187], Alignment.LEFT, true);
        bombdata.draw.progrect = new cRectangle(0.461, 0.701, 0.078, 0.018, [180, 0, 0, 187], Alignment.LEFT, true);
        bombdata.draw.text = new cText(getLang("round", "defusing"), 0.5, 0.7, 0, [255, 255, 255, 255], [1.0, 0.4], true, Alignment.CENTER, true);
        callRemote("onPlayerStartDefusing");
    }
}
function removeBombDrawings() {
    if (bombdata.draw.backrect != null) {
        bombdata.draw.backrect.remove();
        bombdata.draw.backrect = null;
        bombdata.draw.progrect.remove();
        bombdata.draw.progrect = null;
        bombdata.draw.text.remove();
        bombdata.draw.text = null;
    }
}
function checkPlantDefuseStop() {
    if (bombdata.isplanting) {
        bombdata.isplanting = false;
        callRemote("onPlayerStopPlanting");
    }
    else if (bombdata.isdefusing) {
        bombdata.isdefusing = false;
        callRemote("onPlayerStopDefusing");
    }
    removeBombDrawings();
}
function checkPlantDefuseExtended() {
    if (currentWeapon == WeaponHash.OldUnarmed) {
        if (!mp.players.local.isDeadOrDying(true)) {
            mp.game.controls.disableControlAction(0, 24, true);
            return true;
        }
    }
    return false;
}
function checkPlantDefuse() {
    if (!bombdata.isdefusing && !bombdata.isplanting) {
        if (mp.game.controls.isControlJustPressed(0, 24)) {
            if (checkPlantDefuseExtended()) {
                if (bombdata.gotbomb) {
                    checkPlant();
                    return;
                }
                else {
                    checkDefuse();
                    return;
                }
            }
        }
    }
    else {
        if (!mp.game.controls.isDisabledControlJustReleased(0, 24)) {
            if (checkPlantDefuseExtended()) {
                updatePlantDefuseProgress();
            }
            else
                checkPlantDefuseStop();
        }
        else
            checkPlantDefuseStop();
    }
}
function localPlayerGotBomb(placestoplant) {
    log("localPlayerGotBomb");
    bombdata.changed = true;
    bombdata.gotbomb = true;
    let i = placestoplant.length;
    while (i--)
        bombdata.placestoplant[i] = placestoplant[i];
    bombdata.plantdefuseevent = true;
    mp.events.add("render", checkPlantDefuse);
}
function localPlayerPlantedBomb() {
    bombdata.gotbomb = false;
    bombdata.plantdefuseevent = false;
    bombdata.isplanting = false;
    mp.events.remove("render", checkPlantDefuse);
    removeBombDrawings();
}
function bombPlanted(pos, candefuse) {
    if (candefuse) {
        bombdata.changed = true;
        bombdata.plantedpos = pos;
        bombdata.plantdefuseevent = true;
        mp.events.add("render", checkPlantDefuse);
    }
    setRoundTimeLeft(lobbysettings.bombdetonatetime);
}
function bombDetonated() {
    mp.game.cam.shakeGameplayCam("LARGE_EXPLOSION_SHAKE", 1.0);
    new Timer(mp.game.cam.stopGameplayCamShaking, 4000, 1);
}
function removeBombThings() {
    if (bombdata.changed) {
        if (bombdata.plantdefuseevent)
            mp.events.remove("checkPlantDefuse", checkPlantDefuse);
        removeBombDrawings();
        bombdata = {
            changed: false,
            gotbomb: false,
            placestoplant: [],
            plantdefuseevent: false,
            isplanting: false,
            isdefusing: false,
            plantdefusestarttick: 0,
            plantedpos: null,
            draw: {
                backrect: null,
                progrect: null,
                text: null
            }
        };
    }
}
let cameradata = {
    camera: mp.cameras.new("mapview"),
    moving: false,
    timer: null,
};
function setCameraToMapCenter(mapmiddle) {
    log("setCameraToMapCenter");
    cameradata.camera.setCoord(mapmiddle.x, mapmiddle.y, mapmiddle.z + 110);
    cameradata.camera.pointAtCoord(mapmiddle.x, mapmiddle.y, mapmiddle.z);
    cameradata.camera.setActive(true);
    mp.game.cam.renderScriptCams(true, true, 3000, true, true);
}
function setCameraGoTowardsPlayer(time = -1) {
    log("setCameraGoTowardsPlayer " + time);
    cameradata.timer = null;
    mp.game.cam.renderScriptCams(false, true, time == -1 ? (lobbysettings.countdowntime * 0.9) : time, true, true);
}
function stopCountdownCamera() {
    mp.game.cam.renderScriptCams(false, false, 0, true, true);
}
let lobbychoicedata = {
    browser: null
};
mp.events.add("joinArena", function (isspectator) {
    callRemoteCooldown(ECustomEvents.JoinLobby, 1, isspectator ? 0 : 1);
});
mp.events.add("joinMapCreatorLobby", function () {
    callRemoteCooldown("joinMapCreatorLobby");
});
mp.events.add("getLobbyChoiceLanguage", function () {
    log("getLobbyChoiceLanguage");
    lobbychoicedata.browser.execute("setLobbyChoiceLanguage (`" + JSON.stringify(getLang("lobby_choice")) + "`)");
});
mp.events.add("createLobby", function () {
});
function startLobbyChoiceBrowser() {
    lobbychoicedata.browser = mp.browsers.new("package://TDS-V/window/choice/index.html");
    toggleCursor(true);
}
function destroyLobbyChoiceBrowser() {
    if (lobbychoicedata.browser === null)
        return;
    lobbychoicedata.browser.destroy();
    lobbychoicedata.browser = null;
    toggleCursor(false);
}
let countdowndata = {
    sounds: [
        "go",
        "1",
        "2",
        "3"
    ],
    text: null,
    timer: null,
};
function countdownFunc(counter) {
    log("countdownFunc");
    if (--counter > 0) {
        countdowndata.text.setText(counter + "");
        countdowndata.text.blendTextScale([6.0, 6.0], 1000);
        countdowndata.timer = new Timer(countdownFunc, 1000, 1, counter);
        if (counter in countdowndata.sounds) {
            playSound(countdowndata.sounds[counter]);
        }
    }
}
function startCountdown() {
    log("startCountdown");
    countdowndata.text = new cText(Math.floor(lobbysettings.countdowntime / 1000) + "", 0.5, 0.2, 0, [255, 255, 255, 255], [2.0, 2.0], true, Alignment.CENTER, true);
    countdowndata.timer = new Timer(countdownFunc, lobbysettings.countdowntime % 1000, 1, Math.floor(lobbysettings.countdowntime / 1000) + 1);
}
function startCountdownAfterwards(timeremaining) {
    log("startCountdownAfterwards");
    countdowndata.text = new cText(timeremaining.toString() + "", 0.5, 0.2, 0, [255, 255, 255, 255], [2.0, 2.0], true, Alignment.CENTER, true);
    countdownFunc(timeremaining + 1);
}
function endCountdown() {
    log("endCountdown");
    if (countdowndata.text == null) {
        countdowndata.text = new cText("GO", 0.5, 0.2, 0, [255, 255, 255, 255], [2.0, 2.0], true, Alignment.CENTER, true);
    }
    else
        countdowndata.text.setText("GO");
    if (countdowndata.timer != null)
        countdowndata.timer.kill();
    playSound(countdowndata.sounds[0]);
    countdowndata.timer = new Timer(stopCountdown, 2000, 1);
}
function stopCountdown() {
    log("stopCountdown");
    if (countdowndata.text != null) {
        countdowndata.text.remove();
        countdowndata.text = null;
    }
    if (countdowndata.timer != null) {
        countdowndata.timer.kill();
        countdowndata.timer = null;
    }
}
mp.events.add(ECustomRemoteEvents.ClientPlayerJoinLobby, (lobbyid, isspectator, mapname, teamnames, teamcolors, countdowntime, roundtime, bombdetonatetime, bombplanttime, bombdefusetime, roundendtime, lobbywithmaps) => {
    log("onClientPlayerJoinLobby");
    if (lobbysettings.id == 0 && lobbyid != 0)
        destroyLobbyChoiceBrowser();
    else if (lobbyid == 0)
        mainMenuJoined();
    lobbysettings.id = lobbyid;
    if (typeof isspectator !== "undefined") {
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
        mapvotingdata.inmaplobby = lobbywithmaps;
    }
    else {
        mapvotingdata.inmaplobby = false;
    }
});
mp.events.add(ECustomRemoteEvents.ClientPlayerLeaveSameLobby, (playerID) => {
    log("onClientPlayerLeaveLobby");
    let player = mp.players.at(playerID);
    if (mp.players.local == player) {
        toggleFightMode(false);
        removeBombThings();
        removeRoundThings(true);
        stopCountdownCamera();
        closeMapVotingMenu();
        clearMapVotingsInBrowser();
        removeRoundInfo();
        stopMapCreator();
        hideRoundEndReason();
    }
});
function mainMenuJoined() {
    mp.game.cam.doScreenFadeIn(100);
    startLobbyChoiceBrowser();
}
mp.events.add("onClientPlayerJoinMapCreatorLobby", () => {
    startMapCreator();
});
let maplimitdata = {
    limit: [],
    outsidecounter: 11,
    checktimer: null,
    minX: 0,
    maxX: 0,
    minY: 0,
    maxY: 0,
    outsidetext: null,
};
function pointIsInPoly(p) {
    if (p.x < maplimitdata.minX || p.x > maplimitdata.maxX || p.y < maplimitdata.minY || p.y > maplimitdata.maxY) {
        return false;
    }
    var inside = false;
    var vs = maplimitdata.limit;
    for (var i = 0, j = vs.length - 1; i < vs.length; j = i++) {
        var xi = vs[i].x, yi = vs[i].y;
        var xj = vs[j].x, yj = vs[j].y;
        var intersect = ((yi > p.y) != (yj > p.y))
            && (p.x < (xj - xi) * (p.y - yi) / (yj - yi) + xi);
        if (intersect)
            inside = !inside;
    }
    return inside;
}
;
function checkMapLimit() {
    if (maplimitdata.limit != null) {
        var pos = mp.players.local.position;
        if (!pointIsInPoly(pos)) {
            maplimitdata.outsidecounter--;
            if (maplimitdata.outsidecounter == 10 && maplimitdata.outsidetext == null)
                maplimitdata.outsidetext = new cText(getLang("round", "outside_map_limit").replace("{1}", maplimitdata.outsidecounter), 0.5, 0.5, 0, [255, 255, 255, 255], [1.2, 1.2], true, Alignment.CENTER, true);
            else if (maplimitdata.outsidecounter > 0)
                maplimitdata.outsidetext.setText(getLang("round", "outside_map_limit").replace("{1}", maplimitdata.outsidecounter));
            else if (maplimitdata.outsidecounter == 0) {
                callRemoteCooldown("onPlayerWasTooLongOutsideMap");
                maplimitdata.checktimer.kill();
                maplimitdata.checktimer = null;
                maplimitdata.outsidetext.remove();
                maplimitdata.outsidetext = null;
            }
        }
        else
            resetMapLimitCheck();
    }
    else
        resetMapLimitCheck();
}
function loadMapLimitData(data) {
    maplimitdata.limit = [];
    for (let j = 0; j < data.length; j++) {
        maplimitdata.limit[j] = { x: data[j].x, y: data[j].y };
    }
    maplimitdata.outsidecounter = 11;
    if (data.length > 0) {
        var minX = maplimitdata.limit[0].x;
        var maxX = maplimitdata.limit[0].x;
        var minY = maplimitdata.limit[0].y;
        var maxY = maplimitdata.limit[0].y;
        for (let i = 1; i < data.length; i++) {
            var q = maplimitdata.limit[i];
            minX = Math.min(q.x, minX);
            maxX = Math.max(q.x, maxX);
            minY = Math.min(q.y, minY);
            maxY = Math.max(q.y, maxY);
        }
        maplimitdata.minX = minX;
        maplimitdata.maxX = maxX;
        maplimitdata.minY = minY;
        maplimitdata.maxY = maxY;
    }
}
function resetMapLimitCheck() {
    if (maplimitdata.outsidetext != null) {
        maplimitdata.outsidetext.remove();
        maplimitdata.outsidetext = null;
    }
    maplimitdata.outsidecounter = 11;
}
function startMapLimit() {
    if (maplimitdata.checktimer != null)
        maplimitdata.checktimer.kill();
    if (0 in maplimitdata.limit) {
        maplimitdata.checktimer = new Timer(checkMapLimit, 1000, -1);
    }
}
function stopMapLimitCheck() {
    if (maplimitdata.checktimer != null) {
        maplimitdata.checktimer.kill();
        maplimitdata.checktimer = null;
    }
    maplimitdata.outsidecounter = 11;
    if (maplimitdata.outsidetext != null) {
        maplimitdata.outsidetext.remove();
        maplimitdata.outsidetext = null;
    }
}
function emptyMapLimit() {
    maplimitdata.limit = [];
}
var mapvotingdata = {
    lastlobbyID: -1,
    lastmapdatas: "",
    inmaplobby: false,
    votings: [],
    visible: false,
    favouritesloaded: false,
    favourites: []
};
function openMapVotingMenu() {
    toggleCursor(true);
    mapvotingdata.visible = true;
    if (lobbysettings.id != mapvotingdata.lastlobbyID) {
        callRemoteCooldown("onMapsListRequest");
        if (!mapvotingdata.favouritesloaded) {
            let favouritesstr = mp.storage.data.mapfavourites;
            if (favouritesstr != undefined) {
                mapvotingdata.favourites = JSON.parse(favouritesstr);
                loadMapFavouritesInBrowser(favouritesstr);
            }
            else
                loadMapFavouritesInBrowser("[]");
            mapvotingdata.favouritesloaded = true;
        }
    }
    else
        openMapMenuInBrowser(mapvotingdata.lastmapdatas);
}
function closeMapVotingMenu() {
    if (mapvotingdata.visible) {
        closeMapMenuInBrowser();
        mapvotingdata.visible = false;
        toggleCursor(false);
    }
}
mp.events.add("closeMapVotingMenu", closeMapVotingMenu);
mp.keys.bind(Keys.M, false, () => {
    if (!mapvotingdata.visible) {
        if (!ischatopen)
            if (mapvotingdata.inmaplobby)
                openMapVotingMenu();
    }
    else
        closeMapVotingMenu();
});
mp.events.add("onClientMapsListRequest", (mapdatasjson) => {
    mapvotingdata.lastmapdatas = mapdatasjson;
    openMapMenuInBrowser(mapdatasjson);
});
mp.events.add("onMapMenuVote", (mapname) => {
    callRemoteCooldown("onMapVotingRequest", mapname);
});
mp.events.add("onClientToggleMapFavourite", (mapname, bool) => {
    if (bool)
        mapvotingdata.favourites.push(mapname);
    else {
        for (let i = 0; i < mapvotingdata.favourites.length; ++i) {
            if (mapvotingdata.favourites[i] == mapname)
                mapvotingdata.favourites.splice(i, 1);
        }
    }
    mp.storage.data.mapfavourites = JSON.stringify(mapvotingdata.favourites);
});
mp.events.add("onAddVoteToMap", (mapname, oldmapname) => {
    addVoteToMapInMapMenuBrowser(mapname, oldmapname);
    let foundmap = false;
    for (let i = 0; i < mapvotingdata.votings.length && !foundmap; ++i) {
        if (mapvotingdata.votings[i].name === mapname) {
            ++mapvotingdata.votings[i].votes;
            foundmap = true;
        }
    }
    if (!foundmap)
        mapvotingdata.votings.push({ name: mapname, votes: 1 });
    if (typeof oldmapname !== "undefined") {
        for (let i = 0; i < mapvotingdata.votings.length; ++i) {
            if (mapvotingdata.votings[i].name === oldmapname) {
                if (--mapvotingdata.votings[i].votes <= 0)
                    mapvotingdata.votings.splice(i, 1);
                break;
            }
        }
    }
});
mp.events.add("onMapVotingSyncOnJoin", (mapsvotesjson) => {
    let mapsvotesdict = JSON.parse(mapsvotesjson);
    for (let key in mapsvotesdict) {
        mapvotingdata.votings.push({ name: key, votes: mapsvotesdict[key] });
    }
    loadMapVotingsForMapBrowser(mapsvotesjson);
});
let lobbyplayerdata = {
    playerssamelobby: [],
    playerssamelobbynames: []
};
mp.events.add(ECustomRemoteEvents.SyncPlayersSameLobby, (players) => {
    mp.gui.chat.push(players);
    lobbyplayerdata.playerssamelobby = JSON.parse(players);
    lobbyplayerdata.playerssamelobbynames = [];
    for (let i = 0; i < lobbyplayerdata.playerssamelobby.length; ++i) {
        lobbyplayerdata.playerssamelobbynames.push(lobbyplayerdata.playerssamelobby[i].name);
    }
    mainbrowserdata.browser.execute("loadNamesForChat(`" + JSON.stringify(lobbyplayerdata.playerssamelobbynames) + "`);");
});
mp.events.add(ECustomRemoteEvents.ClientPlayerJoinSameLobby, (player) => {
    if (player != mp.players.local) {
        lobbyplayerdata.playerssamelobby.push(player);
        lobbyplayerdata.playerssamelobbynames.push(player.name);
        mainbrowserdata.browser.execute("addNameForChat('" + player.name + "');");
    }
});
mp.events.add(ECustomRemoteEvents.ClientPlayerLeaveSameLobby, (player) => {
    if (player != mp.players.local) {
        let index = lobbyplayerdata.playerssamelobby.indexOf(player);
        if (index != -1) {
            lobbyplayerdata.playerssamelobby.splice(index, 1);
            lobbyplayerdata.playerssamelobbynames.splice(index, 1);
            mainbrowserdata.browser.execute("removeNameForChat(" + index + ");");
        }
    }
});
var rounddata = {
    mapinfo: null,
    isspectator: true,
    infight: false,
    currentMap: ""
};
function setMapInfo(mapname) {
    rounddata.currentMap = mapname;
    if (rounddata.mapinfo == null)
        rounddata.mapinfo = new cText(mapname, 0.5, 0.95, 0, [255, 255, 255, 255], [0.5, 0.5], true, Alignment.CENTER, true);
    else
        rounddata.mapinfo.setText(mapname);
}
function removeMapInfo() {
    log("removeMapInfo");
    if (rounddata.mapinfo != null) {
        rounddata.mapinfo.remove();
        rounddata.mapinfo = null;
    }
}
function removeRoundThings(removemapinfo) {
    log("removeRoundThings");
    stopSpectate();
    stopCountdown();
    if (removemapinfo) {
        removeMapInfo();
    }
}
function toggleFightMode(bool) {
    log("toggleFightMode " + bool);
    if (bool) {
        rounddata.infight = true;
    }
    else {
        rounddata.infight = false;
        stopMapLimitCheck();
    }
}
mp.events.add("onClientMapChange", function (mapname, maplimit, mapmidx, mapmidy, mapmidz) {
    log("onClientMapChange");
    setMapInfo(mapname);
    hideRoundEndReason();
    mp.game.cam.doScreenFadeIn(lobbysettings.roundendtime / 2);
    maplimit = JSON.parse(maplimit);
    if (maplimit.length > 0)
        loadMapLimitData(maplimit);
    setCameraToMapCenter(new mp.Vector3(mapmidx, mapmidy, mapmidz));
    toggleFightControls(false);
});
mp.events.add("onClientCountdownStart", function (resttime) {
    log("onClientCountdownStart");
    if (cameradata.timer != null) {
        cameradata.timer.kill();
        cameradata.timer = null;
    }
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
    hideRoundEndReason();
    toggleFightControls(false);
});
mp.events.add("onClientRoundStart", function (isspectator, wastedticks) {
    log("onClientRoundStart");
    mp.game.cam.doScreenFadeIn(50);
    stopCountdownCamera();
    endCountdown();
    rounddata.isspectator = isspectator == 1;
    if (!rounddata.isspectator) {
        startMapLimit();
        toggleFightMode(true);
    }
    roundStartedRoundInfo(wastedticks);
    toggleFightControls(true);
    damagesysdata.shotsdoneinround = 0;
});
mp.events.add("onClientRoundEnd", function (reason) {
    log("onClientRoundEnd");
    mp.game.cam.doScreenFadeOut(lobbysettings.roundendtime / 2);
    toggleFightMode(false);
    removeBombThings();
    emptyMapLimit();
    removeRoundThings(false);
    stopCountdown();
    stopCountdownCamera();
    removeRoundInfo();
    toggleFightControls(false);
    clearMapVotingsInBrowser();
    showRoundEndReason(reason, rounddata.currentMap);
});
mp.events.add("onClientPlayerSpectateMode", function () {
    log("onClientPlayerSpectateMode");
    rounddata.isspectator = true;
    startSpectate();
});
mp.events.add(ECustomRemoteEvents.ClientPlayerDeath, function (playerID, teamID, killstr) {
    log("onClientPlayerDeath");
    let player = mp.players.atRemoteId(playerID);
    if (mp.players.local == player) {
        toggleFightMode(false);
        removeBombThings();
    }
    playerDeathRoundInfo(teamID, killstr);
});
mp.events.add("PlayerQuit", function (player, exitType, reason) {
    log("PlayerQuit");
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
mp.events.add("onClientPlayerTeamChange", function (teamID, teamUID) {
    setVoiceChatRoom(teamUID);
});
let roundinfo = {
    amountinteams: [],
    aliveinteams: [],
    roundtime: 0,
    starttick: 0,
    teamnames: [],
    teamcolors: [],
    drawclasses: {
        text: {
            time: null,
            teams: []
        },
        rect: {
            time: null,
            teams: []
        }
    },
    drawdata: {
        time: {
            text: {
                ypos: 0.005,
                scale: [0.5, 0.5],
                color: [255, 255, 255, 255]
            },
            rectangle: {
                xpos: 0.47,
                ypos: 0,
                width: 0.06,
                height: 0.05,
                color: [20, 20, 20, 180]
            }
        },
        team: {
            text: {
                ypos: -0.002,
                scale: [0.41, 0.41],
                color: [255, 255, 255, 255]
            },
            rectangle: {
                ypos: 0,
                width: 0.13,
                height: 0.06,
                a: 180
            }
        },
        kills: {
            showtick: 15000,
            fadeaftertick: 11000,
            xpos: 0.99,
            ypos: 0.45,
            scale: [0.3, 0.3],
            height: 0.04
        }
    }
};
function refreshRoundInfo() {
    let tick = getTick();
    let fullseconds = Math.ceil((roundinfo.roundtime - (tick - roundinfo.starttick)) / 1000);
    let minutes = Math.floor(fullseconds / 60);
    let seconds = fullseconds % 60;
    roundinfo.drawclasses.text.time.setText(minutes + ":" + (seconds >= 10 ? seconds : "0" + seconds));
}
function setRoundTimeLeft(lefttime) {
    roundinfo.starttick = getTick() - (roundinfo.roundtime - lefttime);
}
function removeRoundInfo() {
    roundinfo.amountinteams = [];
    roundinfo.aliveinteams = [];
    mp.events.remove("render", refreshRoundInfo);
    if (roundinfo.drawclasses.rect.time != null) {
        roundinfo.drawclasses.rect.time.remove();
        roundinfo.drawclasses.text.time.remove();
        for (let i = 0; i < roundinfo.drawclasses.text.teams.length; ++i) {
            roundinfo.drawclasses.text.teams[i].remove();
            roundinfo.drawclasses.rect.teams[i].remove();
        }
        roundinfo.drawclasses.text.time = null;
        roundinfo.drawclasses.rect.time = null;
        roundinfo.drawclasses.text.teams = [];
        roundinfo.drawclasses.rect.teams = [];
    }
}
function roundStartedRoundInfo(wastedticks) {
    roundinfo.starttick = getTick();
    if (wastedticks != null)
        roundinfo.starttick -= wastedticks;
    let tdata = roundinfo.drawdata.time.text;
    let trdata = roundinfo.drawdata.time.rectangle;
    roundinfo.drawclasses.rect.time = new cRectangle(trdata.xpos, trdata.ypos, trdata.width, trdata.height, trdata.color);
    roundinfo.drawclasses.text.time = new cText("0:00", trdata.xpos + trdata.width / 2, tdata.ypos, 0, tdata.color, tdata.scale, true, Alignment.CENTER, true);
    let teamdata = roundinfo.drawdata.team.text;
    let teamrdata = roundinfo.drawdata.team.rectangle;
    let leftteamamount = Math.ceil(roundinfo.teamnames.length / 2);
    for (let i = 0; i < leftteamamount; i++) {
        let startx = trdata.xpos - teamrdata.width * (i + 1);
        roundinfo.drawclasses.rect.teams[i] = new cRectangle(startx, teamrdata.ypos, teamrdata.width, teamrdata.height, [roundinfo.teamcolors[i].Red, roundinfo.teamcolors[i].Green, roundinfo.teamcolors[i].Blue, teamrdata.a]);
        roundinfo.drawclasses.text.teams[i] = new cText(roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i], startx + teamrdata.width / 2, teamdata.ypos, 0, teamdata.color, teamdata.scale, true, Alignment.CENTER, true);
    }
    for (let j = 0; j < roundinfo.teamnames.length - leftteamamount; j++) {
        let startx = trdata.xpos + trdata.width + teamrdata.width * j;
        let i = leftteamamount + j;
        roundinfo.drawclasses.rect.teams[i] = new cRectangle(startx, teamrdata.ypos, teamrdata.width, teamrdata.height, [roundinfo.teamcolors[i].Red, roundinfo.teamcolors[i].Green, roundinfo.teamcolors[i].Blue, teamrdata.a]);
        roundinfo.drawclasses.text.teams[i] = new cText(roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i], startx + teamrdata.width / 2, teamdata.ypos, 0, teamdata.color, teamdata.scale, true, Alignment.CENTER, true);
    }
    mp.events.add("render", refreshRoundInfo);
}
function addTeamInfos(teamnames, teamcolors) {
    for (let i = 1; i < teamnames.length; i++) {
        roundinfo.teamnames[i - 1] = teamnames[i];
    }
    for (let i = 1; i < teamcolors.length; i++) {
        roundinfo.teamcolors[i - 1] = teamcolors[i];
    }
}
function refreshRoundInfoTeamData() {
    for (let i = 0; i < roundinfo.drawclasses.text.teams.length; ++i) {
        roundinfo.drawclasses.text.teams[i].setText(roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i]);
    }
}
function playerDeathRoundInfo(teamID, killstr) {
    roundinfo.aliveinteams[teamID - 1]--;
    roundinfo.drawclasses.text.teams[teamID - 1].setText(roundinfo.teamnames[teamID - 1] + "\n" + roundinfo.aliveinteams[teamID - 1] + "/" + roundinfo.amountinteams[teamID - 1]);
    addKillMessage(killstr);
}
mp.events.add("onClientPlayerAmountInFightSync", (amountinteam, isroundstarted, amountaliveinteam) => {
    log("onClientPlayerAmountInFightSync");
    roundinfo.amountinteams = [];
    roundinfo.aliveinteams = [];
    amountinteam = JSON.parse(amountinteam);
    if (isroundstarted == 1)
        amountaliveinteam = JSON.parse(amountaliveinteam);
    for (let i = 0; i < amountinteam.length; i++) {
        roundinfo.amountinteams[i] = Number.parseInt(amountinteam[i]);
        if (isroundstarted == 0)
            roundinfo.aliveinteams[i] = Number.parseInt(amountinteam[i]);
        else
            roundinfo.aliveinteams[i] = Number.parseInt(amountaliveinteam[i]);
    }
    refreshRoundInfoTeamData();
});
var lobbysettings = {
    id: -1,
    countdowntime: 0,
    bombdetonatetime: 0,
    bombplanttime: 0,
    bombdefusetime: 0,
    roundendtime: 0
};
let spectatedata = {
    binded: false
};
function pressSpectateKeyLeft() {
    callRemoteCooldown("spectateNext", false);
}
function pressSpectateKeyRight() {
    callRemoteCooldown("spectateNext", true);
}
function startSpectate() {
    if (!spectatedata.binded) {
        mp.keys.bind(Keys.LeftArrow, false, pressSpectateKeyLeft);
        mp.keys.bind(Keys.A, false, pressSpectateKeyLeft);
        mp.keys.bind(Keys.RightArrow, false, pressSpectateKeyRight);
        mp.keys.bind(Keys.D, false, pressSpectateKeyRight);
        spectatedata.binded = true;
    }
}
function stopSpectate() {
    if (spectatedata.binded) {
        mp.keys.unbind(Keys.LeftArrow, false, pressSpectateKeyLeft);
        mp.keys.unbind(Keys.A, false, pressSpectateKeyLeft);
        mp.keys.unbind(Keys.RightArrow, false, pressSpectateKeyRight);
        mp.keys.unbind(Keys.D, false, pressSpectateKeyRight);
        spectatedata.binded = false;
    }
}
let mapcreatordata = {
    browser: null
};
function startMapCreator() {
    if (mapcreatordata.browser !== null)
        return;
    mapcreatordata.browser = mp.browsers.new("package://TDS-V/window/mapcreator/index.html");
}
function stopMapCreator() {
    if (mapcreatordata.browser === null)
        return;
    mapcreatordata.browser.destroy();
    mapcreatordata.browser = null;
}
mp.events.add(ECustomBrowserRemoteEvents.RequestCurrentPositionForMapCreator, () => {
    if (mapcreatordata.browser === null)
        return;
    let position = localPlayer.position;
    let x = Math.round(position.x * 100) / 100;
    let y = Math.round(position.y * 100) / 100;
    let z = Math.round(position.z * 100) / 100;
    let rotation = Math.round(localPlayer.getRotation(2).z * 100) / 100;
    mapcreatordata.browser.execute("loadPositionFromClient (" + x + ", " + y + ", " + z + ", " + rotation + "); ");
});
mp.events.add(ECustomBrowserRemoteEvents.GotoPositionByMapCreator, (x, y, z, rot) => {
    if (mapcreatordata.browser === null)
        return;
    localPlayer.position = { x: x, y: y, z: z };
    if (!isNaN(rot))
        localPlayer.setRotation(0.0, 0.0, rot, 2, true);
});
mp.events.add("CheckMapName", (name) => {
    callRemoteCooldown("checkMapName", name);
});
mp.events.add("SendMapNameCheckResult", (alreadyinuse) => {
    mapcreatordata.browser.execute("loadResultOfMapNameCheck(" + alreadyinuse + ");");
});
mp.events.add("SendMapFromCreator", (mapjson) => {
    callRemoteCooldown("sendMapFromCreator", mapjson);
});
let loginpanel = {
    loginbrowser: null,
    name: null,
    isregistered: null
};
mp.events.add(ECustomBrowserRemoteEvents.LoginFunc, function (password) {
    callRemoteCooldown("onPlayerTryLogin", password);
});
mp.events.add(ECustomBrowserRemoteEvents.RegisterFunc, function (password, email) {
    callRemoteCooldown("onPlayerTryRegister", password, email);
});
mp.events.add(ECustomBrowserRemoteEvents.GetRegisterLoginLanguage, () => {
    loginpanel.loginbrowser.execute("loadLanguage ( `" + JSON.stringify(getLang("loginregister")) + "` );");
});
mp.events.add(ECustomRemoteEvents.StartRegisterLogin, function (name, isregistered) {
    log("startRegisterLogin registerlogin");
    loginpanel.name = name;
    loginpanel.isregistered = isregistered;
    loginpanel.loginbrowser = mp.browsers.new("package://TDS-V/window/registerlogin/index.html");
    mp.gui.chat.activate(false);
    toggleCursor(true);
});
mp.events.add(ECustomRemoteEvents.RegisterLoginSuccessful, function () {
    log("registerLoginSuccessful registerlogin");
    loginpanel.loginbrowser.destroy();
    loginpanel.loginbrowser = null;
    mp.gui.chat.activate(true);
    --nothidecursor;
});
var reportsdata = {
    inreportsmenu: false,
    inreport: false,
};
mp.events.add("syncReports", (reports) => {
    mainbrowserdata.angular.call("syncReports(`" + reports + "`);");
});
mp.events.add("syncReportText", (reporttext) => {
    mainbrowserdata.angular.call("syncReportText(`" + reporttext + "`);");
});
mp.events.add("syncReportTexts", (reporttexts) => {
    mainbrowserdata.angular.call("syncReportTexts(`" + reporttexts + "`);");
});
mp.events.add("syncReportState", (reportid, state) => {
    mainbrowserdata.angular.call(`syncReportState(${reportid}, ${state});`);
});
mp.events.add("syncReport", (report) => {
    mainbrowserdata.angular.call(`syncReport('${report}');`);
});
mp.events.add("syncReportRemove", (reportid) => {
    mainbrowserdata.angular.call(`syncReportRemove (${reportid});`);
});
function addTextToReport(reportid, text) {
    mp.events.callRemote("onClientAddTextToReport", reportid, text);
}
function createReport(title, text, forminadminlvl) {
    mp.events.callRemote("onClientCreateReport", title, text, forminadminlvl);
}
function openReportsMenu() {
    reportsdata.inreportsmenu = true;
    mp.events.callRemote("onClientOpenReportsMenu");
}
function closeReportsMenu() {
    reportsdata.inreport = false;
    mp.events.callRemote("onPlayerCloseReportsMenu");
}
function openReport(index) {
    reportsdata.inreport = true;
    mp.events.callRemote("onClientOpenReport", index);
}
function closeReport() {
    reportsdata.inreport = false;
    mp.events.callRemote("onClientCloseReport");
}
function toggleReportState(reportid, state) {
    mp.events.callRemote("onClientChangeReportState", reportid, state == 1);
}
function removeReport(reportid) {
    mp.events.callRemote("onClientRemoveReport");
}
var suggestionsdata = {
    insuggestionsmenu: false,
    insuggestion: false,
};
mp.events.add("syncSuggestions", (suggestions) => {
    mainbrowserdata.angular.call("syncSuggestions(`" + suggestions + "`);");
});
mp.events.add("syncSuggestionText", (suggestiontext) => {
    mainbrowserdata.angular.call("syncSuggestionText(`" + suggestiontext + "`);");
});
mp.events.add("syncSuggestionTexts", (suggestiontexts) => {
    mainbrowserdata.angular.call("syncSuggestionTexts(`" + suggestiontexts + "`);");
});
mp.events.add("syncSuggestionState", (suggestionid, state) => {
    mainbrowserdata.angular.call(`syncSuggestionState(${suggestionid}, ${state});`);
});
mp.events.add("syncSuggestion", (suggestion) => {
    mainbrowserdata.angular.call(`syncSuggestion('${suggestion}');`);
});
mp.events.add("syncSuggestionRemove", (suggestionid) => {
    mainbrowserdata.angular.call(`syncSuggestionRemove (${suggestionid});`);
});
mp.events.add("syncSuggestionVotes", (suggestionvotes) => {
    mainbrowserdata.angular.call(`syncSuggestionVotes(${suggestionvotes});`);
});
mp.events.add("syncSuggestionVote", (name, vote) => {
    mainbrowserdata.angular.call(`syncSuggestionVote('${name}', ${vote});`);
});
function addTextToSuggestion(suggestionid, text) {
    mp.events.callRemote("onClientAddTextToSuggestion", suggestionid, text);
}
function createSuggestion(title, text, topic) {
    mp.events.callRemote("onClientCreateSuggestion", title, text, topic);
}
function openSuggestionsMenu() {
    suggestionsdata.insuggestionsmenu = true;
    mp.events.callRemote("onClientOpenSuggestionsMenu");
}
function closeSuggestionsMenu() {
    suggestionsdata.insuggestion = false;
    mp.events.callRemote("onPlayerCloseSuggestionsMenu");
}
function openSuggestion(index) {
    suggestionsdata.insuggestion = true;
    mp.events.callRemote("onClientOpenSuggestion", index);
}
function closeSuggestion() {
    suggestionsdata.insuggestion = false;
    mp.events.callRemote("onClientCloseSuggestion");
}
function changeSuggestionState(suggestionid, state) {
    mp.events.callRemote("onClientChangeSuggestionState", suggestionid, state);
}
function removeSuggestion(suggestionid) {
    mp.events.callRemote("onClientRemoveSuggestion");
}
function requestSuggestionsByState(state) {
    mp.events.callRemote("onClientRequestSuggestionsByState", state);
}
function toggleSuggestionVote(suggestionid, vote) {
    mp.events.callRemote("onClientToggleSuggestionVote", suggestionid, vote);
}
let userpaneldata = {
    open: false
};
function openUserpanel() {
    mainbrowserdata.angular.call("openUserpanel();");
    toggleCursor(true);
    userpaneldata.open = true;
}
function closeUserpanel() {
    mainbrowserdata.angular.call("closeUserpanel();");
    if (reportsdata.inreport) {
        reportsdata.inreport = false;
        mp.events.callRemote("onPlayerCloseReport");
    }
    if (reportsdata.inreportsmenu) {
        reportsdata.inreportsmenu = false;
        mp.events.callRemote("onPlayerCloseReportsMenu");
    }
    toggleCursor(false);
    userpaneldata.open = false;
}
function addUserpanelFunctionsToAngular() {
    mainbrowserdata.angular.listen("closeUserpanel", closeUserpanel);
    mainbrowserdata.angular.listen("requestLanguage", getLanguage);
    mainbrowserdata.angular.listen("createReport", createReport);
    mainbrowserdata.angular.listen("openReportsMenu", openReportsMenu);
    mainbrowserdata.angular.listen("closeReportsMenu", closeReportsMenu);
    mainbrowserdata.angular.listen("openReport", openReport);
    mainbrowserdata.angular.listen("closeReport", closeReport);
    mainbrowserdata.angular.listen("toggleReportState", toggleReportState);
    mainbrowserdata.angular.listen("removeReport", removeReport);
    mainbrowserdata.angular.listen("addTextToReport", addTextToReport);
    mainbrowserdata.angular.listen("createSuggestion", createSuggestion);
    mainbrowserdata.angular.listen("openSuggestionsMenu", openSuggestionsMenu);
    mainbrowserdata.angular.listen("closeSuggestionsMenu", closeSuggestionsMenu);
    mainbrowserdata.angular.listen("openSuggestion", openSuggestion);
    mainbrowserdata.angular.listen("closeSuggestion", closeSuggestion);
    mainbrowserdata.angular.listen("changeSuggestionState", changeSuggestionState);
    mainbrowserdata.angular.listen("removeSuggestion", removeSuggestion);
    mainbrowserdata.angular.listen("addTextToSuggestion", addTextToSuggestion);
    mainbrowserdata.angular.listen("requestSuggestionsByState", requestSuggestionsByState);
    mainbrowserdata.angular.listen("toggleSuggestionVote", toggleSuggestionVote);
}
mp.keys.bind(Keys.U, false, () => {
    if (ischatopen)
        return;
    if (!userpaneldata.open)
        openUserpanel();
    else
        closeUserpanel();
});
