using TDS_Client.Instance.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Common.Dto.Map;
using TDS_Common.Enum;

namespace TDS_Client.Manager.Lobby
{
    internal static class MapLimitManager
    {
        private static MapLimit currentMapLimit;

        public static void Load(Position4DDto[] edges)
        {
            currentMapLimit?.Stop();
            currentMapLimit = new MapLimit(edges, Settings.MapLimitType);
        }

        public static void Start()
        {
            currentMapLimit?.Start();
        }

        public static void Stop()
        {
            currentMapLimit?.Stop();
        }
    }
}