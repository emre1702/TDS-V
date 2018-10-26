namespace TDS.Manager.Utility
{

    using System;
    using GTANetworkAPI;
    using Microsoft.EntityFrameworkCore;
    using TDS.Entity;
    using TDS.Manager.Lobby;
    using TDS.Manager.Maps;

    class ResourceStart : Script
    {

        public ResourceStart()
        {
            NAPI.Server.SetAutoRespawnAfterDeath(false);
            NAPI.Server.SetGlobalServerChat(false);
            this.LoadAll();
        }

        private async void LoadAll()
        {
            try
            {
                using (var dbcontext = new TDSNewContext())
                {
                    dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                    await Setting.Load(dbcontext);

                    NAPI.Server.SetGamemodeName(Setting.GamemodeName);

                    BansManager.RemoveExpiredBans(dbcontext);
                    await Maps.LoadMaps();
                    await LobbyManager.LoadAllLobbies(dbcontext);


                    // Gang.LoadGangFromDatabase ();

                    // Season.LoadSeason ();

                    dbcontext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                NAPI.Util.ConsoleOutput(ex.ToString());
            }
        }
    }

}
