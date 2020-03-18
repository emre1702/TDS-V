using TDS_Server.Data.Models.Map.Creator;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Extensions
{
    public static class PositionExtensions
    {

        public static Position3D ToPosition3D(this Position4DDto pos)
        {
            return new Position3D(pos.X, pos.Y, pos.Z);
        }

        public static Position3DDto SwitchNamespace(this TDS_Shared.Data.Models.Map.Position3DDto dto)
        {
            return new Position3DDto { X = dto.X, Y = dto.Y, Z = dto.Z };
        }

        public static TDS_Shared.Data.Models.Map.Position3DDto SwitchNamespace(this Position3DDto dto)
        {
            return new TDS_Shared.Data.Models.Map.Position3DDto { X = dto.X, Y = dto.Y, Z = dto.Z };
        }
    }
}
