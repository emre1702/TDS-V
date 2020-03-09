using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TDS_Common.Instance.Utility;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Manager.Stats;
using TDS_Server_DB.Entity;

namespace TDS_Server.Core.Manager.Utility
{
    class ResourceStop : Script
    {
        private static bool _resourceStopped = false;
        private static bool _isFirstResourceStopCheck = true;

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

        public static Task CheckHourForResourceRestart()
        {
            if (_isFirstResourceStopCheck)
            {
                _isFirstResourceStopCheck = false;
                return Task.FromResult(0);
            }

            if (DateTime.UtcNow.Hour != 5)
                return Task.FromResult(0);

            ResourceRestartCountdown(3, true);

            return Task.FromResult(0);
        }

        private static void ResourceRestartCountdown(int counter, bool isMinute)
        {
            if (isMinute)
            {
                if (--counter > 1)
                {
                    LangUtils.SendAllChatMessage(lang => "#o#" + string.Format(lang.RESOURCE_RESTART_INFO_MINUTES, counter));
                    _ = new TDSTimer(() => ResourceRestartCountdown(counter, true), 60 * 1000, 1);
                }
                else if (counter == 1)
                {
                    LangUtils.SendAllChatMessage(lang => "#o#" + string.Format(lang.RESOURCE_RESTART_INFO_MINUTES, counter));
                    _ = new TDSTimer(() => ResourceRestartCountdown(counter, true), 45 * 1000, 1);
                }
                else
                {
                    LangUtils.SendAllChatMessage(lang => "#o#" + string.Format(lang.RESOURCE_RESTART_INFO_SECONDS, 15));
                    _ = new TDSTimer(() => ResourceRestartCountdown(10, false), 5000, 1);
                }
            }
            else // is second
            {
                if (--counter > 0)
                {
                    LangUtils.SendAllChatMessage(lang => "#o#" + string.Format(lang.RESOURCE_RESTART_INFO_SECONDS, 15));
                    _ = new TDSTimer(() => ResourceRestartCountdown(counter, false), 1000, 1, true);
                }
                else
                {
                    ExecuteResourceRestart();
                }
            }
        }


        private static void ExecuteResourceRestart()
        {
            if (DateTime.UtcNow.DayOfWeek == DayOfWeek.Monday)
            {
                using var dbContext = new TDSDbContext();
                ChallengeManager.ClearWeeklyChallenges(dbContext);
            }
            
            OnResourceStop();
            Process.GetCurrentProcess().Kill();
        }

        private static void SaveAllInDatabase()
        {
            try
            {
                LogsManager.Save().Wait();
                ServerTotalStatsManager.Save().Wait();
                ServerDailyStatsManager.Save().Wait();
                LobbyManager.SaveAll().Wait();

                foreach (TDSPlayer player in PlayerManager.PlayerManager.LoggedInPlayers)
                {
                    try
                    {
                        player.SaveData().Wait();
                    }
                    catch (Exception ex)
                    {
                        ErrorLogsManager.Log(ex.Message, Environment.StackTrace, player);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log(ex.Message, Environment.StackTrace);
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
