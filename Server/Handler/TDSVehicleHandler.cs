using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Handler.Entities.Vehicle;

namespace TDS_Server.Handler
{
    public class TDSVehicleHandler
    {
        private readonly ConcurrentDictionary<IVehicle, ITDSVehicle> _tdsVehicleCache = new ConcurrentDictionary<IVehicle, ITDSVehicle>();

        private readonly IServiceProvider _serviceProvider;

        public TDSVehicleHandler(IServiceProvider serviceProvider) 
        { 
            _serviceProvider = serviceProvider;
        }

        public ITDSVehicle Get(IVehicle modVehicle)
        {
            if (!_tdsVehicleCache.TryGetValue(modVehicle, out ITDSVehicle? tdsVehicle))
            {
                tdsVehicle = ActivatorUtilities.CreateInstance<TDSVehicle>(_serviceProvider, modVehicle);
                _tdsVehicleCache[modVehicle] = tdsVehicle;
            }

            return tdsVehicle;
        }

        public void Remove(IVehicle modVehicle)
        {
            _tdsVehicleCache.TryRemove(modVehicle, out _);
        }
    }
}
