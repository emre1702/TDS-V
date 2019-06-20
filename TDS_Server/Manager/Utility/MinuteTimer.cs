using GTANetworkAPI;
using System;
using System.Collections.Generic;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Player;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Utility
{
    internal class MinuteTimer
    {
        private static int _counter = 0;

        public static async void Execute()
        {
            Client? exceptionsource = null;
            // int currenttick = Environment.TickCount;
            ++_counter;

            try
            {
                #region Save player data

                List<Client> clients = NAPI.Pools.GetAllPlayers();
                foreach (Client client in clients)
                {
                    exceptionsource = client;
                    if (!client.Exists)
                        continue;
                    TDSPlayer player = client.GetChar();
                    if (!player.LoggedIn)
                        continue;
                    ++player.PlayMinutes;
                    if (player.MuteTime.HasValue && player.MuteTime > 0)
                        --player.MuteTime;
                    player.CheckSaveData();
                }

                #endregion Save player data

                exceptionsource = null;

                #region Save logs

                // log-save //
                if (_counter % SettingsManager.SaveLogsCooldownMinutes == 0)
                {
                    await LogsManager.Save();
                }

                #endregion Save logs

                if (_counter % SettingsManager.SaveSeasonsCooldownMinutes == 0)
                {
                    //Season.SaveSeason();
                }
            }
            catch (Exception ex)
            {
                if (exceptionsource == null)
                    ErrorLogsManager.Log(ex.Message, Environment.StackTrace);
                else
                    ErrorLogsManager.Log(ex.Message, Environment.StackTrace, exceptionsource);
            }
        }
    }
}