"use strict";
mp.gui.execute("window.location = 'package://TDS-V/window/chat/chat.html'");
mp.events.add("onChatLoad", () => {
    mp.events.callRemote("onPlayerChatLoad", languagesetting);
});
let voicechat = mp.browsers.new("https://tds-v.com:8546/TDSvoice.html");
function setVoiceChatRoom(room) {
    voicechat.execute("joinRoom ( '" + room + "' );");
}
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
var res = mp.game.graphics.getScreenActiveResolution(0, 0);
var nothidecursor = 0;
var currentmoney = null;
var localPlayer = mp.players.local;
var gameplayCam = mp.cameras.new("gameplay");
let activatedlogging = false;
function log(message) {
    if (activatedlogging) {
        mp.gui.chat.push(message);
    }
}
let mainbrowserdata = {
    browser: mp.browsers.new("package://TDS-V/window/main/main.html"),
};
mp.events.add("onClientMoneyChange", money => {
    currentmoney = money;
    mainbrowserdata.browser.execute("setMoney ( " + money + " );");
});
function playSound(soundname) {
    mainbrowserdata.browser.execute("playSound ( '" + soundname + "' );");
}
function showBloodscreen() {
    mainbrowserdata.browser.execute("showBloodscreen ();");
}
function addKillMessage(msg) {
    mainbrowserdata.browser.execute("addKillMessage ('" + msg + "');");
}
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
let uimenudata = {
    lastbrowser: null
};
function startUIMenu(uimenu) {
    if (uimenudata.lastbrowser != null)
        uimenudata.lastbrowser.destroy();
    uimenudata.lastbrowser = mp.browsers.new("package://TDS-V/window/uimenu/index.html");
    mp.events.add("browserDomReady", (thebrowser) => {
        if (thebrowser == uimenudata.lastbrowser)
            uimenudata.lastbrowser.execute("addScript ( \"" + uimenu + "\");");
    });
    return uimenudata.lastbrowser;
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
    lastarmorhp: 200
};
mp.events.add("render", () => {
    if (!rounddata.infight)
        return;
    let armorhp = mp.players.local.getHealth() + mp.players.local.getArmour();
    if (armorhp < damagesysdata.lastarmorhp)
        showBloodscreen();
    damagesysdata.lastarmorhp = armorhp;
});
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
function getStringWidth(text, scale, font) {
    mp.game.ui.setTextEntryForWidth("STRING");
    mp.game.ui.addTextComponentSubstringPlayerName(text);
    mp.game.ui.setTextFont(font);
    mp.game.ui.setTextScale(scale[0], scale[1]);
    let width = mp.game.ui.getTextScreenWidth(true);
    return width;
}
function drawText(text, x, y, font, color, scale, outline, alignment, relative) {
    let xpos = relative ? x : x / res.x;
    let ypos = relative ? y : y / res.y;
    let thetext = text + "               ";
    if (alignment == Alignment.LEFT)
        xpos += getStringWidth(text, scale, font);
    else if (alignment == Alignment.RIGHT)
        xpos -= getStringWidth(text, scale, font);
    mp.game.graphics.drawText(thetext, [xpos, ypos], { font, color, scale, outline });
}
function drawRectangle(x, y, width, length, color, alignment = Alignment.LEFT, relative = true) {
    let xpos = relative ? x : x / res.x;
    let ypos = relative ? y : y / res.y;
    let sizex = relative ? width : width / res.x;
    let sizey = relative ? length : length / res.y;
    if (alignment == Alignment.LEFT)
        xpos += sizex / 2;
    else if (alignment == Alignment.RIGHT)
        xpos -= sizex / 2;
    ypos += sizey / 2;
    mp.game.graphics.drawRect(xpos, ypos, sizex, sizey, color[0], color[1], color[2], color[3]);
}
mp.events.add("render", () => {
    let tick = getTick();
    for (var i = 0; i < drawdrawings.length; i++) {
        let c = drawdrawings[i];
        if (c.activated) {
            switch (c.type) {
                case "cRectangle":
                    drawRectangle(c.position[0], c.position[1], c.size[0], c.size[1], c.color, c.alignment, c.relative);
                    break;
                case "cText":
                    let alpha = c.color[3];
                    if (c.enda != null) {
                        alpha = getBlendValue(tick, c.color[3], c.enda, c.endastarttick, c.endaendtick);
                    }
                    let scale = [c.scale[0], c.scale[1]];
                    if (c.endscale != null) {
                        scale[0] = getBlendValue(tick, c.scale[0], c.endscale[0], c.endscalestarttick, c.endscaleendtick);
                        scale[1] = getBlendValue(tick, c.scale[1], c.endscale[1], c.endscalestarttick, c.endscaleendtick);
                    }
                    drawText(c.text, c.position[0], c.position[1], c.font, [c.color[0], c.color[1], c.color[2], alpha], scale, c.outline, c.alignment, c.relative);
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
class cLine {
    constructor(start, end, r, g, b, a) {
        this.type = "cLine";
        this.activated = true;
        this.remove = removeClassDraw;
        this.type = "line";
        this.activated = true;
        this.start = start;
        this.end = end;
        this.color = [r, g, b, a];
        this.remove = removeClassDraw;
        drawdrawings.push(this);
    }
}
class cRectangle {
    constructor(xpos, ypos, width, height, color, alignment = Alignment.LEFT, relative = true) {
        this.type = "cRectangle";
        this.activated = true;
        this.remove = removeClassDraw;
        this.setWidth = (newwidth) => {
            this.size[0] = newwidth;
        };
        this.position = [xpos, ypos];
        this.size = [width, height];
        this.color = color;
        this.alignment = alignment;
        this.relative = relative;
        drawdrawings.push(this);
    }
}
class cText {
    constructor(text, x, y, font, color, scale, outline, alignment, relative) {
        this.type = "cText";
        this.activated = true;
        this.endscale = null;
        this.enda = null;
        this.remove = removeClassDraw;
        this.blendTextAlpha = blendClassDrawTextAlpha;
        this.blendTextScale = blendClassDrawTextScale;
        this.getText = getClassDrawText;
        this.setText = setClassDrawText;
        this.text = text;
        this.position = [x, y];
        this.font = font;
        this.color = color;
        this.scale = scale;
        this.outline = outline;
        this.alignment = alignment;
        this.relative = relative;
        drawdrawings.push(this);
    }
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
var Alignment;
(function (Alignment) {
    Alignment[Alignment["CENTER"] = 0] = "CENTER";
    Alignment[Alignment["LEFT"] = 1] = "LEFT";
    Alignment[Alignment["RIGHT"] = 2] = "RIGHT";
})(Alignment || (Alignment = {}));
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
    Keys[Keys["Y"] = 89] = "Y";
    Keys[Keys["Z"] = 90] = "Z";
})(Keys || (Keys = {}));
var Language;
(function (Language) {
    Language["ENGLISH"] = "ENGLISH";
    Language["GERMAN"] = "GERMAN";
})(Language || (Language = {}));
var languagelist = {
    "GERMAN": {
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
    "ENGLISH": {
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
let languagesetting = "ENGLISH";
function getLang(type, str = null) {
    if (str != null)
        return languagelist[languagesetting][type][str];
    else
        return languagelist[languagesetting][type];
}
function setLanguage(lang) {
    languagesetting = lang;
    mp.storage.data.language = lang;
    mp.storage.flush();
    mp.events.callRemote("onPlayerLanguageChange", lang);
}
mp.events.add("setLanguage", setLanguage);
function loadLanguage() {
    let langnumber = mp.game.invoke("3160758157564346030", 0);
    let savedlang = mp.storage.data.language;
    mp.gui.chat.push("" + savedlang);
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
        if (bombdata.isplanting) {
            updatePlantDefuseProgress();
        }
        else {
            bombdata.plantdefusestarttick = getTick();
            bombdata.isplanting = true;
            bombdata.draw.backrect = new cRectangle(0.46, 0.7, 0.08, 0.02, [0, 0, 0, 187], Alignment.CENTER, true);
            bombdata.draw.progrect = new cRectangle(0.461, 0.701, 0.078, 0.018, [0, 180, 0, 187], Alignment.CENTER, true);
            bombdata.draw.text = new cText(getLang("round", "planting"), 0.5, 0.71, 0, [255, 255, 255, 255], [1.0, 0.4], true, Alignment.CENTER, true);
            mp.events.callRemote("onPlayerStartPlanting");
        }
    }
    else
        checkPlantDefuseStop();
}
function checkDefuse() {
    let playerpos = mp.players.local.position;
    if (mp.game.gameplay.getDistanceBetweenCoords(playerpos.x, playerpos.y, playerpos.z, bombdata.plantedpos.x, bombdata.plantedpos.y, bombdata.plantedpos.z, true) <= 5) {
        if (bombdata.isdefusing) {
            updatePlantDefuseProgress();
        }
        else {
            bombdata.plantdefusestarttick = getTick();
            bombdata.isdefusing = true;
            bombdata.draw.backrect = new cRectangle(0.46, 0.7, 0.08, 0.02, [0, 0, 0, 187], Alignment.CENTER, true);
            bombdata.draw.progrect = new cRectangle(0.461, 0.701, 0.078, 0.018, [180, 0, 0, 187], Alignment.CENTER, true);
            bombdata.draw.text = new cText(getLang("round", "defusing"), 0.5, 0.71, 0, [255, 255, 255, 255], [1.0, 0.4], true, Alignment.CENTER, true);
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
    if (bombdata.draw.backrect != null) {
        bombdata.draw.backrect.remove();
        bombdata.draw.backrect = null;
        bombdata.draw.progrect.remove();
        bombdata.draw.progrect = null;
        bombdata.draw.text.remove();
        bombdata.draw.text = null;
    }
}
function checkPlantDefuse() {
    if (mp.players.local.weapon == WeaponHash.Unarmed) {
        if (mp.game.controls.isControlJustPressed(0, 24)) {
            if (!mp.players.local.isDeadOrDying(true)) {
                mp.game.controls.disableControlAction(0, 24, true);
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
    mp.events.add("checkPlantDefuse", checkPlantDefuse);
}
function localPlayerPlantedBomb() {
    bombdata.gotbomb = false;
    bombdata.plantdefuseevent = false;
    bombdata.isplanting = false;
    mp.events.remove("checkPlantDefuse", checkPlantDefuse);
}
function bombPlanted(pos, candefuse) {
    if (candefuse) {
        bombdata.changed = true;
        bombdata.plantedpos = pos;
        bombdata.plantdefuseevent = true;
    }
    mp.events.add("checkPlantDefuse", checkPlantDefuse);
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
        if (bombdata.draw.backrect != null) {
            bombdata.draw.backrect.remove();
            bombdata.draw.backrect = null;
            bombdata.draw.progrect.remove();
            bombdata.draw.progrect = null;
            bombdata.draw.text.remove();
            bombdata.draw.text = null;
        }
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
function loadMapMiddleForCamera(mapmiddle) {
    log("loadMapMiddleForCamera");
    cameradata.camera.setCoord(mapmiddle.x, mapmiddle.y, mapmiddle.z + 110);
    cameradata.camera.pointAtCoord(mapmiddle.x, mapmiddle.y, mapmiddle.z);
    cameradata.camera.setActive(true);
    mp.game.cam.renderScriptCams(true, true, 3000, true, true);
}
function setCameraGoTowardsPlayer(time = -1) {
    log("setCameraGoTowardsPlayer " + time);
    let pos = gameplayCam.getCoord();
    let rot = gameplayCam.getRot(2);
    cameradata.camera.setParams(pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, gameplayCam.getFov(), time == -1 ? (lobbysettings.countdowntime * 0.9) : time, 1, 1, 2);
}
function stopCountdownCamera() {
    log("stopCountdownCamera");
    cameradata.camera.setActive(false);
    mp.game.cam.renderScriptCams(false, false, 0, true, true);
}
let lobbychoicedata = {
    browser: null
};
mp.events.add("joinArena", function (isspectator) {
    mp.events.callRemote("joinLobby", 1, isspectator);
});
mp.events.add("getLobbyChoiceLanguage", function () {
    log("getLobbyChoiceLanguage");
    lobbychoicedata.browser.execute("getLobbyChoiceLanguage (`" + JSON.stringify(getLang("lobby_choice")) + "`)");
});
mp.events.add("createLobby", function () {
});
mp.events.add("onClientJoinMainMenu", () => {
    log("onClientJoinMainMenu");
    lobbychoicedata.browser = mp.browsers.new("package://TDS-V/window/lobby/choice.html");
    mp.events.add('browserDomReady', (browser) => {
        if (browser == lobbychoicedata.browser) {
            lobbychoicedata.browser.execute("getLobbyChoiceLanguage (`" + JSON.stringify(getLang("lobby_choice")) + "`)");
        }
    });
    mp.gui.cursor.visible = true;
    nothidecursor++;
});
function destroyLobbyChoiceBrowser() {
    lobbychoicedata.browser.destroy();
    nothidecursor--;
    if (nothidecursor == 0)
        mp.gui.cursor.visible = false;
}
mp.events.add("onClientPlayerJoinLobby", () => {
    destroyLobbyChoiceBrowser();
});
mp.events.add("onClientPlayerJoinRoundlessLobby", () => {
    log("onClientPlayerJoinRoundlessLobby");
    destroyLobbyChoiceBrowser();
});
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
    counter--;
    ;
    if (counter > 0) {
        countdowndata.text.setText(counter + "");
        countdowndata.text.blendTextScale(6, 1000);
        countdowndata.timer = new Timer(countdownFunc, 1000, 1, counter);
        if (countdowndata.sounds[counter] != null) {
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
mp.events.add("onClientPlayerJoinLobby", (lobbyid, isspectator, mapname, teamnames, teamcolors, countdowntime, roundtime, bombdetonatetime, bombplanttime, bombdefusetime, roundendtime) => {
    log("onClientPlayerJoinLobby");
    lobbysettings.id = lobbyid;
    rounddata.isspectator = isspectator == 1;
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
});
mp.events.add("onClientPlayerLeaveLobby", (playerID) => {
    log("onClientPlayerLeaveLobby");
    let player = mp.players.at(playerID);
    if (mp.players.local == player) {
        toggleFightMode(false);
        removeBombThings();
        removeRoundThings(true);
        stopCountdownCamera();
        removeRoundInfo();
    }
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
    log("checkMapLimit");
    if (maplimitdata.limit != null) {
        var pos = mp.players.local.position;
        if (!pointIsInPoly(pos)) {
            maplimitdata.outsidecounter--;
            if (maplimitdata.outsidecounter == 10 && maplimitdata.outsidetext == null)
                maplimitdata.outsidetext = new cText(getLang("round", "outside_map_limit").replace("{1}", maplimitdata.outsidecounter), 0.5, 0.5, 0, [255, 255, 255, 255], [1.2, 1.2], true, Alignment.CENTER, true);
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
}
function loadMapLimitData(data) {
    log("loadMapLimitData");
    maplimitdata.limit = [];
    for (let j = 0; j < data.length; j++) {
        maplimitdata.limit[j] = { x: data[j].Item1, y: data[j].Item2 };
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
    log("startMapLimit");
    if (maplimitdata.checktimer != null)
        maplimitdata.checktimer.kill();
    if (0 in maplimitdata.limit) {
        maplimitdata.checktimer = new Timer(checkMapLimit, 1000, -1);
    }
}
function stopMapLimitCheck() {
    log("stopMapLimitCheck");
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
let mapvotingdata = {
    menu: null,
    lastlobbyID: -1,
    lastmapdatas: "",
    openwithlastdata: false,
    menuloaded: false
};
function openMapVotingMenu() {
    ++nothidecursor;
    mp.gui.cursor.visible = true;
    mapvotingdata.menuloaded = false;
    mapvotingdata.menu = mp.browsers.new("package://TDS-V/window/mapmanager/mapmanager.html");
    if (lobbysettings.id != mapvotingdata.lastlobbyID) {
        mapvotingdata.openwithlastdata = false;
        mp.events.callRemote("onMapMenuOpen");
    }
    else
        mapvotingdata.openwithlastdata = true;
}
function closeMapVotingMenu() {
    mapvotingdata.menu.destroy();
    mapvotingdata.menu = null;
    if (--nothidecursor <= 0)
        mp.gui.cursor.visible = false;
}
mp.events.add("closeMapVotingMenu", closeMapVotingMenu);
mp.keys.bind(Keys.M, false, () => {
    if (mapvotingdata.menu == null)
        openMapVotingMenu();
    else
        closeMapVotingMenu();
});
mp.events.add("onClientMapMenuOpen", (mapdatasjson) => {
    mapvotingdata.lastmapdatas = mapdatasjson;
    if (mapvotingdata.menuloaded)
        mapvotingdata.menu.execute("openMapMenu ( '" + getLanguage() + "', '" + mapdatasjson + "');");
    else
        mapvotingdata.openwithlastdata = true;
});
mp.events.add("browserDomReady", (browser) => {
    if (browser == mapvotingdata.menu) {
        mapvotingdata.menuloaded = true;
        if (mapvotingdata.openwithlastdata)
            mapvotingdata.menu.execute("openMapMenu ( '" + getLanguage() + "', '" + mapvotingdata.lastmapdatas + "');");
    }
});
mp.events.add("onMapMenuVote", (mapname) => {
    mp.events.callRemote("onMapVotingRequest", mapname);
});
let rounddata = {
    mapinfo: null,
    isspectator: true,
    infight: false
};
function setMapInfo(mapname) {
    rounddata.mapinfo = new cText(mapname, 0.5, 0.95, 0, [255, 255, 255, 255], [0.5, 0.5], true, Alignment.CENTER, true);
}
mp.events.add("render", () => {
    if (!rounddata.infight) {
    }
});
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
mp.events.add("onClientMapChange", function (maplimit, mapmidx, mapmidy, mapmidz) {
    log("onClientMapChange");
    mp.game.cam.doScreenFadeIn(lobbysettings.roundendtime / 2);
    maplimit = JSON.parse(maplimit);
    if (maplimit.length > 0)
        loadMapLimitData(maplimit);
    loadMapMiddleForCamera(new mp.Vector3(mapmidx, mapmidy, mapmidz));
});
mp.events.add("onClientCountdownStart", function (mapname, resttime) {
    log("onClientCountdownStart");
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
});
mp.events.add("onClientRoundEnd", function () {
    log("onClientRoundEnd");
    mp.game.cam.doScreenFadeOut(lobbysettings.roundendtime / 2);
    toggleFightMode(false);
    removeBombThings();
    emptyMapLimit();
    removeRoundThings(false);
    stopCountdown();
    stopCountdownCamera();
    removeRoundInfo();
});
mp.events.add("onClientPlayerSpectateMode", function () {
    log("onClientPlayerSpectateMode");
    rounddata.isspectator = true;
    startSpectate();
});
mp.events.add("onClientPlayerDeath", function (playerID, teamID, killstr) {
    log("onClientPlayerDeath");
    let player = mp.players.at(playerID);
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
        roundinfo.drawclasses.rect.teams[i] = new cRectangle(startx, teamrdata.ypos, teamrdata.width, teamrdata.height, [roundinfo.teamcolors[0 + i * 3], roundinfo.teamcolors[1 + i * 3], roundinfo.teamcolors[2 + i * 3], teamrdata.a]);
        roundinfo.drawclasses.text.teams[i] = new cText(roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i], startx + teamrdata.width / 2, teamdata.ypos, 0, teamdata.color, teamdata.scale, true, Alignment.CENTER, true);
    }
    for (let j = 0; j < roundinfo.teamnames.length - leftteamamount; j++) {
        let startx = trdata.xpos + trdata.width + teamrdata.width * j;
        let i = leftteamamount + j;
        roundinfo.drawclasses.rect.teams[i] = new cRectangle(startx, teamrdata.ypos, teamrdata.width, teamrdata.height, [roundinfo.teamcolors[0 + i * 3], roundinfo.teamcolors[1 + i * 3], roundinfo.teamcolors[2 + i * 3], teamrdata.a]);
        roundinfo.drawclasses.text.teams[i] = new cText(roundinfo.teamnames[i] + "\n" + roundinfo.aliveinteams[i] + "/" + roundinfo.amountinteams[i], startx + teamrdata.width / 2, teamdata.ypos, 0, teamdata.color, teamdata.scale, true, Alignment.CENTER, true);
    }
    mp.events.add("render", refreshRoundInfo);
}
function addTeamInfos(teamnames, teamcolors) {
    for (let i = 1; i < teamnames.length; i++) {
        roundinfo.teamnames[i - 1] = teamnames[i];
    }
    for (let i = 3; i < teamcolors.length; i++) {
        roundinfo.teamcolors[i - 3] = teamcolors[i];
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
    mp.events.callRemote("spectateNext", false);
}
function pressSpectateKeyRight() {
    mp.events.callRemote("spectateNext", true);
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
        mp.keys.unbind(Keys.LeftArrow, pressSpectateKeyLeft);
        mp.keys.unbind(Keys.A, pressSpectateKeyLeft);
        mp.keys.unbind(Keys.RightArrow, pressSpectateKeyRight);
        mp.keys.unbind(Keys.D, pressSpectateKeyRight);
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
    loginpanel.loginbrowser.execute("loadLanguage ( `" + JSON.stringify(getLang("loginregister")) + "` );");
});
mp.events.add("startRegisterLogin", function (name, isregistered) {
    log("startRegisterLogin registerlogin");
    loginpanel.name = name;
    loginpanel.isregistered = isregistered == 1;
    loginpanel.loginbrowser = mp.browsers.new("package://TDS-V/window/registerlogin/registerlogin.html");
    mp.events.add('browserDomReady', (browser) => {
        if (browser == loginpanel.loginbrowser)
            browser.execute("getLoginPanelData ( `" + loginpanel.name + "`, `" + loginpanel.isregistered + "`, `" + JSON.stringify(getLang("loginregister")) + "` );");
    });
    mp.gui.chat.show(false);
    mp.game.ui.displayHud(false);
    mp.gui.cursor.visible = true;
    nothidecursor++;
});
mp.events.add("registerLoginSuccessful", function () {
    log("registerLoginSuccessful registerlogin");
    loginpanel.loginbrowser.destroy();
    loginpanel.loginbrowser = null;
    mp.gui.chat.show(true);
    mp.game.ui.displayHud(true);
    nothidecursor--;
});
