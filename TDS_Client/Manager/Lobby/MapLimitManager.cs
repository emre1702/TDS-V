using RAGE;
using System.Collections.Generic;
using TDS_Client.Instance.Lobby;

namespace TDS_Client.Manager.Lobby
{
    static class MapLimitManager
    {
        private static MapLimit currentMapLimit;

        public static void Load(List<Vector3> edges)
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
