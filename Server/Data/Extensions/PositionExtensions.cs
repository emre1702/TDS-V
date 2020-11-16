using GTANetworkAPI;
using TDS.Server.Data.Models.Map.Creator;
using TDS.Shared.Data.Models.GTA;

namespace TDS.Server.Data.Extensions
{
    public static class PositionExtensions
    {
        public static Position3DDto SwitchNamespace(this Position3D dto)
        {
            return new Position3DDto { X = dto.X, Y = dto.Y, Z = dto.Z };
        }

        public static Position3D SwitchNamespace(this Position3DDto dto)
        {
            return new Position3D { X = dto.X, Y = dto.Y, Z = dto.Z };
        }

        public static Position3D ToPosition3D(this Position4DDto pos)
        {
            return new Position3D(pos.X, pos.Y, pos.Z);
        }

        public static Vector3 ToVector3(this Position3D pos)
            => new Vector3(pos.X, pos.Y, pos.Z);

    }
}
