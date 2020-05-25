using GTANetworkAPI;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Extensions
{
    internal static class PositionExtensions
    {
        #region Public Methods

        public static Vector3 ToMod(this Position3D pos)
        {
            return new Vector3(pos.X, pos.Y, pos.Z);
        }

        public static Position3D ToTDS(this Vector3 pos)
        {
            return new Position3D(pos.X, pos.Y, pos.Z);
        }

        #endregion Public Methods
    }
}
