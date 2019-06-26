using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Stats;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Utility
{
    internal class ResourceStop : Script
    {
        [ServerEvent(Event.ResourceStop)]
        public void OnResourceStop()
        {
            SaveAllInDatabase();
            RemoveAllCreated();
        }

        private async void SaveAllInDatabase()
        {
            Client? exceptionsource = null;
            try
            {
                await LogsManager.Save();
                await ServerTotalStatsManager.Save();
                await ServerDailyStatsManager.Save();

                List<Client> players = NAPI.Pools.GetAllPlayers();
                foreach (Client player in players)
                {
                    exceptionsource = player;
                    await player.GetChar().SaveData();
                }
                exceptionsource = null;
            }
            catch (Exception ex)
            {
                if (exceptionsource == null)
                    ErrorLogsManager.Log(ex.Message, Environment.StackTrace);
                else
                    ErrorLogsManager.Log(ex.Message, Environment.StackTrace, exceptionsource);
            }
        }

        private void RemoveAllCreated()
        {
            List<Blip> blips = NAPI.Pools.GetAllBlips();
            foreach (Blip blip in blips)
                NAPI.Entity.DeleteEntity(blip);

            List<Marker> markers = NAPI.Pools.GetAllMarkers();
            foreach (Marker marker in markers)
                NAPI.Entity.DeleteEntity(marker);

            List<Pickup> pickups = NAPI.Pools.GetAllPickups();
            foreach (Pickup pickup in pickups)
                NAPI.Entity.DeleteEntity(pickup);

            List<Vehicle> vehicles = NAPI.Pools.GetAllVehicles();
            foreach (Vehicle vehicle in vehicles)
                NAPI.Entity.DeleteEntity(vehicle);

            List<GTANetworkAPI.Object> objects = NAPI.Pools.GetAllObjects();
            foreach (GTANetworkAPI.Object obj in objects)
                NAPI.Entity.DeleteEntity(obj);
        }
    }
}