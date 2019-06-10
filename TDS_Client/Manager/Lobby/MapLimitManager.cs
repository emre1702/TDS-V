using TDS_Client.Instance.Lobby;
using TDS_Common.Dto.Map;

namespace TDS_Client.Manager.Lobby
{
    internal static class MapLimitManager
    {
        private static MapLimit currentMapLimit;

        public static void Load(Position4DDto[] edges)
        {
            currentMapLimit?.Remove();
            currentMapLimit = new MapLimit(edges);
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