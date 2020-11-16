using TDS.Client.Data.Interfaces;

namespace TDS.Client.Handler.Entities.Languages
{
    internal class MapCreatorMenuTextsEnglish : IMapCreatorMenuTexts
    {
        #region Public Properties

        public virtual string ADD => "Add";
        public virtual string BOMB => "Bomb";
        public virtual string BOMB_PLACES => "Bomb-spots";
        public virtual string BOMB_PLACES_MIN => "There has to be atleast one bomb-spot to be able to place the bomb.";
        public virtual string CHECK => "Check";
        public virtual string DESCRIPTIONS => "Descriptions";
        public virtual string GENERAL => "General";
        public virtual string GO => "Go";
        public virtual string LAST => "Last";

        public virtual string LAST_INFO => @"When you send the map, it will get saved on the server.<br>Then Bonus has the chance to load the map and test it.<br>
            If he find mistakes or problems, he will solve them.<br>At the end if the map is acceptable, he will make it playable.<br><br>
            This whole process can take a bit, so you have to wait atleast 1-2 days.<br>Please first use 'Check', then maybe improve the map and at the end press 'Send'";

        public virtual string MAP_EDGES => "Map-edges";
        public virtual string MAP_LIMIT => "Map-limit";
        public virtual string MAP_LIMIT_MIN_MAX => "The map-limit amount has to be 0 or min. {1} and max. {2}.";
        public virtual string MAP_MAX_PLAYERS_MIN_MAX => "Max-players has to be min. {1} and max. {2}.";
        public virtual string MAP_MIN_PLAYERS_MIN_MAX => "Min-players has to be min. {1} and max. {2}.";
        public virtual string MAP_NAME_ALREADY_USED => "The map name is already used!<br>Please change it and use 'Check' again.";
        public virtual string MAP_NAME_ATLEAST_CHARS => "The map name has to consist of atleast {1} chars.";
        public virtual string MAP_TEAM_SPAWNS_MIN_PER_TEAM => "The min. amount for spawns per team has to be atleast {1}.";
        public virtual string MAX_PLAYERS => "Max-players:";
        public virtual string MIN_PLAYERS => "Min-players:";
        public virtual string NORMAL => "Normal";
        public virtual string REMOVE => "Remove";
        public virtual string SEND => "Send";
        public virtual string TEAM => "Team";
        public virtual string TEAM_SPAWNS => "Team-spawns";

        #endregion Public Properties
    }
}
