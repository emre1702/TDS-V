namespace TDS_Server.Manager.Utility
{

    using System;
    using System.Linq;
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using TDS_Server.Entity;
    using TDS_Server.Instance;
    using TDS_Server.Manager.Commands;
    using TDS_Server.Manager.Maps;

    class ResourceStart : Script
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
                using var dbcontext = new TDSNewContext();
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
