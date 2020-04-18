using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Server;
using TDS_Server.Handler;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;
using TDS_Server.Handler.Player;

namespace TDS_Server.Handler.Server
{
    public class ServerStatsHandler : DatabaseEntityWrapper
    {
        public ServerDailyStats DailyStats { get; private set; }
        public ServerTotalStats TotalStats { get; private set; }

        private readonly EventsHandler _eventsHandler;
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public ServerStatsHandler(EventsHandler eventsHandler, TDSPlayerHandler tdsPlayerHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler) 
            : base(dbContext, loggingHandler)
        {
            _eventsHandler = eventsHandler;
            _tdsPlayerHandler = tdsPlayerHandler;

            _eventsHandler.PlayerLoggedIn += PlayerLoggedIn;
            _eventsHandler.PlayerLoggedOut += CheckPlayerPeak;
            _eventsHandler.PlayerRegistered += PlayerRegistered;

            // Only to remove nullable warning
            DailyStats = new ServerDailyStats();
            TotalStats = new ServerTotalStats();

            ExecuteForDB(dbContext =>
            {
                DailyStats = dbContext.ServerDailyStats.FirstOrDefault(s => s.Date.Date == DateTime.UtcNow.Date);
                if (DailyStats is null)
                {
                    DailyStats = new ServerDailyStats { Date = DateTime.UtcNow.Date };
                    dbContext.ServerDailyStats.Add(DailyStats);
                    dbContext.SaveChanges();
                }

                TotalStats = dbContext.ServerTotalStats.First();
            }).Wait();

            _eventsHandler.Minute += Save;
        }

        private async ValueTask CheckNewDay()
        {
            if (DailyStats.Date.Date == DateTime.UtcNow.Date)
                return;

            await ExecuteForDBAsync(async dbContext =>
            {
                if (DailyStats.Date.Date == DateTime.UtcNow.Date)
                    return;
                dbContext.Entry(DailyStats).State = EntityState.Detached;
                DailyStats = new ServerDailyStats { Date = DateTime.UtcNow.Date };
                dbContext.ServerDailyStats.Add(DailyStats);
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
            {
                ++DailyStats.ArenaRoundsPlayed;
                ++TotalStats.ArenaRoundsPlayed;
            }
            else
            {
                ++DailyStats.CustomArenaRoundsPlayed;
                ++TotalStats.CustomArenaRoundsPlayed;
            }
                
        }

        private async void PlayerLoggedIn(ITDSPlayer player)
        {
            await CheckNewDay();
            ++DailyStats.AmountLogins;
            CheckPlayerPeak(player);
        }

        private async void CheckPlayerPeak(ITDSPlayer _)
        {
            await CheckNewDay();
            int amountLoggedIn = _tdsPlayerHandler.AmountLoggedInPlayers;
            if (amountLoggedIn > DailyStats.PlayerPeak)
            {
                DailyStats.PlayerPeak = (short)amountLoggedIn;
            }
            if (amountLoggedIn > TotalStats.PlayerPeak)
            {
                TotalStats.PlayerPeak = (short)amountLoggedIn;
            }
        }

        private async void PlayerRegistered(ITDSPlayer _, Players dbPlayer)
        {
            await CheckNewDay();
            ++DailyStats.AmountRegistrations;
        }

        public async void Save(int _)
        {
            await SaveTask();
        }

        public async Task SaveTask()
        {
            await ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync();
            });

            await CheckNewDay();
        }
    }
}
