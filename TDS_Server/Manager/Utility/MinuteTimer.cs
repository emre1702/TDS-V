namespace TDS_Server.Manager.Utility
{
    using GTANetworkAPI;
    using System;
    using System.Collections.Generic;
    using TDS_Server.Entity;
    using TDS_Server.Instance.Player;
    using TDS_Server.Manager.Logs;
    using TDS_Server.Manager.Player;

    internal class MinuteTimer
    {
        private static int counter = 0;

        public static async void Execute()
        {
            Client? exceptionsource = null;
            int currenttick = Environment.TickCount;
            ++counter;

            try
            {
                using (var dbcontext = new TDSNewContext())
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
                        await player.CheckSaveData(dbcontext);
                    }

                    #endregion Save player data

                    exceptionsource = null;

                    #region Save logs

                    // log-save //
                    if (counter % SettingsManager.SaveLogsCooldownMinutes == 0)
                    {
                        await AdminLogsManager.Save(dbcontext);
                        await ChatLogsManager.Save(dbcontext);
                        await ErrorLogsManager.Save(dbcontext);
                        await RestLogsManager.Save(dbcontext);
                    }

                    #endregion Save logs

                    if (counter % SettingsManager.SaveSeasonsCooldownMinutes == 0)
                    {
                        //Season.SaveSeason();
                    }
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