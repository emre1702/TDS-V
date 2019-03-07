using GTANetworkAPI;

namespace TDS_Server.Dto
{
    class BombPlantPlaceDto
    {
        public Object Object;
        public Blip Blip;
        public Vector3 Position;

        public void Delete()
        {
            Object.Delete();
            Blip.Delete();
        }
    }
}
