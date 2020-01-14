using System.Collections.Generic;
using TDS_Client.Dto.Map;
using TDS_Client.Instance.Lobby;
using TDS_Client.Manager.Utility;
using TDS_Server.Dto.Map;

namespace TDS_Client.Manager.Lobby
{
    internal static class MapLimitManager
    {
        private static MapLimit _currentMapLimit;

        public static void Load(List<Position3DDto> edges)
        {
            _currentMapLimit?.Stop();
            _currentMapLimit = new MapLimit(edges, Settings.MapLimitType, Settings.MapLimitTime, Settings.MapBorderColor);
        }

        public static void Start()
        {
            _currentMapLimit?.Start();
        }

        public static void Stop()
        {
            _currentMapLimit?.Stop();
        }
    }
}
