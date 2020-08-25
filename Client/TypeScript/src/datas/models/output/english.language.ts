import Language from "../../interfaces/output/language.interface";
import LoginRegisterEnglish from "./login-register.english.language";

export default class English implements Language {
    AFK_KICK_INFO = "You were AFK and got kicked out of the lobby.";

    AFK_KICK_WARNING = "You are AFK and will\nbe kicked in {0} seconds!\nChange your position or shoot!";

    BOMB_PLANTED = "The bomb got planted!";

    COULD_NOT_LOAD_OBJECT = "Could not load object.";

    COUNTDOWN_STARTED_NOTIFICATION = "Countdown started";

    DEFUSING = "Defusing ...";

    DELETE_DESCRIPTION = "Delete";

    DELETE_KEY = "Delete";

    DIRECTION = "Direction";

    DOWN = "Down";

    END_KEY = "End";

    ERROR = "Error occured, please report this: '{0}'";

    FAST_MODE = "Fast mode (3x)";

    FASTER = "Faster";
    FIRING_MODE = "Firing mode";
    FREECAM = "Freecam";
    GANG_LOBBY_FREE_HOUSE_DESCRIPTION = "Level {0} house";
    INVALID_MODEL = "The model '{0}' of an object is invalid and could not be loaded.";
    INVALID_MODEL_INFO = "The model of an object is invalid and could not be loaded. The developers are informed.";

    LEFT_CTRL = "LCTRL";

    LEFT_SHIFT = "LShift";

    LET_IT_FLOAT = "Let it float";

    LOGIN_REGISTER_TEXTS = new LoginRegisterEnglish();

    OBJECT_MODEL_INVALID = "Object model is invalid.";

    ON_FOOT = "On Foot";

    OUTSIDE_MAP_LIMIT = "You are outside of the map!\nThere are {0} seconds left to return to the map.";

    OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME = "You're off the map!\nIf you don't go back, you will die in {0} seconds.";

    OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME = "You're off the map!\nIf you do not go back, you will be teleported back in {0} seconds.";

    PLANTING = "Planting ...";

    PUT_ON_GROUND = "Put on ground";

    ROUND_ENDED_NOTIFICATION = "Round ended";

    ROUND_INFOS = "Round infos";

    ROUND_STARTED_NOTIFICATION = "Round started";

    SCOREBOARD_ASSISTS = "Assists";

    SCOREBOARD_DEATHS = "Deaths";

    SCOREBOARD_KILLS = "Kills";

    SCOREBOARD_LOBBY = "Lobby";

    SCOREBOARD_NAME = "Name";

    SCOREBOARD_PLAYTIME = "Playtime";

    SCOREBOARD_TEAM = "Team";

    SLOW_MODE = "Slow mode (0.5x)";

    SLOWER = "Slower";

    UP = "Up";

    WELCOME_MESSAGE = [
        "#n#Welcome to #b#Team Deathmatch Server#w#.",
        "#n#For announcements, support, bug-reports etc.",
        "#n#please visit our Discord-server:",
        "#n#discord.gg/ntVnGFt",
        "#n#You can get/hide the cursor with #r#END#w#.",
        "#n#Have fun on TDS#w#!"
    ];

    YOU_DIED = "You died.";

    get(index: any): string {
        return (this as any)[index];
    }
}
