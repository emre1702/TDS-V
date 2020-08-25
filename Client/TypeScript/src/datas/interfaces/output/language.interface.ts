export default interface Language {
    AFK_KICK_INFO: string;
    AFK_KICK_WARNING: string;
    BOMB_PLANTED: string;
    COULD_NOT_LOAD_OBJECT: string;
    COUNTDOWN_STARTED_NOTIFICATION: string;
    DEFUSING: string;
    DELETE_DESCRIPTION: string;
    DELETE_KEY: string;
    DIRECTION: string;
    DOWN: string;
    END_KEY: string;
    ERROR: string;
    FAST_MODE: string;
    FASTER: string;
    FIRING_MODE: string;
    FREECAM: string;
    GANG_LOBBY_FREE_HOUSE_DESCRIPTION: string;
    INVALID_MODEL: string;
    INVALID_MODEL_INFO: string;
    LEFT_CTRL: string;
    LEFT_SHIFT: string;
    LET_IT_FLOAT: string;
    LOGIN_REGISTER_TEXTS: LoginRegisterTexts;

    OBJECT_MODEL_INVALID: string;
    ON_FOOT: string;
    OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME: string;
    OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME: string;
    PLANTING: string;
    PUT_ON_GROUND: string;
    ROUND_ENDED_NOTIFICATION: string;
    ROUND_INFOS: string;
    ROUND_STARTED_NOTIFICATION: string;
    SCOREBOARD_ASSISTS: string;
    SCOREBOARD_DEATHS: string;
    SCOREBOARD_KILLS: string;
    SCOREBOARD_NAME: string;
    SCOREBOARD_PLAYTIME: string;
    SCOREBOARD_TEAM: string;
    SLOW_MODE: string;
    SLOWER: string;
    UP: string;
    WELCOME_MESSAGE: string[];
    YOU_DIED: string;


    get(index: any): string;
}
