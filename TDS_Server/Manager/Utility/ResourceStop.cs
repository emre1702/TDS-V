using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Server.Entity;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Player;

namespace TDS_Server.Manager.Utility
{

    class ResourceStop : Script
    {

        [ServerEvent(Event.ResourceStop)]
        public void OnResourceStop()
        {
            SaveAllInDatabase();
            RemoveAllCreated();
        }

        private async void SaveAllInDatabase()
        {
            Client exceptionsource = null;
            try
            {
                using (var dbcontext = new TDSNewContext())
                {
                    await AdminLogsManager.Save(dbcontext);
                    await ChatLogsManager.Save(dbcontext);
                    await ErrorLogsManager.Save(dbcontext);
                    await RestLogsManager.Save(dbcontext);

                    List<Client> players = NAPI.Pools.GetAllPlayers();
                    foreach (Client player in players)
                    {
                        exceptionsource = player;
                        await player.GetChar().SaveData(dbcontext);
                    }
                    exceptionsource = null;

                    //Season.SaveSeason();
                }
            }
            catch (Exception ex)
            {
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
