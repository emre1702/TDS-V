using TDS_Client.Interface;

namespace TDS_Client.Instance.Language
{
    class German : English
    {
        public override string DEFUSING => "Entschärfung ...";
        public override string PLANTING => "Lege Bombe ...";
        public override string OUTSIDE_MAP_LIMIT => "Du bist außerhalb der Map-Grenze!\nDir bleiben noch {1} Sekunden, um zurück zu gehen.";

        public override string SCOREBOARD_NAME => "Name";
        public override string SCOREBOARD_PLAYTIME => "Spielzeit";
        public override string SCOREBOARD_KILLS => "Kills";
        public override string SCOREBOARD_ASSISTS => "Assists";
        public override string SCOREBOARD_DEATHS => "Tode";
        public override string SCOREBOARD_TEAMS => "Team";
        public override string SCOREBOARD_LOBBY => "Lobby";

        public override ILoginRegisterTexts LOGIN_REGISTER_TEXTS => new LoginRegisterTextsGerman();

        public override ILobbyChoiceTexts LOBBY_CHOICE_TEXTS => new LobbyChoiceTextsGerman();

        public override IMapCreatorMenuTexts MAPCREATOR_MENU => new MapCreatorMenuTextsGerman();

        public override IOrderTexts ORDER => new OrderTextsGerman();
    }
}
