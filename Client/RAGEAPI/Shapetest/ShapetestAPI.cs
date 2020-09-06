using TDS_Client.Data.Interfaces.RAGE.Game.Shapetest;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Shapetest
{
    internal class ShapetestAPI : IShapetestAPI
    {
        #region Public Methods

        public int GetShapeTestResult(int rayHandle, ref int hit, Position3D endCoords, Position3D surfaceNormal, ref int entityHit)
        {
            var endCoorsV = endCoords.ToVector3();
            var surfaceNormalV = surfaceNormal.ToVector3();
            var result = RAGE.Game.Shapetest.GetShapeTestResult(rayHandle, ref hit, endCoorsV, surfaceNormalV, ref entityHit);

            endCoords.X = endCoorsV.X;
            endCoords.Y = endCoorsV.Y;
            endCoords.Z = endCoorsV.Z;

            surfaceNormal.X = surfaceNormalV.X;
            surfaceNormal.Y = surfaceNormalV.Y;
            surfaceNormal.Z = surfaceNormalV.Z;

            return result;
        }

        public int GetShapeTestResultEx(int rayHandle, ref int hit, Position3D endCoords, Position3D surfaceNormal, ref int materialHash, ref int entityHit)
        {
            var endCoorsV = endCoords.ToVector3();
            var surfaceNormalV = surfaceNormal.ToVector3();
            var result = RAGE.Game.Shapetest.GetShapeTestResultEx(rayHandle, ref hit, endCoorsV, surfaceNormalV, ref materialHash, ref entityHit);

            endCoords.X = endCoorsV.X;
            endCoords.Y = endCoorsV.Y;
            endCoords.Z = endCoorsV.Z;

            surfaceNormal.X = surfaceNormalV.X;
            surfaceNormal.Y = surfaceNormalV.Y;
            surfaceNormal.Z = surfaceNormalV.Z;

            return result;
        }

        public int StartShapeTestRay(float x1, float y1, float z1, float x2, float y2, float z2, int flags, int ignoreEntity, int p8)
        {
            return RAGE.Game.Shapetest.StartShapeTestRay(x1, y1, z1, x2, y2, z2, flags, ignoreEntity, p8);
        }

        #endregion Public Methods
    }
}