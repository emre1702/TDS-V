using TDS_Client.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Client.Handler.Entities.Languages
{
    internal class English : ILanguage
    {
        #region Public Properties

        public virtual string AFK_KICK_INFO => "You were AFK and got kicked out of the lobby.";

        public virtual string AFK_KICK_WARNING => "You are AFK and will\nbe kicked in {0} seconds!\nChange your position or shoot!";

        public virtual string BOMB_PLANTED => "The bomb got planted!";

        public virtual string COULD_NOT_LOAD_OBJECT => "Could not load object.";

        public virtual string COUNTDOWN_STARTED_NOTIFICATION => "Countdown started";

        public virtual string DEFUSING => "Defusing ...";

        public virtual string DELETE_DESCRIPTION => "Delete";

        public virtual string DELETE_KEY => "Delete";

        public virtual string DIRECTION => "Direction";

        public virtual string DOWN => "Down";

        public virtual string END_KEY => "End";

        public virtual Language Enum => Language.English;

        public virtual string ERROR => "Error occured, please report this: '{0}'";

        public virtual string FAST_MODE => "Fast mode (3x)";

        public virtual string FASTER => "Faster";

        public virtual string FIRING_MODE => "Firing mode";

        public virtual string FREECAM => "Freecam";

        public virtual string GANG_LOBBY_FREE_HOUSE_DESCRIPTION => "Level {0} house";

        public virtual string LEFT_CTRL => "LCTRL";

        public virtual string LEFT_SHIFT => "LShift";

        public virtual string LET_IT_FLOAT => "Let it float";

        public virtual ILoginRegisterTexts LOGIN_REGISTER_TEXTS => new LoginRegisterTextsEnglish();

        public virtual string OBJECT_MODEL_INVALID => "Object model is invalid.";

        public virtual string ON_FOOT => "On Foot";

        public virtual string OUTSIDE_MAP_LIMIT => "You are outside of the map!\nThere are {0} seconds left to return to the map.";

        public virtual string OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME => "You're off the map!\nIf you don't go back, you will die in {0} seconds.";

        public virtual string OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME => "You're off the map!\nIf you do not go back, you will be teleported back in {0} seconds.";

        public virtual string PLANTING => "Planting ...";

        public virtual string PUT_ON_GROUND => "Put on ground";

        public virtual string ROUND_ENDED_NOTIFICATION => "Round ended";

        public virtual string ROUND_INFOS => "Round infos";

        public virtual string ROUND_STARTED_NOTIFICATION => "Round started";

        public virtual string SCOREBOARD_ASSISTS => "Assists";

        public virtual string SCOREBOARD_DEATHS => "Deaths";

        public virtual string SCOREBOARD_KILLS => "Kills";

        public virtual string SCOREBOARD_LOBBY => "Lobby";

        public virtual string SCOREBOARD_NAME => "Name";

        public virtual string SCOREBOARD_PLAYTIME => "Playtime";

        public virtual string SCOREBOARD_TEAM => "Team";

        public virtual string SLOW_MODE => "Slow mode (0.5x)";

        public virtual string SLOWER => "Slower";

        public virtual string UP => "Up";

        public virtual string[] WELCOME_MESSAGE => new string[] {
            "#n#Welcome to #b#Team Deathmatch Server#w#.",
            "#n#For announcements, support, bug-reports etc.",
            "#n#please visit our Discord-server:",
            "#n#discord.gg/ntVnGFt",
            "#n#You can get/hide the cursor with #r#END#w#.",
            "#n#Have fun on TDS#w#!"
        };

        public virtual string YOU_DIED => "You died.";

        #endregion Public Properties
    }
}
