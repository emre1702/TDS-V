using TDS_Client.Interface;
using TDS_Common.Enum;

namespace TDS_Client.Instance.Language
{
    class English : ILanguage
    {
        public virtual string DEFUSING => "Defusing ...";
        public virtual string PLANTING => "Planting ...";
        public virtual string OUTSIDE_MAP_LIMIT => "You are outside of the map!\nThere are {1} seconds left to return to the map.";

        public virtual string SCOREBOARD_NAME => "Name";
        public virtual string SCOREBOARD_PLAYTIME => "Playtime";
        public virtual string SCOREBOARD_KILLS => "Kills";
        public virtual string SCOREBOARD_ASSISTS => "Assists";
        public virtual string SCOREBOARD_DEATHS => "Deaths";
        public virtual string SCOREBOARD_TEAM => "Team";
        public virtual string SCOREBOARD_LOBBY => "Lobby";

        public virtual string BOMB_PLANTED => "The bomb got planted!";

        public virtual ILoginRegisterTexts LOGIN_REGISTER_TEXTS => new LoginRegisterTextsEnglish();

        public virtual ILobbyChoiceTexts LOBBY_CHOICE_TEXTS => new LobbyChoiceTextsEnglish();

        public virtual IMapCreatorMenuTexts MAPCREATOR_MENU => new MapCreatorMenuTextsEnglish();

        public virtual IOrderTexts ORDER => new OrderTextsEnglish();

        public ELanguage Enum => ELanguage.English;
    }
}
