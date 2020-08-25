import Language from "../../interfaces/output/language.interface";
import English from "./english.language";
import LoginRegisterGerman from "./login-register.german.language";

export default class German extends English implements Language {
    AFK_KICK_INFO = "Du warst AFK und wurdest aus der Lobby geworfen.";

    AFK_KICK_WARNING = "Du bist AFK und wirst\nin {0} Sekunden gekickt!\nÄndere deine Position oder schieße!";

    BOMB_PLANTED = "Die Bombe wurde platziert!";

    COULD_NOT_LOAD_OBJECT = "Konnte Objekt nicht laden.";

    COUNTDOWN_STARTED_NOTIFICATION = "Countdown gestartet";

    DEFUSING = "Entschärfung ...";

    DELETE_DESCRIPTION = "Entfernen";

    DELETE_KEY = "Entf";

    DIRECTION = "Richtung";

    DOWN = "Runter";

    END_KEY = "Ende";

    ERROR = "Fehler aufgetaucht, bitte melde es! Info: '{0}'";

    FAST_MODE = "Schneller-Modus (3x)";

    FASTER = "Schneller";

    FIRING_MODE = "Feuermodus";

    FREECAM = "Freecam";

    GANG_LOBBY_FREE_HOUSE_DESCRIPTION = "Level {0} Haus";

    LEFT_CTRL = "LSTRG";

    LEFT_SHIFT = "LShift";

    LET_IT_FLOAT = "Schweben lassen";

    LOGIN_REGISTER_TEXTS = new LoginRegisterGerman();

    OBJECT_MODEL_INVALID = "Objekt Modell ist ungültig.";

    ON_FOOT = "Zu Fuß";

    OUTSIDE_MAP_LIMIT = "Du bist außerhalb der Map-Grenze!\nDir bleiben noch {0} Sekunden, um zurück zu gehen.";

    OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME = "Du bist außerhalb der Map-Grenze!\nWenn du nicht zurück gehst, wirst du in {0} Sekunden sterben.";

    OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME = "Du bist außerhalb der Map-Grenze!\nWenn du nicht zurück gehst, wirst du in {0} Sekunden zurück teleportiert.";

    PLANTING = "Lege Bombe ...";

    PUT_ON_GROUND = "Boden legen";

    ROUND_ENDED_NOTIFICATION = "Runde zuende";

    ROUND_INFOS = "Runden Infos";

    ROUND_STARTED_NOTIFICATION = "Runde gestartet";

    SCOREBOARD_ASSISTS = "Assists";

    SCOREBOARD_DEATHS = "Tode";

    SCOREBOARD_KILLS = "Kills";

    SCOREBOARD_LOBBY = "Lobby";

    SCOREBOARD_NAME = "Name";

    SCOREBOARD_PLAYTIME = "Spielzeit";

    SCOREBOARD_TEAM = "Team";

    SLOW_MODE = "Langsamer-Modus (0.5x)";

    SLOWER = "Langsamer";

    UP = "Hoch";

    WELCOME_MESSAGE = [
        "#n#Willkommen auf dem #b#Team Deathmatch Server#w#.",
        "#n#Für Ankündigungen, Support, Bug-Meldung usw.",
        "#n#bitte unseren Discord-Server nutzen:",
        "#n#discord.gg/ntVnGFt",
        "#n#Du kannst den Cursor mit #r#ENDE#w# umschalten.",
        "#n#Viel Spaß wünscht das #b#TDS-Team#w#!"
    ];

    YOU_DIED = "Du bist gestorben.";
}
