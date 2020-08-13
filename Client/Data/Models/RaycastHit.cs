using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Models
{
    public class RaycastHit
    {
        #region Public Fields

        public Position EndCoords = new Position();
        public int EntityHit = -1;
        public bool Hit = false;
        public int Ray = -1;
        public int ShapeResult = -1;
        public Position SurfaceNormal = new Position();

        #endregion Public Fields
    }
}
