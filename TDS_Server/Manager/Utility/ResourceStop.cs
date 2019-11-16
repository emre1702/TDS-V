using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Stats;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Utility
{
    class ResourceStop : Script
    {
        private static bool _resourceStopped = false;

        [ServerEvent(Event.ResourceStop)]
        public static void OnResourceStop()
        {
            if (_resourceStopped)
                return;
            _resourceStopped = true;
            SaveAllInDatabase();
            RemoveAllCreated();
        }

        public static void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            OnResourceStop();
        }

        private static async void SaveAllInDatabase()
        {
            TDSPlayer? exceptionsource = null;
            try
            {
                await LogsManager.Save();
                await ServerTotalStatsManager.Save();
                await ServerDailyStatsManager.Save();

                foreach (TDSPlayer player in Player.Player.LoggedInPlayers)
                {
                    exceptionsource = player;
                    await player.SaveData();
                }
                exceptionsource = null;
            }
            catch (Exception ex)
            {
                if (exceptionsource is null)
                    ErrorLogsManager.Log(ex.Message, Environment.StackTrace);
                else
                    ErrorLogsManager.Log(ex.Message, Environment.StackTrace, exceptionsource);
            }
        }

        private static void RemoveAllCreated()
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