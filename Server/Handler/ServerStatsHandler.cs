using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Server;
using TDS_Server.Handler;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Player;

namespace TDS_Server.Core.Manager.Stats
{
    public class ServerStatsHandler : DatabaseEntityWrapper
    {
        public ServerDailyStats Stats { get; private set; }

        private readonly EventsHandler _eventsHandler;
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public ServerStatsHandler(EventsHandler eventsHandler, TDSPlayerHandler tdsPlayerHandler, TDSDbContext dbContext, LoggingHandler loggingHandler) 
            : base(dbContext, loggingHandler)
        {
            _eventsHandler = eventsHandler;
            _tdsPlayerHandler = tdsPlayerHandler;

            _eventsHandler.PlayerLoggedIn += PlayerLoggedIn;
            _eventsHandler.PlayerLoggedOut += CheckPlayerPeak;
            _eventsHandler.PlayerRegistered += PlayerRegistered;

            // Only to remove nullable warning
            Stats = new ServerDailyStats();

            ExecuteForDB(dbContext =>
            {
                Stats = dbContext.ServerDailyStats.FirstOrDefault(s => s.Date.Date == DateTime.Today);
                if (Stats is null)
                {
                    Stats = new ServerDailyStats { Date = DateTime.Today };
                    dbContext.ServerDailyStats.Add(Stats);
                    dbContext.SaveChanges();
                }
            }).Wait();
        }

        public static void Init()
        {

        }

        private async Task CheckNewDay()
        {
            if (Stats.Date.Date == DateTime.Today)
                return;

            await ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Entry(Stats).State = EntityState.Detached;
                Stats = new ServerDailyStats { Date = DateTime.Today };
                dbContext.ServerDailyStats.Add(Stats);
                await dbContext.SaveChangesAsync();
            });

        }

        public async void AddArenaRound(RoundEndReason roundEndReason, bool isOfficial)
        {
            if (roundEndReason == RoundEndReason.Command
                || roundEndReason == RoundEndReason.Empty
                || roundEndReason == RoundEndReason.NewPlayer
                || roundEndReason == RoundEndReason.Error)
                return;
            await CheckNewDay();
            if (isOfficial)
                ++Stats.ArenaRoundsPlayed;
            else
                ++Stats.CustomArenaRoundsPlayed;
        }

        private async void PlayerLoggedIn(ITDSPlayer player)
        {
            await CheckNewDay();
            ++Stats.AmountLogins;
            CheckPlayerPeak(player);
        }

        private async void CheckPlayerPeak(ITDSPlayer _)
        {
            if (_tdsPlayerHandler.AmountLoggedInPlayers <= Stats.PlayerPeak)
                return;
            await CheckNewDay();
            Stats.PlayerPeak = (short)_tdsPlayerHandler.AmountLoggedInPlayers;
        }

        private async void PlayerRegistered(ITDSPlayer _)
        {
            await CheckNewDay();
            ++Stats.AmountRegistrations;
        }

        public async Task Save()
        {
            await ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync();
            });

            await CheckNewDay();
        }
    }
}
