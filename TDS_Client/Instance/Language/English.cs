using TDS_Client.Interface;
using TDS_Common.Enum;

namespace TDS_Client.Instance.Language
{
    internal class English : ILanguage
    {
        public virtual string DEFUSING => "Defusing ...";
        public virtual string PLANTING => "Planting ...";
        public virtual string OUTSIDE_MAP_LIMIT => "You are outside of the map!\nThere are {0} seconds left to return to the map.";
        public virtual string OUTSIDE_MAP_LIMIT_KILL_AFTER_TIME => "You're off the map!\nIf you don't go back, you will die in {0} seconds.";
        public virtual string OUTSIDE_MAP_LIMIT_TELEPORT_AFTER_TIME => "You're off the map!\nIf you do not go back, you will be teleported back in {0} seconds.";

        public virtual string SCOREBOARD_NAME => "Name";
        public virtual string SCOREBOARD_PLAYTIME => "Playtime";
        public virtual string SCOREBOARD_KILLS => "Kills";
        public virtual string SCOREBOARD_ASSISTS => "Assists";
        public virtual string SCOREBOARD_DEATHS => "Deaths";
        public virtual string SCOREBOARD_TEAM => "Team";
        public virtual string SCOREBOARD_LOBBY => "Lobby";

        public virtual string BOMB_PLANTED => "The bomb got planted!";

        public virtual string DIRECTION => "Direction";
        public virtual string FASTER => "Faster";
        public virtual string SLOWER => "Slower";
        public virtual string FAST_MODE => "Fast mode (3x)";
        public virtual string SLOW_MODE => "Slow mode (0.5x)";
        public virtual string DOWN => "Down";
        public virtual string UP => "Up";
        public virtual string LEFT_SHIFT => "LShift";
        public virtual string LEFT_CTRL => "LCTRL";
        public virtual string DELETE_KEY => "Delete";
        public virtual string DELETE_DESCRIPTION => "Delete";
        public virtual string END_KEY => "End";
        public virtual string OBJECT_MODEL_INVALID => "Object model is invalid.";
        public virtual string COULD_NOT_LOAD_OBJECT => "Could not load object.";
        public virtual string FREECAM => "Freecam";
        public virtual string ON_FOOT => "On Foot";
        public virtual string PUT_ON_GROUND => "Put on ground";
        public virtual string LET_IT_FLOAT => "Let it float";
        public virtual string ERROR => "Error occured, please report this: '{0}'";
        public virtual string AFK_KICK_INFO => "You were AFK and got kicked out of the lobby.";
        public virtual string AFK_KICK_WARNING => "You are AFK and will be kicked in {0} seconds!\nChange your position or shoot!";
        public virtual string FIRING_MODE => "Firing mode";

        public virtual ILoginRegisterTexts LOGIN_REGISTER_TEXTS => new LoginRegisterTextsEnglish();

        public virtual ELanguage Enum => ELanguage.English;
    }
}