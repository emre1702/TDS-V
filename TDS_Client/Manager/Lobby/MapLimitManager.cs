using RAGE;
using System.Collections.Generic;
using TDS_Client.Instance.Lobby;
using TDS_Common.Dto.Map;

namespace TDS_Client.Manager.Lobby
{
    static class MapLimitManager
    {
        private static MapLimit currentMapLimit;

        public static void Load(MapPositionDto[] edges)
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
