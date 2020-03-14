using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.Object;
using TDS_Server.Data.Models.GTA;

namespace TDS_Server.Data.Models
{
    internal class BombPlantPlaceDto
    {
        public IObject Object;
        public IBlip Blip;
        public Position3D Position;

        public BombPlantPlaceDto(IObject obj, IBlip blip, Position3D pos) => (Object, Blip, Position) = (obj, blip, pos);

        public void Delete()
        {
            Object.Delete();
            Blip.Delete();
        }
    }
}
