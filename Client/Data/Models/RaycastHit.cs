using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Models
{
    public class RaycastHit
    {
        public int Ray = -1;
        public bool Hit = false;
        public Position3D EndCoords = new Position3D();
        public Position3D SurfaceNormal = new Position3D();
        public int EntityHit = -1;
        public int MaterialHash = -1;
        public int ShapeResult = -1;
    }
}
