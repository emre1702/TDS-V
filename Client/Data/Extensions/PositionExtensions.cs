using RAGE;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Extensions
{
    public static class PositionExtensions
    {
        public static Vector3 ToVector3(this Position3D pos)
            => new Vector3(pos.X, pos.Y, pos.Z);
    }
}
