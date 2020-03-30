using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;

namespace TDS_Server.RAGEAPI.Vehicle
{
    class Vehicle : Entity.Entity, IVehicle
    {
        internal readonly GTANetworkAPI.Vehicle _instance;

        public Vehicle(GTANetworkAPI.Vehicle instance) : base(instance)
            => _instance = instance;


        public List<ITDSEntity> Occupants
        {

            get
            {
                var modOccupants = _instance.Occupants;
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
                return modOccupants.Select(o =>
                {
                    if (o is GTANetworkAPI.Player player)
                    {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                        return (ITDSEntity)Init.GetTDSPlayerIfLoggedIn(player);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                    }
                    return null;

                })
                .Where(o => o is { })
                .ToList();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
            }
        }

        public int MaxOccupants => _instance.MaxOccupants;
    }
}
