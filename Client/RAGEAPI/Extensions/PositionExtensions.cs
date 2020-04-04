using RAGE;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Extensions
{
    internal static class PositionExtensions
    {
        public static Vector3 ToVector3(this Position3D pos)
        {
            return new Vector3(pos.X, pos.Y, pos.Z);
        }

        public static Position3D ToPosition3D(this Vector3 pos)
        {
            return new Position3D(pos.X, pos.Y, pos.Z);
        }
    }
}
