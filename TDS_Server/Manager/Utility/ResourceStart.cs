namespace TDS_Server.Manager.Utility
{

    using System;
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
                    dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                    await SettingsManager.Load(dbcontext);
                    CommandsManager.LoadCommands(dbcontext);

                    NAPI.Server.SetGamemodeName(SettingsManager.GamemodeName);

                    BansManager.RemoveExpiredBans(dbcontext);
                    //await Maps.LoadMaps();
                    await LobbyManager.LoadAllLobbies();


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
