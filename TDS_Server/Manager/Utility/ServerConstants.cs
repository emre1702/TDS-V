using GTANetworkAPI;

namespace TDS_Server.Manager.Utility
{
    class ServerConstants
    {
        private const string _resourcePathPath = "dotnet/resources/tds/";
        public const string MapsPath = _resourcePathPath + "maps/";
        public const string NewMapsPath = _resourcePathPath + "newmaps/";
        public const string SavedMapsPath = _resourcePathPath + "savedmaps/";
        public const string NeedCheckMapsPath = _resourcePathPath + "needcheckmaps/";
        public static uint TargetHash => NAPI.Util.GetHashKey("v_ret_ta_skull");
    }
}
