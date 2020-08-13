using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Shapetest
{
    public interface IShapetestAPI
    {
        #region Public Methods

        int GetShapeTestResult(int ray, ref int curtemp, Position endCoords, Position surfaceNormal, ref int entityHit);

        int GetShapeTestResultEx(int ray, ref int curtemp, Position endCoords, Position surfaceNormal, ref int materialHash, ref int entityHit);

        int StartShapeTestRay(float x1, float y1, float z1, float x2, float y2, float z2, int flags, int ignoreEntity, int p8);

        #endregion Public Methods
    }
}
