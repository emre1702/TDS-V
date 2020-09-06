using RAGE;
using TDS_Shared.Data.Models.Map.Creator;

namespace TDS_Client.Data.Extensions
{
    public static class Vector3Extensions
    {
        public static Vector3 Copy(this Vector3 pos)
            => new Vector3(pos.X, pos.Y, pos.Z);

        public static Vector3 AddToZ(this Vector3 pos, float addToZ)
            => new Vector3(pos.X, pos.Y, pos.Z + addToZ);

        public static Vector3 GetPosFrom(this Vector3 pos, MapCreatorPosition otherPos)
        {
            pos.X = otherPos.PosX;
            pos.Y = otherPos.PosY;
            pos.Z = otherPos.PosZ;
            return pos;
        }

        public static Vector3 GetRotFrom(this Vector3 rot, MapCreatorPosition otherRot)
        {
            rot.X = otherRot.RotX;
            rot.Y = otherRot.RotY;
            rot.Z = otherRot.RotZ;
            return rot;
        }
    }
}
