using GTANetworkAPI;

namespace TDS_Server.Dto
{
    class BombPlantPlaceDto
    {
        public Object Object;
        public Blip Blip;
        public Vector3 Position;

        public BombPlantPlaceDto(Object obj, Blip blip, Vector3 pos) => (Object, Blip, Position) = (obj, blip, pos);

        public void Delete()
        {
            Object.Delete();
            Blip.Delete();
        }
    }
}
