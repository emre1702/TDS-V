using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TDS_Server.Instance;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Manager.Commands;
using TDS_Server.Manager.Mapping;
using TDS_Server.Manager.Maps;
using TDS_Server.Manager.Stats;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;
using Z.EntityFramework.Plus;

namespace TDS_Server.Manager.Utility
{
    internal class ResourceStart : Script
    {
        public ResourceStart()
        {
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetGlobalServerChat(false);
            LoadAll();
        }

        private async void LoadAll()
        {
            try
            {
                SettingsManager.LoadLocal();

                using var dbcontext = new TDSNewContext(SettingsManager.ConnectionString);

                dbcontext.PlayerStats.Where(s => s.LoggedIn).Update(s => new PlayerStats { LoggedIn = false });
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
                await LobbyManager.LoadAllLobbies(dbcontext);
                await Gang.LoadAll(dbcontext);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }
    }
}