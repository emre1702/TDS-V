using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Rest;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.SeedData.Rest
{
    public static class FreeroamDefaultVehicleSeeds
    {
        public static ModelBuilder HasFreeroamDefaultVehicles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FreeroamDefaultVehicle>().HasData(
                new FreeroamDefaultVehicle { VehicleType = FreeroamVehicleType.Car, VehicleHash = VehicleHash.Pfister811 },
                new FreeroamDefaultVehicle { VehicleType = FreeroamVehicleType.Helicopter, VehicleHash = VehicleHash.Akula },
                new FreeroamDefaultVehicle { VehicleType = FreeroamVehicleType.Plane, VehicleHash = VehicleHash.Pyro },
                new FreeroamDefaultVehicle { VehicleType = FreeroamVehicleType.Bike, VehicleHash = VehicleHash.Hakuchou2 },
                new FreeroamDefaultVehicle { VehicleType = FreeroamVehicleType.Boat, VehicleHash = VehicleHash.Speeder2 }
            );
            return modelBuilder;
        }
    }
}
