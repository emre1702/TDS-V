using TDS_Client.Interface;
using TDS_Common.Enum;

namespace TDS_Client.Instance.Language
{
    internal class German : English
    {
        public override string DEFUSING => "Entschärfung ...";
        public override string PLANTING => "Lege Bombe ...";
        public override string OUTSIDE_MAP_LIMIT => "Du bist außerhalb der Map-Grenze!\nDir bleiben noch {1} Sekunden, um zurück zu gehen.";

        public override string SCOREBOARD_NAME => "Name";
        public override string SCOREBOARD_PLAYTIME => "Spielzeit";
        public override string SCOREBOARD_KILLS => "Kills";
        public override string SCOREBOARD_ASSISTS => "Assists";
        public override string SCOREBOARD_DEATHS => "Tode";
        public override string SCOREBOARD_TEAM => "Team";
        public override string SCOREBOARD_LOBBY => "Lobby";
        public override string BOMB_PLANTED => "Die Bombe wurde platziert!";

        public override ILoginRegisterTexts LOGIN_REGISTER_TEXTS => new LoginRegisterTextsGerman();

        public override ILobbyChoiceTexts LOBBY_CHOICE_TEXTS => new LobbyChoiceTextsGerman();

        public override IMapCreatorMenuTexts MAPCREATOR_MENU => new MapCreatorMenuTextsGerman();

        public new ELanguage Enum => ELanguage.German;
    }
}