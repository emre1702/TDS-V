﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TDS_Server.Instance;
using TDS_Server.Instance.GangTeam;
using TDS_Server.Manager.Commands;
using TDS_Server.Manager.Maps;
using TDS_Server_DB.Entity;
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
                await dbcontext.SaveChangesAsync();
                dbcontext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                await SettingsManager.Load(dbcontext);

                await AdminsManager.Init(dbcontext);
                Workaround.Init();
                await CommandsManager.LoadCommands(dbcontext);
                Damagesys.LoadDefaults(dbcontext);

                NAPI.Server.SetGamemodeName(SettingsManager.GamemodeName);

                await BansManager.RemoveExpiredBans(dbcontext);

                await MapsLoader.LoadDefaultMaps(dbcontext);
                await MapCreator.LoadNewMaps(dbcontext);
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