using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Client.Interface
{
    interface ILanguage
    {
        ILoginRegisterTexts LOGIN_REGISTER_TEXTS { get; }
        ILobbyChoiceTexts LOBBY_CHOICE_TEXTS { get; }
        IMapCreatorMenuTexts MAPCREATOR_MENU { get; }
        IOrderTexts ORDER { get; }

        string OUTSIDE_MAP_LIMIT { get; }
        string PLANTING { get; }
        string DEFUSING { get; }
        string SCOREBOARD_NAME { get; }
        string SCOREBOARD_PLAYTIME { get; }
        string SCOREBOARD_KILLS { get; }
        string SCOREBOARD_ASSISTS { get; }
        string SCOREBOARD_DEATHS { get; }
        string SCOREBOARD_TEAMS { get; }
        string SCOREBOARD_LOBBY { get; }

        ELanguage Enum { get; }
    }  
}
