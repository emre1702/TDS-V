using RAGE;
using System;
using TDS_Shared.Data.Extensions;
using TDS_Shared.Data.Models.Map.Creator;
using TDS_Shared.Data.Utility;

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

        public static Vector3 Around(this Vector3 pos, float distance, bool considerZ)
        {
            var newPos = new Vector3(pos.X, pos.Y, pos.Z);
            float addToX = SharedUtils.Rnd.NextFloat(-distance, distance);
            newPos.X += addToX;
            distance -= Math.Abs(addToX);

            if (distance == 0)
                return newPos;

            if (!considerZ)
            {
                newPos.Y += SharedUtils.GetRandom(true, false) ? distance : -distance;
                return newPos;
            }

            float addToY = SharedUtils.Rnd.NextFloat(-distance, distance);
            newPos.Y += addToY;
            distance -= addToY;

            if (distance == 0)
                return newPos;

            newPos.Z += SharedUtils.GetRandom(true, false) ? distance : -distance;

            return newPos;
        }
    }
}
