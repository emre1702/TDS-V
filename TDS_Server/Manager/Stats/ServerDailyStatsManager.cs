﻿using Microsoft.EntityFrameworkCore;
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

        public static async Task Init()
        {
            DbContext = new TDSNewContext();
            Stats = await DbContext.ServerDailyStats.FirstOrDefaultAsync(s => s.Date == NpgsqlDate.Today);
            if (Stats == null)
            {
                Stats = new ServerDailyStats { Date = NpgsqlDate.Today };
                DbContext.ServerDailyStats.Add(Stats);
                await DbContext.SaveChangesAsync();
            }

            CustomEventManager.OnPlayerLoggedIn += CheckPlayerPeak;
            CustomEventManager.OnPlayerLoggedOut += CheckPlayerPeak;
        }

        public static void AddArenaRound(ERoundEndReason roundEndReason, bool isOfficial)
        {
            if (roundEndReason == ERoundEndReason.Command
                || roundEndReason == ERoundEndReason.Empty
                || roundEndReason == ERoundEndReason.NewPlayer)
                return;
            if (isOfficial)
                ++Stats.ArenaRoundsPlayed;
            else
                ++Stats.CustomArenaRoundsPlayed;
        }

        public static void CheckPlayerPeak(TDSPlayer _)
        {
            if (Player.Player.AmountLoggedInPlayers <= Stats.PlayerPeak)
                return;
            Stats.PlayerPeak = Player.Player.AmountLoggedInPlayers;
        }

        public static Task Save()
        {
            return DbContext.SaveChangesAsync();
        }
    }
}
