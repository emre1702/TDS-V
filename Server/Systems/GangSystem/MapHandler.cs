using GTANetworkAPI;
using TDS.Server.Data.Interfaces.GangsSystem;

namespace TDS.Server.GangsSystem
{
    public class MapHandler : IGangMapHandler
    {
        public Vector3? SpawnPosition => _houseHandler.House?.Position;
        public float? SpawnHeading => _houseHandler.House?.SpawnRotation;

        private readonly IGangHouseHandler _houseHandler;

        public MapHandler(IGangHouseHandler houseHandler)
            => _houseHandler = houseHandler;
    }
}
