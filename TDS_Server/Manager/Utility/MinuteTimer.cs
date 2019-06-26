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
    internal class MinuteTimer
    {
        private static int _counter = 0;

        public static async void Execute()
        {
            // int currenttick = Environment.TickCount;
            ++_counter;

            try
            {
                SavePlayers();

                await ServerTotalStatsManager.Save();
                await ServerDailyStatsManager.Save();

                // log-save //
                if (_counter % SettingsManager.SaveLogsCooldownMinutes == 0)
                {
                    await LogsManager.Save();
                }

                if (_counter % SettingsManager.SaveSeasonsCooldownMinutes == 0)
                {
                    //Season.SaveSeason();
                }
            }
            catch (Exception ex)
            {
                ErrorLogsManager.Log(ex.Message, Environment.StackTrace);
            }
        }

        private static void SavePlayers()
        {
            foreach (var player in Player.Player.GetAllTDSPlayer())
            {
                try
                {
                    ++player.PlayMinutes;
                    if (player.MuteTime.HasValue && player.MuteTime > 0)
                        --player.MuteTime;
                    player.CheckSaveData();
                }
                catch (Exception ex)
                {
                    ErrorLogsManager.Log(ex.Message, Environment.StackTrace, player);
                }
            }
        }
    }
}