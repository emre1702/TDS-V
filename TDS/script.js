"use strict";
var res = mp.game.graphics.getScreenActiveResolution(0, 0);
var nothidecursor = 0;
var currentmoney = null;
var localPlayer = mp.players.local;
var gameplayCam = mp.cameras.new("gameplay");
let activatedlogging = true;
function log(message) {
    if (activatedlogging) {
        mp.gui.chat.push(message);
    }
}
mp.events.add("onClientMoneyChange", money => {
    log("onClientMoneyChange start");
    currentmoney = money;
    log("onClientMoneyChange end");
});
mp.events.add("render", () => {
    if (currentmoney != null) {
        mp.game.graphics.drawText("$" + currentmoney, 7, [115, 186, 131, 255], 1.0, 1.0, true, res.x - 90, 50);
    }
});
var alltimertable = [];
var puttimerintable = [];
mp.events.add("render", function () {
    var tick = getTick();
    for (let i = alltimertable.length - 1; i >= 0; i--)
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
        for (var j = 0; j < puttimerintable.length; j++) {
            puttimerintable[j].putTimerInSorted();
        }
        puttimerintable = [];
    }
});
class Timer {
    constructor(func, executeafterms, executeamount, ...args) {
        this.func = func;
        this.executeatms = executeafterms + getTick();
        this.executeafterms = executeafterms;
        this.executeamountleft = executeamount;
        this.args = args;
        this.killit = false;
        puttimerintable[puttimerintable.length] = this;
        return this;
    }
    kill() {
        this.killit = true;
    }
    execute(notremove) {
        log("timer execute start");
        var argslength = this.args.length;
        switch (argslength) {
            case 0:
                this.func();
                break;
            case 1:
                this.func(this.args[0]);
                break;
            case 2:
                this.func(this.args[0], this.args[1]);
                break;
            case 3:
                this.func(this.args[0], this.args[1], this.args[2]);
                break;
            case 4:
                this.func(this.args[0], this.args[1], this.args[2], this.args[3]);
                break;
            case 5:
                this.func(this.args[0], this.args[1], this.args[2], this.args[3], this.args[4]);
                break;
            case 6:
                this.func(this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5]);
                break;
            case 7:
                this.func(this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6]);
                break;
            case 8:
                this.func(this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6], this.args[7]);
                break;
            case 9:
                this.func(this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6], this.args[7], this.args[8]);
                break;
            case 10:
                this.func(this.args[0], this.args[1], this.args[2], this.args[3], this.args[4], this.args[5], this.args[6], this.args[7], this.args[8], this.args[9]);
                break;
        }
        if (notremove == null) {
            var index = alltimertable.indexOf(this);
            alltimertable.splice(index, 1);
        }
        this.executeamountleft--;
        if (this.executeamountleft !== 0) {
            this.executeatms += this.executeafterms;
            this.putTimerInSorted();
        }
        log("timer execute end");
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
mp.keys.bind(0x23, true, function (sender, e) {
    if (mp.gui.cursor.visible) {
        mp.gui.cursor.visible = false;
        nothidecursor = 0;
    }
    else {
        mp.gui.cursor.visible = true;
        nothidecursor = 1;
    }
});
function getPlayerByName(name) {
    mp.players.forEach((player, index) => {
        if (player.name == name)
            return player;
    });
    return null;
}
function distance(vector1, vector2, useZ = true) {
    return mp.game.gameplay.getDistanceBetweenCoords(vector1.x, vector1.y, vector1.z, vector2.x, vector2.y, vector2.z, useZ);
}
let damagesysdata = {
    lasthparmor: 0,
};
mp.events.add("playerSpawn", (player) => {
    if (player == localPlayer) {
        mp.game.cam.doScreenFadeIn(2000);
    }
});
mp.events.add("playerDeath", (player, reason, killer) => {
    if (player == localPlayer) {
        mp.game.cam.doScreenFadeOut(2000);
        mp.game.gameplay.ignoreNextRestart(true);
        mp.game.gameplay.setFadeOutAfterDeath(false);
    }
});
let drawdrawings = [];
mp.events.add("render", () => {
    let tick = getTick();
    for (var i = 0; i < drawdrawings.length; i++) {
        let c = drawdrawings[i];
        if (c.activated) {
            switch (c.type) {
                case "rectangle":
                    mp.game.graphics.drawRect(c.x, c.y, c.width, c.height, c.r, c.g, c.b, c.a);
                    break;
                case "text":
                    let alpha = c.a;
                    if (c.enda != null) {
                        alpha = getBlendValue(tick, c.a, c.enda, c.endastarttick, c.endaendtick);
                    }
                    let scale = c.scale;
                    if (c.endscale != null) {
                        scale = getBlendValue(tick, c.scale, c.endscale, c.endscalestarttick, c.endscaleendtick);
                    }
                    mp.game.graphics.drawText(c.text, c.font, c.color, c.scaleX, c.scaleY, c.outline, c.x, c.y);
                    break;
            }
        }
    }
});
function getBlendValue(tick, start, end, starttick, endtick) {
    let progress = (tick - starttick) / (endtick - starttick);
    if (progress > 1)
        progress = 1;
    return start + progress * (end - start);
}
function removeClassDraw() {
    let index = drawdrawings.indexOf(this);
    if (index != -1) {
        drawdrawings.splice(index, 1);
        return true;
    }
    else
        return false;
}
function blendClassDrawTextAlpha(enda, mstime) {
    this.enda = enda;
    this.endastarttick = getTick();
    this.endaendtick = this.endastarttick + mstime;
}
function blendClassDrawTextScale(endscale, mstime) {
    this.endscale = endscale;
    this.endscalestarttick = getTick();
    this.endscaleendtick = this.endscalestarttick + mstime;
}
function getClassDrawText() {
    return this.text;
}
function setClassDrawText(text) {
    this.text = text;
}
function cLine(start, end, r, g, b, a) {
    this.type = "line";
    this.activated = true;
    this.start = start;
    this.end = end;
    this.r = r;
    this.g = g;
    this.b = b;
    this.a = a;
    this.remove = removeClassDraw;
    drawdrawings.push(this);
}
function cRectangle(xpos, ypos, wsize, hsize, r, g, b, a) {
    this.type = "rectangle";
    this.activated = true;
    this.x = xpos;
    this.y = ypos;
    this.width = wsize;
    this.height = hsize;
    this.r = r;
    this.g = g;
    this.b = b;
    this.a = a;
    this.remove = removeClassDraw;
    drawdrawings.push(this);
}
function cText(text, x, y, fontid, color, scaleX, scaleY, outline) {
    this.type = "text";
    this.activated = true;
    this.text = text;
    this.x = x;
    this.y = y;
    this.color = color;
    this.scaleX = scaleX;
    this.scaleY = scaleY;
    this.outline = outline;
    this.remove = removeClassDraw;
    this.blendTextAlpha = blendClassDrawTextAlpha;
    this.blendTextScale = blendClassDrawTextScale;
    this.getText = getClassDrawText;
    this.setText = setClassDrawText;
    drawdrawings.push(this);
}
function cEditBox(defaulttext, xpos, ypos, wsize, hsize, r = 20, g = 20, b = 20, a = 187, scale = 1.0, textr = 255, textg = 255, textb = 255, texta = 255, font = 0, justify = 0, shadow = false, outline = false, wordwrap = 0) {
    this.type = "editbox";
    this.activated = true;
    this.text = defaulttext;
    this.x = xpos;
    this.y = ypos;
    this.width = wsize;
    this.height = hsize;
    this.r = r;
    this.g = g;
    this.b = b;
    this.scale = scale;
    this.textr = textr;
    this.textg = textg;
    this.textb = textb;
    this.texta = texta;
    this.font = font;
    this.justify = justify;
    this.shadow = shadow;
    this.outline = outline;
    this.wordwrap = wordwrap == 0 ? Math.floor(wsize) : wordwrap;
    this.remove = removeClassDraw;
    drawdrawings.push(this);
}
var Keys;
(function (Keys) {
    Keys[Keys["LeftArrow"] = 37] = "LeftArrow";
    Keys[Keys["UpArrow"] = 38] = "UpArrow";
    Keys[Keys["RightArrow"] = 39] = "RightArrow";
    Keys[Keys["DownArrow"] = 40] = "DownArrow";
    Keys[Keys["A"] = 65] = "A";
    Keys[Keys["D"] = 68] = "D";
})(Keys || (Keys = {}));
var languagelist = {
    "german": {
        "loginregister": {
            "tab_login": "Login",
            "tab_register": "Register",
            "username": "Benutzername",
            "password": "Passwort",
            "login_titel": "Willkommen zurueck!",
            "login_forgotpw": "Passwort vergessen?",
            "login_button": "Einloggen",
            "register_titel": "Registriere dich auf TDS",
            "register_email": "Email-Adresse für Passwort-Reset",
            "register_confirmpw": "Passwort bestaetigen",
            "register_button": "Abschicken",
            "forgotpw_titel": "Setze dein Passwort zurueck!",
            "forgotpw_email": "Email-Adresse",
            "forgotpw_reset": "Zuruecksetzen",
            "passwordhastobesame": "Beide Passwoerter muessen die selben sein!"
        },
        "round": {
            "outside_map_limit": "Du bist außerhalb der Map-Grenze!\nDir bleiben noch {1} Sekunden, um zurück zu gehen.",
            "planting": "Lege Bombe ...",
            "defusing": "Entschärfe ..."
        },
        "lobby_choice": {
            "arena": "Arena",
            "gang": "Gang",
            "custom": "Eigenes",
            "player": "Spieler",
            "spectator": "Zuschauer",
            "back": "Zurück",
            "create": "Erstellen",
            "custom_lobby_own": "Eigene Lobby",
            "lobby_name": "Lobby-Name",
            "bomb": "Bombe",
            "lobby_password": "Lobby-Passwort",
            "max_players": "max. Spieler",
            "round_time": "Runden-Zeit (Sekunden)",
            "countdown_time": "Countdown-Zeit (Sekunden)",
            "armor": "Weste",
            "health": "HP",
            "time_scale": "Zeit-Tempo",
        },
        "scoreboard": {
            "name": "Name",
            "playtime": "Spielzeit",
            "kills": "Kills",
            "assists": "Assists",
            "deaths": "Tode",
            "team": "Team",
            "lobby": "Lobby"
        },
        "mapcreator_menu": {
            "send_text": "Senden",
            "send_description": "Sende die Map zum Server.",
            "spawnpoint_text": "Spawn-Punkt",
            "spawnpoint_description": "Füge Spawn-Punkte hinzu oder entferne sie!",
            "team_text": "Team-Nummer",
            "team_description": "Setze die Team-Nummer.",
            "spawnpoint_add_text": "Spawnpunkt hinzufügen",
            "spawnpoint_add_description": "Fügt einen Spawnpunkt hinzu.",
            "maplimit_text": "Map-Limit",
            "maplimit_description": "Setze die Ecken des Map-Limits (optional).",
            "maplimit_add_text": "Ecke hinzufügen",
            "maplimit_add_description": "Fügt eine Ecke für die Map-Begrenzung hinzu.",
        }
    },
    "english": {
        "loginregister": {
            "tab_login": "Login",
            "tab_register": "Register",
            "username": "username",
            "password": "password",
            "login_titel": "Welcome back!",
            "login_forgotpw": "Password forgotten?",
            "login_button": "login",
            "register_titel": "Register at TDS!",
            "register_email": "Email-address for password-reset",
            "register_confirmpw": "confirm password",
            "register_button": "Send",
            "forgotpw_titel": "reset your password",
            "forgotpw_email": "Email-address",
            "forgotpw_reset": "reset",
            "passwordhastobesame": "Both passwords have to be the same!"
        },
        "round": {
            "outside_map_limit": "You are outside of the map!\nThere are {1} seconds left to return to the map.",
            "planting": "planting ...",
            "defusing": "defusing ..."
        },
        "lobby_choice": {
            "arena": "Arena",
            "gang": "Gang",
            "custom": "Custom",
            "player": "player",
            "spectator": "spectator",
            "back": "back",
            "create": "create",
            "lobby_name": "lobby-name",
            "bomb": "bomb",
            "lobby_password": "lobby-password",
            "max_players": "max. players",
            "round_time": "round-time (seconds)",
            "countdown_time": "countdown-time (seconds)",
            "armor": "armor",
            "health": "health",
            "time_scale": "time-scale",
        },
        "scoreboard": {
            "name": "name",
            "playtime": "playtime",
            "kills": "kills",
            "assists": "assists",
            "deaths": "deaths",
            "team": "team",
            "lobby": "lobby",
            "custom_lobby_own": "own lobby"
        },
        "mapcreator_menu": {
            "send_text": "send",
            "send_description": "Sends your map.",
            "spawnpoint_text": "spawn-point",
            "spawnpoint_description": "Add spawn-points or remove them!",
            "team_text": "team-number",
            "team_description": "Sets the team-number.",
            "spawnpoint_add_text": "add spawn-point",
            "spawnpoint_add_description": "Adds a spawn-point.",
            "maplimit_text": "map-limit",
            "maplimit_description": "Sets the corners for the map-limit (optional).",
            "maplimit_add_text": "add corner",
            "maplimit_add_description": "Adds a corner for the map-limit."
        }
    }
};
let languagesetting = "english";
function getLang(type, str = null) {
    if (str != null)
        return languagelist[languagesetting][type][str];
    else
        return languagelist[languagesetting][type];
}
function setLanguage(lang) {
    languagesetting = lang;
    mp.events.callRemote("onPlayerLanguageChange", lang);
}
mp.events.add("setLanguage", setLanguage);
function loadLanguage() {
    var langnumber = mp.game.invoke("3160758157564346030", 0);
    if (langnumber == 2)
        languagesetting = "german";
}
loadLanguage();
function getLanguage() {
    return languagesetting;
}
let bombdata = {
    changed: false,
    gotbomb: false,
    placestoplant: [],
    plantdefuseevent: false,
    isplanting: false,
    isdefusing: false,
    plantdefusestarttick: 0,
    plantedpos: null
};
function drawPlant() {
    let tickswasted = getTick() - bombdata.plantdefusestarttick;
    if (tickswasted < lobbysettings.bombplanttime) {
        mp.game.graphics.drawRect(res.x * 0.46, res.y * 0.7, res.x * 0.08, res.y * 0.02, 0, 0, 0, 187);
        let progress = tickswasted / lobbysettings.bombplanttime;
        mp.game.graphics.drawRect(res.x * 0.461, res.y * 0.701, res.x * 0.078 * progress, res.y * 0.018, 0, 180, 0, 187);
        mp.game.graphics.drawText(getLang("round", "planting"), 1, { r: 255, g: 255, b: 255, a: 255 }, 0.4, 0.4, true, res.x * 0.5, res.y * 0.71);
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
        if (bombdata.isplanting) {
            drawPlant();
        }
        else {
            bombdata.plantdefusestarttick = getTick();
            bombdata.isplanting = true;
            mp.events.callRemote("onPlayerStartPlanting");
        }
    }
    else
        checkPlantDefuseStop();
}
function drawDefuse() {
    let tickswasted = getTick() - bombdata.plantdefusestarttick;
    if (tickswasted < lobbysettings.bombdefusetime) {
        mp.game.graphics.drawRect(res.x * 0.46, res.y * 0.7, res.x * 0.08, res.y * 0.02, 0, 0, 0, 187);
        let progress = tickswasted / lobbysettings.bombdefusetime;
        mp.game.graphics.drawRect(res.x * 0.461, res.y * 0.701, res.x * 0.078 * progress, res.y * 0.018, 180, 0, 0, 187);
        mp.game.graphics.drawText(getLang("round", "defusing"), 1, { r: 255, g: 255, b: 255, a: 255 }, 0.4, 0.4, true, res.x * 0.5, res.y * 0.71);
    }
}
function checkDefuse() {
    let playerpos = mp.players.local.position;
    if (mp.game.gameplay.getDistanceBetweenCoords(playerpos.x, playerpos.y, playerpos.z, bombdata.plantedpos.x, bombdata.plantedpos.y, bombdata.plantedpos.z, true) <= 5) {
        if (bombdata.isdefusing) {
            drawDefuse();
        }
        else {
            bombdata.plantdefusestarttick = getTick();
            bombdata.isdefusing = true;
            mp.events.callRemote("onPlayerStartDefusing");
        }
    }
    else
        checkPlantDefuseStop();
}
function checkPlantDefuseStop() {
    if (bombdata.isplanting) {
        bombdata.isplanting = false;
        mp.events.callRemote("onPlayerStopPlanting");
    }
    else if (bombdata.isdefusing) {
        bombdata.isdefusing = false;
        mp.events.callRemote("onPlayerStopDefusing");
    }
}
function checkPlantDefuse() {
    if (mp.players.local.weapon == WeaponHash.Unarmed) {
        if (!mp.players.local.isDeadOrDying(true)) {
            if (bombdata.gotbomb) {
                checkPlant();
                return;
            }
            else {
                checkDefuse();
                return;
            }
        }
        else
            checkPlantDefuseStop();
    }
    else
        checkPlantDefuseStop();
}
function localPlayerGotBomb(placestoplant) {
    bombdata.changed = true;
    bombdata.gotbomb = true;
    let i = placestoplant.length;
    while (i--)
        bombdata.placestoplant[i] = placestoplant[i];
    bombdata.plantdefuseevent = true;
}
function localPlayerPlantedBomb() {
    bombdata.gotbomb = false;
    bombdata.plantdefuseevent = false;
    bombdata.isplanting = false;
}
function bombPlanted(pos, candefuse) {
    if (candefuse) {
        bombdata.changed = true;
        bombdata.plantedpos = pos;
        bombdata.plantdefuseevent = true;
    }
    setRoundTimeLeft(lobbysettings.bombdetonatetime);
}
function bombDetonated() {
    mp.game.cam.shakeGameplayCam("LARGE_EXPLOSION_SHAKE", 1.0);
    new Timer(mp.game.cam.stopGameplayCamShaking, 4000, 1);
}
function removeBombThings() {
    if (bombdata.changed) {
        bombdata = {
            changed: false,
            gotbomb: false,
            placestoplant: [],
            plantdefuseevent: false,
            isplanting: false,
            isdefusing: false,
            plantdefusestarttick: 0,
            plantedpos: null
        };
    }
}
let cameradata = {
    camera: mp.cameras.new("default"),
    moving: false,
    timer: null,
};
function loadMapMiddleForCamera(mapmiddle) {
    log("loadMapMiddleForCamera " + String(mapmiddle));
    cameradata.camera.setCoord(mapmiddle.x, mapmiddle.y, mapmiddle.z + 80);
    cameradata.camera.pointAtCoord(mapmiddle.x, mapmiddle.y, mapmiddle.z);
    cameradata.camera.setActive(true);
    mp.game.cam.renderScriptCams(true, true, 3000, true, true);
}
function setCameraGoTowardsPlayer(time = -1) {
    log("setCameraGoTowardsPlayer " + time);
    let pos = gameplayCam.getCoord();
    cameradata.camera.setCoord(pos.x, pos.y, pos.z + 80);
    mp.game.cam.renderScriptCams(true, true, time == -1 ? (lobbysettings.countdowntime * 0.9) : time, true, true);
}
function stopCountdownCamera() {
    log("stopCountdownCamera");
    cameradata.camera.setActive(false);
}
let lobbychoicedata = {
    browser: null
};
mp.events.add("joinArena", function (isspectator) {
    mp.events.callRemote("joinLobby", 1, isspectator);
});
mp.events.add("getLobbyChoiceLanguage", function () {
    log("getLobbyChoiceLanguage start");
    lobbychoicedata.browser.execute("getLobbyChoiceLanguage (" + JSON.stringify(getLang("lobby_choice")) + ")");
    log("getLobbyChoiceLanguage end");
});
mp.events.add("createLobby", function () {
});
mp.events.add("onClientJoinMainMenu", () => {
    log("onClientJoinMainMenu start");
    lobbychoicedata.browser = mp.browsers.new("package://TDS-V/window/lobby/choice.html");
    mp.events.add('browserDomReady', (browser) => {
        if (browser == lobbychoicedata.browser) {
            lobbychoicedata.browser.execute("getLobbyChoiceLanguage (" + JSON.stringify(getLang("lobby_choice")) + ")");
        }
    });
    mp.gui.cursor.visible = true;
    nothidecursor++;
    log("onClientJoinMainMenu end");
});
function destroyLobbyChoiceBrowser() {
    lobbychoicedata.browser.destroy();
    nothidecursor--;
    if (nothidecursor == 0)
        mp.gui.cursor.visible = false;
}
mp.events.add("onClientPlayerJoinLobby", () => {
    log("onClientPlayerJoinLobby start");
    destroyLobbyChoiceBrowser();
    log("onClientPlayerJoinLobby end");
});
mp.events.add("onClientPlayerJoinRoundlessLobby", () => {
    log("onClientPlayerJoinRoundlessLobby start");
    destroyLobbyChoiceBrowser();
    log("onClientPlayerJoinRoundlessLobby end");
});
let countdownsounds = [
    "go.wav",
    "1.wav",
    "2.wav",
    "3.wav"
];
let countdowndata = {
    sounds: [
        "go.wav",
        "1.wav",
        "2.wav",
        "3.wav"
    ],
    text: null,
    timer: null,
};
function countdownFunc(counter) {
    log("countdownFunc start");
    counter--;
    ;
    if (counter > 0) {
        countdowndata.text.setText(counter.toString());
        countdowndata.text.blendTextScale(6, 1000);
        countdowndata.timer = new Timer(countdownFunc, 1000, 1, counter);
        if (countdownsounds[counter] != null) {
        }
    }
    log("countdownFunc end");
}
function startCountdown() {
    log("startCountdown start");
    countdowndata.text = new cText(Math.floor(lobbysettings.countdowntime / 1000).toString(), res.x / 2, res.y * 0.2, 1, { r: 255, g: 255, b: 255, a: 255 }, 2.0, 2.0, true);
    countdowndata.timer = new Timer(countdownFunc, lobbysettings.countdowntime % 1000, 1, Math.floor(lobbysettings.countdowntime / 1000) + 1);
    log("startCountdown end");
}
function startCountdownAfterwards(timeremaining) {
    log("startCountdownAfterwards start");
    countdowndata.text = new cText(timeremaining.toString(), res.x / 2, res.y * 0.2, 1, { r: 255, g: 255, b: 255, a: 255 }, 2.0, 2.0, true);
    countdownFunc(timeremaining + 1);
    log("startCountdownAfterwards end");
}
function endCountdown() {
    log("endCountdown start");
    if (countdowndata.text == null) {
        countdowndata.text = new cText("GO", res.x / 2, res.y * 0.2, 1, { r: 255, g: 255, b: 255, a: 255 }, 2.0, 2.0, true);
    }
    else
        countdowndata.text.setText("GO");
    if (countdowndata.timer != null)
        countdowndata.timer.kill();
    countdowndata.timer = new Timer(stopCountdown, 2000, 1);
    log("endCountdown end");
}
function stopCountdown() {
    log("stopCountdown start");
    if (countdowndata.text != null) {
        countdowndata.text.remove();
        countdowndata.text = null;
    }
    if (countdowndata.timer != null) {
        countdowndata.timer.kill();
        countdowndata.timer = null;
    }
    log("stopCountdown end");
}
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
    if (p.X < maplimitdata.minX || p.X > maplimitdata.maxX || p.Y < maplimitdata.minY || p.Y > maplimitdata.maxY) {
        return false;
    }
    var inside = false;
    var vs = maplimitdata.limit;
    for (var i = 0, j = vs.length - 1; i < vs.length; j = i++) {
        var xi = vs[i].X, yi = vs[i].Y;
        var xj = vs[j].X, yj = vs[j].Y;
        var intersect = ((yi > p.Y) != (yj > p.Y))
            && (p.X < (xj - xi) * (p.Y - yi) / (yj - yi) + xi);
        if (intersect)
            inside = !inside;
    }
    return inside;
}
;
function checkMapLimit() {
    log("checkMapLimit start");
    if (maplimitdata.limit != null) {
        var pos = mp.players.local.position;
        if (!pointIsInPoly(pos)) {
            maplimitdata.outsidecounter--;
            if (maplimitdata.outsidecounter == 10 && maplimitdata.outsidetext == null)
                maplimitdata.outsidetext = new cText(getLang("round", "outside_map_limit").replace("{1}", maplimitdata.outsidecounter), res.x / 2, res.y / 2, 1, { r: 255, g: 255, b: 255, a: 255 }, 1.2, 1.2, true);
            else if (maplimitdata.outsidecounter > 0)
                maplimitdata.outsidetext.setText(getLang("round", "outside_map_limit").replace("{1}", maplimitdata.outsidecounter));
            else if (maplimitdata.outsidecounter == 0) {
                mp.events.callRemote("onPlayerWasTooLongOutsideMap");
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
    log("checkMapLimit start");
}
function loadMapLimitData(data) {
    log("loadMapLimitData start");
    maplimitdata.limit = [];
    for (let j = 0; j < data.length; j++) {
        maplimitdata.limit[j] = { X: Number.parseFloat(data[j].Item1), Y: Number.parseFloat(data[j].Item2) };
    }
    maplimitdata.outsidecounter = 11;
    if (data.length > 0) {
        var minX = maplimitdata.limit[0].X;
        var maxX = maplimitdata.limit[0].X;
        var minY = maplimitdata.limit[0].Y;
        var maxY = maplimitdata.limit[0].Y;
        for (let i = 1; i < data.length; i++) {
            var q = maplimitdata.limit[i];
            minX = Math.min(q.X, minX);
            maxX = Math.max(q.X, maxX);
            minY = Math.min(q.Y, minY);
            maxY = Math.max(q.Y, maxY);
        }
        maplimitdata.minX = minX;
        maplimitdata.maxX = maxX;
        maplimitdata.minY = minY;
        maplimitdata.maxY = maxY;
    }
    log("loadMapLimitData end");
}
function resetMapLimitCheck() {
    if (maplimitdata.outsidetext != null) {
        maplimitdata.outsidecounter = 11;
        maplimitdata.outsidetext.remove();
        maplimitdata.outsidetext = null;
    }
}
function startMapLimit() {
    log("startMapLimit start");
    if (maplimitdata.checktimer != null)
        maplimitdata.checktimer.kill();
    if (maplimitdata.limit[0] != undefined) {
        maplimitdata.checktimer = new Timer(checkMapLimit, 1000, -1);
    }
    log("startMapLimit end");
}
function stopMapLimitCheck() {
    log("stopMapLimitCheck start");
    if (maplimitdata.checktimer != null) {
        maplimitdata.checktimer.kill();
        maplimitdata.checktimer = null;
    }
    maplimitdata.outsidecounter = 11;
    if (maplimitdata.outsidetext != null) {
        maplimitdata.outsidetext.remove();
        maplimitdata.outsidetext = null;
    }
    log("stopMapLimitCheck end");
}
function emptyMapLimit() {
    maplimitdata.limit = [];
}
let rounddata = {
    mapinfo: null,
    isspectator: true,
    infight: false
};
function setMapInfo(mapname) {
    rounddata.mapinfo = new cText(mapname, res.x * 0.5, res.y * 0.95, 1, { r: 255, g: 255, b: 255, a: 255 }, 0.5, 0.5, true);
}
mp.events.add("render", () => {
    if (!rounddata.infight) {
    }
});
function removeMapInfo() {
    log("removeMapInfo start");
    if (rounddata.mapinfo != null) {
        rounddata.mapinfo.remove();
        rounddata.mapinfo = null;
    }
    log("removeMapInfo end");
}
function removeRoundThings(removemapinfo) {
    log("removeRoundThings start");
    stopSpectate();
    stopCountdown();
    if (removemapinfo) {
        removeMapInfo();
    }
    log("removeRoundThings end");
}
function toggleFightMode(bool) {
    if (bool) {
        rounddata.infight = true;
    }
    else {
        rounddata.infight = false;
        stopMapLimitCheck();
    }
}
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
var lobbysettings = {
    countdowntime: 0,
    bombdetonatetime: 0,
    bombplanttime: 0,
    bombdefusetime: 0,
    roundendtime: 0
};
let spectatedata = {
    binded: false
};
function pressSpectateKeyLeft(sender, e) {
    mp.events.callRemote("spectateNext", false);
}
function pressSpectateKeyRight(sender, e) {
    mp.events.callRemote("spectateNext", true);
}
function startSpectate() {
    if (!spectatedata.binded) {
        mp.keys.bind(Keys.LeftArrow, true, pressSpectateKeyLeft);
        mp.keys.bind(Keys.A, true, pressSpectateKeyLeft);
        mp.keys.bind(Keys.RightArrow, true, pressSpectateKeyRight);
        mp.keys.bind(Keys.D, true, pressSpectateKeyRight);
        spectatedata.binded = true;
    }
}
function stopSpectate() {
    if (spectatedata.binded) {
        mp.keys.unbind(Keys.LeftArrow, true, pressSpectateKeyLeft);
        mp.keys.unbind(Keys.A, true, pressSpectateKeyLeft);
        mp.keys.unbind(Keys.RightArrow, true, pressSpectateKeyRight);
        mp.keys.unbind(Keys.D, true, pressSpectateKeyRight);
        spectatedata.binded = false;
    }
}
let loginpanel = {
    loginbrowser: null,
    name: null,
    isregistered: null
};
mp.events.add("loginFunc", function (password) {
    mp.events.callRemote("onPlayerTryLogin", password);
});
mp.events.add("registerFunc", function (password, email) {
    mp.events.callRemote("onPlayerTryRegister", password, email);
});
mp.events.add("getRegisterLoginLanguage", () => {
    loginpanel.loginbrowser.execute("loadLanguage ( " + JSON.stringify(getLang("loginregister")) + ");");
});
mp.events.add("startRegisterLogin", function (name, isregistered) {
    log("startRegisterLogin registerlogin start");
    loginpanel.name = name;
    loginpanel.isregistered = isregistered;
    loginpanel.loginbrowser = mp.browsers.new("package://TDS-V/window/registerlogin/registerlogin.html");
    mp.events.add('browserDomReady', (browser) => {
        if (browser == loginpanel.loginbrowser) {
            browser.execute("getLoginPanelData ( " + loginpanel.name + ", " + loginpanel.isregistered + ", " + JSON.stringify(getLang("loginregister")) + ");");
        }
    });
    mp.gui.chat.activate(false);
    mp.game.ui.displayHud(false);
    mp.gui.cursor.visible = true;
    nothidecursor++;
    log("startRegisterLogin registerlogin end");
});
mp.events.add("registerLoginSuccessful", function () {
    log("registerLoginSuccessful registerlogin start");
    loginpanel.loginbrowser.destroy();
    loginpanel.loginbrowser = null;
    mp.gui.chat.activate(true);
    nothidecursor--;
    log("registerLoginSuccessful registerlogin end");
});
