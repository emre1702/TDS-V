﻿using TDS_Shared.Data.Enums;

namespace TDS_Client.Data.Interfaces
{
    public interface ILanguage
    {
        ILoginRegisterTexts LOGIN_REGISTER_TEXTS { get; }

        string[] WELCOME_MESSAGE { get; }
        string OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME { get; }
        string OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME { get; }
        string PLANTING { get; }
        string DEFUSING { get; }
        string SCOREBOARD_NAME { get; }
        string SCOREBOARD_PLAYTIME { get; }
        string SCOREBOARD_KILLS { get; }
        string SCOREBOARD_ASSISTS { get; }
        string SCOREBOARD_DEATHS { get; }
        string SCOREBOARD_TEAM { get; }
        string BOMB_PLANTED { get; }
        string DIRECTION { get; }
        string FASTER { get; }
        string SLOWER { get; }
        string FAST_MODE { get; }
        string SLOW_MODE { get; }
        string DOWN { get; }
        string UP { get; }
        string LEFT_SHIFT { get; }
        string LEFT_CTRL { get; }
        string DELETE_KEY { get; }
        string DELETE_DESCRIPTION { get; }
        string END_KEY { get; }
        string OBJECT_MODEL_INVALID { get; }
        string COULD_NOT_LOAD_OBJECT { get; }
        string FREECAM { get; }
        string ON_FOOT { get; }
        string PUT_ON_GROUND { get; }
        string LET_IT_FLOAT { get; }
        string ERROR { get; }
        string AFK_KICK_INFO { get; }
        string AFK_KICK_WARNING { get; }
        string FIRING_MODE { get; }
        string YOU_DIED { get; }

        Language Enum { get; }

    }
}