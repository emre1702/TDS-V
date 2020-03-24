using System.Collections.Generic;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.RAGE.Startup;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGE.Vehicle
{
    class Vehicle : IVehicle
    {
        private readonly GTANetworkAPI.Vehicle _instance;

        public Vehicle(GTANetworkAPI.Vehicle instance)
            => _instance = instance;

        public ushort Id => _instance.Id;

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
                        return (ITDSEntity)Program.GetTDSPlayerIfLoggedIn(player);
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

        public Position3D Rotation { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public Position3D Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public uint Dimension { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void Delete()
        {
            _instance.Delete();
        }

        public bool Equals(IVehicle? other)
        {
            return Id == other?.Id;
        }
    }
}
