using System.Collections.Generic;
using System.Drawing;
using TDS_Client.Instance.Lobby;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.Map;
using TDS_Shared.Dto.Map;
using TDS_Shared.Enum;

namespace TDS_Client.Manager.Utility
{
    static class ForceStayAtPos
    {
        private static MapLimit _mapLimit;

        public static void Start(Position3DDto pos, float radius, MapLimitType type, int allowedTimeOut = 0)
        {
            _mapLimit?.Stop();

            var edges = new List<Position3DDto>
            {
                new Position3DDto { X = pos.X - radius, Y = pos.Y, Z = pos.Z },  // left 
                new Position3DDto { X = pos.X - radius/2, Y = pos.Y - radius/2, Z = pos.Z },  // left top
                new Position3DDto { X = pos.X, Y = pos.Y - radius, Z = pos.Z },  // top 
                new Position3DDto { X = pos.X + radius/2, Y = pos.Y - radius/2, Z = pos.Z },  // top right 
                new Position3DDto { X = pos.X + radius, Y = pos.Y, Z = pos.Z },  // right 
                new Position3DDto { X = pos.X + radius/2, Y = pos.Y + radius/2, Z = pos.Z },  // right bottom 
                new Position3DDto { X = pos.X, Y = pos.Y + radius, Z = pos.Z },  // bottom 
                new Position3DDto { X = pos.X - radius/2, Y = pos.Y + radius/2, Z = pos.Z },  // bottom left  
            };
            _mapLimit = new MapLimit(edges, type, allowedTimeOut, Color.FromArgb(30, 255, 255, 255));
            _mapLimit.Start();
        }

        public static void Stop()
        {
            _mapLimit?.Stop();
            _mapLimit = null;
        }
    }
}
