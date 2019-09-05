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
            NAPI.World.SetWeather(Weather.CLEAR);
            LoadAll();
        }

        private async void LoadAll()
        {
            try
            {
                SettingsManager.LoadLocal();

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

                ServerDailyStatsManager.Init();
                ServerTotalStatsManager.Init();

                await SettingsManager.Load(dbcontext);

                await AdminsManager.Init(dbcontext);
                Workaround.Init();
                await CommandsManager.LoadCommands(dbcontext);
                Damagesys.LoadDefaults(dbcontext);

                await BansManager.RemoveExpiredBans();

                await MapsLoader.LoadDefaultMaps(dbcontext);
                await MapCreator.LoadNewMaps(dbcontext);
                await MapCreator.LoadSavedMaps(dbcontext);

                Normal.Init(dbcontext);
                Bomb.Init(dbcontext);
                Sniper.Init(dbcontext);
                
                await LobbyManager.LoadAllLobbies(dbcontext);
                await Gang.LoadAll(dbcontext);

                Userpanel.Main.Init(dbcontext);

                ResourceStarted = true;
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }

        [ServerEvent(Event.FirstChanceException)]
        private static void OnFirstChanceException(Exception ex)
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
    }
}