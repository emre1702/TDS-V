using GTANetworkAPI;
using System;
using TDS.Shared.Data.Extensions;
using TDS.Shared.Data.Utility;

namespace TDS.Server.Data.Extensions
{
    public static class Vector3Extensions
    {
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

        public static Vector3 AddToZ(this Vector3 pos, float z)
            => new Vector3(pos.X, pos.Y, pos.Z + z);
    }
}
