namespace TDS_Server.Manager.Utility
{

    using System;
    using System.Linq;
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using TDS_Server.Entity;
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
                    foreach (var stat in dbcontext.Playerstats.Where(s => s.LoggedIn))
                    {
                        stat.LoggedIn = false;
                    }
                    dbcontext.SaveChanges();

                    await SettingsManager.Load(dbcontext);
                    CommandsManager.LoadCommands(dbcontext);

                    NAPI.Server.SetGamemodeName(SettingsManager.GamemodeName);

                    await BansManager.RemoveExpiredBans(dbcontext);

                    using (var dbcontextwithmaps = new TDSNewContext())
                    {
                        await MapsManager.LoadMaps(dbcontextwithmaps);
                        await LobbyManager.LoadAllLobbies(dbcontextwithmaps);
                    }

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
