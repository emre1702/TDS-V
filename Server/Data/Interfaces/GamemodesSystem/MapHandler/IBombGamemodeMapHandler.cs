using System.Collections.Generic;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Models;

namespace TDS.Server.Data.Interfaces.GamemodesSystem.MapHandler
{
    public interface IBombGamemodeMapHandler : IBaseGamemodeMapHandler
    {
        List<BombPlantPlaceDto> BombPlantPlaces { get; }

        void CreateBombTakePickup(ITDSObject bomb);
    }
}
