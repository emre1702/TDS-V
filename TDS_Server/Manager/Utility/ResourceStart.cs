using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Linq;
using TDS_Server.Instance;
using TDS_Server.Instance.GameModes;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Commands;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Player;
using TDS_Server.Manager.Stats;
using TDS_Server_DB.Entity;

namespace TDS_Server.Manager.Utility
{
    internal class ResourceStart : Script
    {
        public static bool ResourceStarted { get; private set; }

        public ResourceStart()
        {
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetGlobalServerChat(false);
            var date = DateTime.UtcNow;
            NAPI.World.SetTime(date.Hour, date.Minute, date.Second);
            LoadAll();
        }

        private async void LoadAll()
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                SettingsManager.LoadLocal();
                ClothesManager.Init();

                using var dbcontext = new TDSNewContext(SettingsManager.ConnectionString);
                dbcontext.Database.Migrate();
                var connection = (NpgsqlConnection)dbcontext.Database.GetDbConnection();
                connection.Open();
                connection.ReloadTypes();

                var playerStats = await dbcontext.PlayerStats.Where(s => s.LoggedIn).ToListAsync();
                foreach (var stat in playerStats)
                {
                    stat.LoggedIn = false;
                }
                await dbcontext.SaveChangesAsync();
                dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                HourTimer.Execute();

                ServerDailyStatsManager.Init();
                ServerTotalStatsManager.Init();

                await SettingsManager.Load(dbcontext);

                await AdminsManager.Init(dbcontext);
                Workaround.Init();
                await CommandsManager.LoadCommands(dbcontext);
                Damagesys.LoadDefaults(dbcontext);

                await BansManager.Get().RemoveExpiredBans();

                await MapsLoader.LoadDefaultMaps(dbcontext);
                await MapCreator.LoadNewMaps(dbcontext);
                await MapCreator.LoadSavedMaps(dbcontext);
                await MapCreator.LoadNeedCheckMaps(dbcontext);

                Normal.Init(dbcontext);
                Bomb.Init(dbcontext);
                Sniper.Init(dbcontext);
                
                await LobbyManager.LoadAllLobbies(dbcontext);
                await Gang.LoadAll(dbcontext);

                Userpanel.Main.Init(dbcontext);

                ResourceStarted = true;

                Account.Init();
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }

        [ServerEvent(Event.UnhandledException)]
        public void OnUnhandledException(Exception ex)
        {
            try
            {
                ErrorLogsManager.Log("Unhandled exception: " + ex.GetBaseException().Message, ex.StackTrace ?? Environment.StackTrace, (TDSPlayer?)null);
            }
            catch
            {
                // ignored
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                ErrorLogsManager.Log("CurrentDomain_UnhandledException: " 
                    +  ((Exception)e.ExceptionObject).GetBaseException().Message, 
                    ((Exception)e.ExceptionObject).StackTrace ?? Environment.StackTrace, 
                    (TDSPlayer?)null);
            }
            catch
            {
                // ignored
            }
        }
    }
}