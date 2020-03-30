using GTANetworkAPI;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Extensions
{
    internal static class PositionExtensions
    {
        public static Vector3 ToVector3(this Position3D pos)
        {
            return new Vector3(pos.X, pos.Y, pos.Z);
        }
    }
}
