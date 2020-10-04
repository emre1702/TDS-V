using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.RoundsHandlers.Datas;
using TDS_Server.Database.Entity;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Server;
using TDS_Server.Handler.Entities;
using TDS_Server.Handler.Events;

namespace TDS_Server.Handler.Server
{
    public class ServerStatsHandler : DatabaseEntityWrapper
    {
        private readonly EventsHandler _eventsHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        public ServerStatsHandler(EventsHandler eventsHandler, ITDSPlayerHandler tdsPlayerHandler, TDSDbContext dbContext, ILoggingHandler loggingHandler)
            : base(dbContext, loggingHandler)
        {
            _eventsHandler = eventsHandler;
            _tdsPlayerHandler = tdsPlayerHandler;

            _eventsHandler.PlayerLoggedIn += PlayerLoggedIn;
            _eventsHandler.PlayerLoggedOut += CheckPlayerPeak;
            _eventsHandler.PlayerRegistered += PlayerRegistered;
            _eventsHandler.LobbyCreated += EventsHandler_LobbyCreatedNew;

            // Only to remove nullable warning
            DailyStats = new ServerDailyStats();
            TotalStats = new ServerTotalStats();

            ExecuteForDB(dbContext =>
            {
                ServerDailyStats? dailyStats = dbContext.ServerDailyStats.FirstOrDefault(s => s.Date.Date == DateTime.UtcNow.Date);
                if (dailyStats is null)
                {
                    dailyStats = new ServerDailyStats { Date = DateTime.UtcNow.Date };
                    dbContext.ServerDailyStats.Add(dailyStats);
                    dbContext.SaveChanges();
                }
                DailyStats = dailyStats;
                TotalStats = dbContext.ServerTotalStats.First();
            }).Wait();

            _eventsHandler.Minute += Save;
        }

        private void EventsHandler_LobbyCreatedNew(IBaseLobby lobby)
        {
            if (lobby is IArena arena)
                arena.Events.RoundEndStats += () => CheckAddArenaRound(arena);
        }

        public ServerDailyStats DailyStats { get; private set; }
        public ServerTotalStats TotalStats { get; private set; }

        public async ValueTask CheckAddArenaRound(IArena lobby)
        {
            if (!lobby.CurrentRoundEndReason.AddToServerStats)
                return;
            await CheckNewDay();
            if (lobby.IsOfficial)
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

        private async void PlayerLoggedIn(ITDSPlayer player)
        {
            await CheckNewDay();
            ++DailyStats.AmountLogins;
            CheckPlayerPeak(player);
        }

        private async void PlayerRegistered(ITDSPlayer _, Players dbPlayer)
        {
            await CheckNewDay();
            ++DailyStats.AmountRegistrations;
        }
    }
}
