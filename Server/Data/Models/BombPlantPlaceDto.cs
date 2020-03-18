using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.MapObject;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Models
{
    public class BombPlantPlaceDto
    {
        public IMapObject Object;
        public IBlip Blip;
        public Position3D Position;

        public BombPlantPlaceDto(IMapObject obj, IBlip blip, Position3D pos) => (Object, Blip, Position) = (obj, blip, pos);

        public void Delete()
        {
            Object.Delete();
            Blip.Delete();
        }
    }
}
