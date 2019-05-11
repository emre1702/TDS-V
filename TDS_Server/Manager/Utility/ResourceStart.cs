using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TDS_Server.Instance;
using TDS_Server.Manager.Commands;
using TDS_Server.Manager.Maps;
using TDS_Server_DB.Entity;

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
                foreach (var stat in await dbcontext.PlayerStats.Where(s => s.LoggedIn).ToListAsync())
                {
                    stat.LoggedIn = false;
                }
                await dbcontext.SaveChangesAsync();

                await SettingsManager.Load(dbcontext);

                await AdminsManager.Init(dbcontext);
                Workaround.Init();
                await CommandsManager.LoadCommands(dbcontext);
                Damagesys.LoadDefaults(dbcontext);

                NAPI.Server.SetGamemodeName(SettingsManager.GamemodeName);

                await BansManager.RemoveExpiredBans(dbcontext);

                await MapsLoader.LoadMaps(dbcontext);
                MapCreator.LoadNewMaps();
                await LobbyManager.LoadAllLobbies(dbcontext);
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }
    }
}