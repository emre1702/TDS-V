using TDS_Client.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Handler.Entities.Languages
{
    internal class German : English
    {
        #region Public Properties

        public override string AFK_KICK_INFO => "Du warst AFK und wurdest aus der Lobby geworfen.";

        public override string AFK_KICK_WARNING => "Du bist AFK und wirst\nin {0} Sekunden gekickt!\nÄndere deine Position oder schieße!";

        public override string BOMB_PLANTED => "Die Bombe wurde platziert!";

        public override string COULD_NOT_LOAD_OBJECT => "Konnte Objekt nicht laden.";

        public override string COUNTDOWN_STARTED_NOTIFICATION => "Countdown gestartet";

        public override string DEFUSING => "Entschärfung ...";

        public override string DELETE_DESCRIPTION => "Entfernen";

        public override string DELETE_KEY => "Entf";

        public override string DIRECTION => "Richtung";

        public override string DOWN => "Runter";

        public override string END_KEY => "Ende";

        public override Language Enum => Language.German;

        public override string ERROR => "Fehler aufgetaucht, bitte melde es! Info: '{0}'";

        public override string FAST_MODE => "Schneller-Modus (3x)";

        public override string FASTER => "Schneller";

        public override string FIRING_MODE => "Feuermodus";

        public override string FREECAM => "Freecam";

        public override string GANG_LOBBY_FREE_HOUSE_DESCRIPTION => "Level {0} Haus";

        public override string LEFT_CTRL => "LSTRG";

        public override string LEFT_SHIFT => "LShift";

        public override string LET_IT_FLOAT => "Schweben lassen";

        public override ILoginRegisterTexts LOGIN_REGISTER_TEXTS => new LoginRegisterTextsGerman();

        public override string OBJECT_MODEL_INVALID => "Objekt Modell ist ungültig.";

        public override string ON_FOOT => "Zu Fuß";

        public override string OUTSIDE_MAP_LIMIT => "Du bist außerhalb der Map-Grenze!\nDir bleiben noch {0} Sekunden, um zurück zu gehen.";

        public override string OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME => "Du bist außerhalb der Map-Grenze!\nWenn du nicht zurück gehst, wirst du in {0} Sekunden sterben.";

        public override string OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME => "Du bist außerhalb der Map-Grenze!\nWenn du nicht zurück gehst, wirst du in {0} Sekunden zurück teleportiert.";

        public override string PLANTING => "Lege Bombe ...";

        public override string PUT_ON_GROUND => "Boden legen";

        public override string ROUND_ENDED_NOTIFICATION => "Runde zuende";

        public override string ROUND_INFOS => "Runden Infos";

        public override string ROUND_STARTED_NOTIFICATION => "Runde gestartet";

        public override string SCOREBOARD_ASSISTS => "Assists";

        public override string SCOREBOARD_DEATHS => "Tode";

        public override string SCOREBOARD_KILLS => "Kills";

        public override string SCOREBOARD_LOBBY => "Lobby";

        public override string SCOREBOARD_NAME => "Name";

        public override string SCOREBOARD_PLAYTIME => "Spielzeit";

        public override string SCOREBOARD_TEAM => "Team";

        public override string SLOW_MODE => "Langsamer-Modus (0.5x)";

        public override string SLOWER => "Langsamer";

        public override string UP => "Hoch";

        public override string[] WELCOME_MESSAGE => new string[] {
            "#n#Willkommen auf dem #b#Team Deathmatch Server#w#.",
            "#n#Für Ankündigungen, Support, Bug-Meldung usw.",
            "#n#bitte unseren Discord-Server nutzen:",
            "#n#discord.gg/ntVnGFt",
            "#n#Du kannst den Cursor mit #r#ENDE#w# umschalten.",
            "#n#Viel Spaß wünscht das #b#TDS-Team#w#!"
        };

        public override string YOU_DIED => "Du bist gestorben.";

        #endregion Public Properties
    }
}
