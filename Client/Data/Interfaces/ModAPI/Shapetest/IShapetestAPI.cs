using System;
using System.Collections.Generic;
using System.Text;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Shapetest
{
    public interface IShapetestAPI
    {
        int StartShapeTestRay(float x1, float y1, float z1, float x2, float y2, float z2, int flags, int ignoreEntity, int p8);
        int GetShapeTestResultEx(int ray, ref int curtemp, Position3D endCoords, Position3D surfaceNormal, ref int materialHash, ref int entityHit);
    }
}
