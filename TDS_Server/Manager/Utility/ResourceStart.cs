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
                using (var dbcontext = new TDSNewContext())
                {
                    foreach (var stat in dbcontext.PlayerStats.Where(s => s.LoggedIn))
                    {
                        stat.LoggedIn = false;
                    }
                    dbcontext.SaveChanges();

                    SettingsManager.Load(dbcontext);
                    AdminsManager.Init(dbcontext);
                    Workaround.Init();
                    await CommandsManager.LoadCommands(dbcontext);
                    Damagesys.LoadDefaults(dbcontext);

                    NAPI.Server.SetGamemodeName(SettingsManager.GamemodeName);

                    BansManager.RemoveExpiredBans(dbcontext);

                    await MapsManager.LoadMaps(dbcontext);
                    LobbyManager.LoadAllLobbies(dbcontext);

                    // Gang.LoadGangFromDatabase ();

                    // Season.LoadSeason ();
                }
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }
    }

}
