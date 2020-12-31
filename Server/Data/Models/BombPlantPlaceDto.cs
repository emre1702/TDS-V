using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Data.Models
{
#nullable enable

    public class BombPlantPlaceDto
    {
        public ITDSBlip? Blip { get; private set; }
        public ITDSObject? Obj { get; set; }
        public Vector3 Position { get; }

        public BombPlantPlaceDto(ITDSObject? obj, ITDSBlip? blip, Vector3 pos) => (Obj, Blip, Position) = (obj, blip, pos);

        public void Delete()
        {
            if (Obj is { })
                NAPI.Entity.DeleteEntity(Obj);
            if (Blip is { })
                NAPI.Entity.DeleteEntity(Blip);
            Obj = null;
            Blip = null;
        }
    }
}