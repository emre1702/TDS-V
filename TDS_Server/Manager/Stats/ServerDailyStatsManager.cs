﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.EventManager;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Server;

namespace TDS_Server.Manager.Stats
{
    class ServerDailyStatsManager
    {
        #nullable disable warnings
        public static TDSDbContext DbContext { get; private set; }
        public static ServerDailyStats Stats { get; private set; }
        #nullable restore warnings

        public static void Init()
        {
            DbContext = new TDSDbContext();
            Stats = DbContext.ServerDailyStats.FirstOrDefault(s => s.Date.Date == DateTime.Today);
            if (Stats is null)
            {
                Stats = new ServerDailyStats { Date = DateTime.Today };
                DbContext.ServerDailyStats.Add(Stats);
                DbContext.SaveChanges();
            }

            CustomEventManager.OnPlayerLoggedIn += PlayerLoggedIn;
            CustomEventManager.OnPlayerLoggedOut += CheckPlayerPeak;
            CustomEventManager.OnPlayerRegistered += PlayerRegistered;
        }

        private static async Task CheckNewDay()
        {
            if (Stats.Date.Date == DateTime.Today)
                return;
            DbContext.Entry(Stats).State = EntityState.Detached;
            Stats = new ServerDailyStats { Date = DateTime.Today };
            DbContext.ServerDailyStats.Add(Stats);
            await DbContext.SaveChangesAsync();
        }

        public static async void AddArenaRound(ERoundEndReason roundEndReason, bool isOfficial)
        {
            if (roundEndReason == ERoundEndReason.Command
                || roundEndReason == ERoundEndReason.Empty
                || roundEndReason == ERoundEndReason.NewPlayer
                || roundEndReason == ERoundEndReason.Error)
                return;
            await CheckNewDay();
            if (isOfficial)
                ++Stats.ArenaRoundsPlayed;
            else
                ++Stats.CustomArenaRoundsPlayed;
        }

        private static async void PlayerLoggedIn(TDSPlayer player)
        {
            await CheckNewDay();
            ++Stats.AmountLogins;
            CheckPlayerPeak(player);
        }

        private static async void CheckPlayerPeak(TDSPlayer _)
        {
            if (PlayerManager.PlayerManager.AmountLoggedInPlayers <= Stats.PlayerPeak)
                return;
            await CheckNewDay();
            Stats.PlayerPeak = (short)PlayerManager.PlayerManager.AmountLoggedInPlayers;
        }

        private static async void PlayerRegistered(Player _)
        {
            await CheckNewDay();
            ++Stats.AmountRegistrations;
        }

        public static async Task Save()
        {
            await DbContext.SaveChangesAsync();
            await CheckNewDay();
        }
    }
}
