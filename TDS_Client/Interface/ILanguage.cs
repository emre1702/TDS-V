using TDS_Common.Enum;

namespace TDS_Client.Interface
{
    internal interface ILanguage
    {
        ILoginRegisterTexts LOGIN_REGISTER_TEXTS { get; }
        ILobbyChoiceTexts LOBBY_CHOICE_TEXTS { get; }
        IMapCreatorMenuTexts MAPCREATOR_MENU { get; }

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

        ELanguage Enum { get; }
    }
}