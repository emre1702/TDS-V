using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using TDS_Server.Database.Entity;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler
{
    public class FreeroamDataHandler
    {
        private readonly Dictionary<FreeroamVehicleType, VehicleHash> _defaultVehicleHashByType;

        public FreeroamDataHandler(TDSDbContext dbContext)
        {
            _defaultVehicleHashByType = dbContext.FreeroamDefaultVehicle.ToDictionary(e => e.VehicleType, e => e.VehicleHash);
        }

        public VehicleHash? GetDefaultHash(FreeroamVehicleType type)
        {
            lock (_defaultVehicleHashByType)
            {
                return _defaultVehicleHashByType.TryGetValue(type, out var hash) ? hash : (VehicleHash?)null;
            }
        }
    }
}
