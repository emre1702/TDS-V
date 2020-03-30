using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Interfaces.ModAPI.Pool;

namespace TDS_Server.RAGEAPI.Pool
{
    class PoolAPI : IPoolAPI
    {
        public List<IPlayer> GetAllModPlayers()
        {
            var players = GTANetworkAPI.NAPI.Pools.GetAllPlayers();
            var newList = new List<IPlayer>();
            foreach (var player in players)
            {
                var modPlayer = Init.GetModPlayer(player);
                if (modPlayer is { })
                    newList.Add(modPlayer);
            }

            return newList;
        }

        public void RemoveAll()
        {
            List<GTANetworkAPI.Blip> blips = NAPI.Pools.GetAllBlips();
            foreach (GTANetworkAPI.Blip blip in blips)
                NAPI.Entity.DeleteEntity(blip);

            List<GTANetworkAPI.Marker> markers = NAPI.Pools.GetAllMarkers();
            foreach (GTANetworkAPI.Marker marker in markers)
                NAPI.Entity.DeleteEntity(marker);

            List<GTANetworkAPI.Pickup> pickups = NAPI.Pools.GetAllPickups();
            foreach (GTANetworkAPI.Pickup pickup in pickups)
                NAPI.Entity.DeleteEntity(pickup);

            List<GTANetworkAPI.Vehicle> vehicles = NAPI.Pools.GetAllVehicles();
            foreach (GTANetworkAPI.Vehicle vehicle in vehicles)
                NAPI.Entity.DeleteEntity(vehicle);

            List<GTANetworkAPI.Object> objects = NAPI.Pools.GetAllObjects();
            foreach (GTANetworkAPI.Object obj in objects)
                NAPI.Entity.DeleteEntity(obj);
        }
    }
}
