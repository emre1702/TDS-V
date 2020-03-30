using RAGE;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Client.Manager.Utility
{
    static class Raycasting
    {
        public class RaycastHit
        {
            public int Ray = -1;
            public bool Hit = false;
            public Vector3 EndCoords = new Vector3();
            public Vector3 SurfaceNormal = new Vector3();
            public int EntityHit = -1;
            public int MaterialHash = -1;
            public int ShapeResult = -1;
        }

        public static RaycastHit RaycastFromTo(Vector3 from, Vector3 to, int ignoreEntity, int flags)
        {
            int ray = RAGE.Game.Shapetest.StartShapeTestRay(from.X, from.Y, from.Z, to.X, to.Y, to.Z, flags, ignoreEntity, 0);
            RaycastHit cast = new RaycastHit();
            int curtemp = 0;
            cast.ShapeResult = RAGE.Game.Shapetest.GetShapeTestResultEx(ray, ref curtemp, cast.EndCoords, cast.SurfaceNormal, ref cast.MaterialHash, ref cast.EntityHit);
            cast.Hit = Convert.ToBoolean(curtemp);
            return cast;
        }
    }
}
