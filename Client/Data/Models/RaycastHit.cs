using RAGE;

namespace TDS_Client.Data.Models
{
    public class RaycastHit
    {
        #region Public Fields

        public Vector3 EndCoords = new Vector3();
        public int EntityHit = -1;
        public bool Hit = false;
        public int Ray = -1;
        public int ShapeResult = -1;
        public Vector3 SurfaceNormal = new Vector3();

        #endregion Public Fields
    }
}
