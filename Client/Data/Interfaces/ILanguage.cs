using TDS_Shared.Data.Enums;

namespace TDS_Client.Data.Interfaces
{
    public interface ILanguage
    {
        #region Public Properties

        string AFK_KICK_INFO { get; }
        string AFK_KICK_WARNING { get; }
        string BOMB_PLANTED { get; }
        string COULD_NOT_LOAD_OBJECT { get; }
        string COUNTDOWN_STARTED_NOTIFICATION { get; }
        string DEFUSING { get; }
        string DELETE_DESCRIPTION { get; }
        string DELETE_KEY { get; }
        string DIRECTION { get; }
        string DOWN { get; }
        string END_KEY { get; }
        Language Enum { get; }
        string ERROR { get; }
        string FAST_MODE { get; }
        string FASTER { get; }
        string FIRING_MODE { get; }
        string FREECAM { get; }
        string GANG_LOBBY_FREE_HOUSE_DESCRIPTION { get; }
        string LEFT_CTRL { get; }
        string LEFT_SHIFT { get; }
        string LET_IT_FLOAT { get; }
        ILoginRegisterTexts LOGIN_REGISTER_TEXTS { get; }

        string OBJECT_MODEL_INVALID { get; }
        string ON_FOOT { get; }
        string OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME { get; }
        string OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME { get; }
        string PLANTING { get; }
        string PUT_ON_GROUND { get; }
        string ROUND_ENDED_NOTIFICATION { get; }
        string ROUND_INFOS { get; }
        string ROUND_STARTED_NOTIFICATION { get; }
        string SCOREBOARD_ASSISTS { get; }
        string SCOREBOARD_DEATHS { get; }
        string SCOREBOARD_KILLS { get; }
        string SCOREBOARD_NAME { get; }
        string SCOREBOARD_PLAYTIME { get; }
        string SCOREBOARD_TEAM { get; }
        string SLOW_MODE { get; }
        string SLOWER { get; }
        string UP { get; }
        string[] WELCOME_MESSAGE { get; }
        string YOU_DIED { get; }

        #endregion Public Properties
    }
}
