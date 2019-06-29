﻿using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Server;

namespace TDS_Server.Manager.Stats
{
    class ServerDailyStatsManager
    {
        public static TDSNewContext DbContext { get; private set; }
        public static ServerDailyStats Stats { get; private set; }

        public static void Init()
        {
            DbContext = new TDSNewContext();
            Stats = DbContext.ServerDailyStats.FirstOrDefault(s => s.Date.Date == DateTime.Today);
            if (Stats == null)
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
                || roundEndReason == ERoundEndReason.NewPlayer)
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
            if (Player.Player.AmountLoggedInPlayers <= Stats.PlayerPeak)
                return;
            await CheckNewDay();
            Stats.PlayerPeak = (short)Player.Player.AmountLoggedInPlayers;
        }

        private static async void PlayerRegistered(Client _)
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
