using TDS_Client.Interface;
using TDS_Common.Enum;

namespace TDS_Client.Instance.Language
{
    internal class German : English
    {
        public override string DEFUSING => "Entschärfung ...";
        public override string PLANTING => "Lege Bombe ...";
        public override string OUTSIDE_MAP_LIMIT => "Du bist außerhalb der Map-Grenze!\nDir bleiben noch {0} Sekunden, um zurück zu gehen.";
        public override string OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME => "Du bist außerhalb der Map-Grenze!\nWenn du nicht zurück gehst, wirst du in {0} Sekunden sterben.";
        public override string OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME => "Du bist außerhalb der Map-Grenze!\nWenn du nicht zurück gehst, wirst du in {0} Sekunden zurück teleportiert.";

        public override string SCOREBOARD_NAME => "Name";
        public override string SCOREBOARD_PLAYTIME => "Spielzeit";
        public override string SCOREBOARD_KILLS => "Kills";
        public override string SCOREBOARD_ASSISTS => "Assists";
        public override string SCOREBOARD_DEATHS => "Tode";
        public override string SCOREBOARD_TEAM => "Team";
        public override string SCOREBOARD_LOBBY => "Lobby";
        public override string BOMB_PLANTED => "Die Bombe wurde platziert!";

        public override string DIRECTION => "Richtung";
        public override string FASTER => "Schneller";
        public override string SLOWER => "Langsamer";
        public override string FAST_MODE => "Schneller-Modus (3x)";
        public override string SLOW_MODE => "Langsamer-Modus (0.5x)";
        public override string DOWN => "Runter";
        public override string UP => "Hoch";
        public override string LEFT_SHIFT => "LShift";
        public override string LEFT_CTRL => "LSTRG";
        public override string DELETE_KEY => "Entf";
        public override string DELETE_DESCRIPTION => "Entfernen";
        public override string END_KEY => "Ende";
        public override string OBJECT_MODEL_INVALID => "Objekt Modell ist ungültig.";
        public override string COULD_NOT_LOAD_OBJECT => "Konnte Objekt nicht laden.";
        public override string FREECAM => "Freecam";
        public override string ON_FOOT => "Zu Fuß";
        public override string PUT_ON_GROUND => "Boden legen";
        public override string LET_IT_FLOAT => "Schweben lassen";
        public override string ERROR => "Error aufgetaucht, bitte melde es: '{0}'";

        public override ILoginRegisterTexts LOGIN_REGISTER_TEXTS => new LoginRegisterTextsGerman();

        public override ELanguage Enum => ELanguage.German;
    }
}