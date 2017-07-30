"use strict";
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
            "outside_map_limit": "Du bist außerhalb der Map-Grenze!\nDir bleiben noch {1} Sekunden, um zurück zu gehen."
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
            "outside_map_limit": "You are outside of the map!\nThere are {1} seconds left to return to the map."
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
        }
    }
};
var languagesetting = "english";
function getLang(type, str = null) {
    if (str != null)
        return languagelist[languagesetting][type][str];
    else
        return languagelist[languagesetting][type];
}
function changeLanguage(lang) {
    languagesetting = lang;
    API.triggerServerEvent("onPlayerLanguageChange", lang);
}
API.onResourceStart.connect(function () {
    var langnumber = API.returnNative("3160758157564346030", 0);
    if (langnumber == 2)
        languagesetting = "german";
    API.triggerServerEvent("onPlayerJoin", languagesetting);
});
