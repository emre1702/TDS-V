using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Models;

namespace TDS_Server.Data.Interfaces.GamemodesSystem.MapHandler
{
    public interface IBombGamemodeMapHandler : IBaseGamemodeMapHandler
    {
        List<BombPlantPlaceDto> BombPlantPlaces { get; }

        void CreateBombTakePickup(ITDSObject bomb);
    }
}
