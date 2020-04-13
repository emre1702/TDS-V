using RAGE;
using TDS_Shared.Data.Models.GTA;
using static RAGE.Ui.Cursor;

namespace TDS_Client.RAGEAPI.Extensions
{
    internal static class PositionExtensions
    {
        public static Vector3 ToVector3(this Position3D pos)
        {
            return new Vector3(pos.X, pos.Y, pos.Z);
        }

        public static Vector2 ToVector2(this Position2D pos)
        {
            return new Vector2(pos.X, pos.Y);
        }

        public static Position3D ToPosition3D(this Vector3 pos)
        {
            return new Position3D(pos.X, pos.Y, pos.Z);
        }

        public static Position2D ToPosition2D(this Vector2 pos)
        {
            return new Position2D(pos.X, pos.Y);
        }


    }
}
