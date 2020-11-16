using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Models
{
#nullable enable

    public class BombPlantPlaceDto
    {

        public ITDSBlip? Blip { get; }
        public ITDSObject? Obj { get; set; }
        public Vector3 Position { get; }

        public BombPlantPlaceDto(ITDSObject? obj, ITDSBlip? blip, Vector3 pos) => (Obj, Blip, Position) = (obj, blip, pos);

        public void Delete()
        {
            Obj?.Delete();
            Blip?.Delete();
        }

    }
}
