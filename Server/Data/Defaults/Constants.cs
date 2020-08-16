namespace TDS_Server.Data.Defaults
{
    public static class Constants
    {
        #region Public Fields

        public const string MapsPath = _resourcePathPath + "maps/";
        public const string NeedCheckMapsPath = _resourcePathPath + "needcheckmaps/";
        public const string NewMapsPath = _resourcePathPath + "newmaps/";
        public const string SavedMapsPath = _resourcePathPath + "savedmaps/";
        public static float ArenaHeadMultiplicator = 1.7f;
        public static string GangwarTargetObjectName = "v_ret_ta_skull";
        public static int RemoveTDSPlayerMinutesAfterLoggedOut = 5;

        #endregion Public Fields

        #region Private Fields

        private const string _resourcePathPath = "resources/tds/server/";

        #endregion Private Fields
    }
}
