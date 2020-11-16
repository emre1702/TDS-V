using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Database.Entity;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Database.Entity.Server;
using TDS.Server.Handler.Entities;
using TDS.Server.Handler.Events;

namespace TDS.Server.Handler.Server
{
    public class ServerStatsHandler : DatabaseEntityWrapper
    {
        private readonly EventsHandler _eventsHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        public ServerStatsHandler(EventsHandler eventsHandler, ITDSPlayerHandler tdsPlayerHandler, TDSDbContext dbContext)
            : base(dbContext)
        {
            _eventsHandler = eventsHandler;
            _tdsPlayerHandler = tdsPlayerHandler;

            _eventsHandler.PlayerLoggedIn += PlayerLoggedIn;
            _eventsHandler.PlayerLoggedOut += CheckPlayerPeak;
            _eventsHandler.PlayerRegistered += PlayerRegistered;
            _eventsHandler.LobbyCreated += EventsHandler_LobbyCreated;

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

        private void EventsHandler_LobbyCreated(IBaseLobby lobby)
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
            await CheckNewDay().ConfigureAwait(false);
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
            try
            {
                await SaveTask().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public async Task SaveTask()
        {
            await ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            await CheckNewDay().ConfigureAwait(false);
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
                await dbContext.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        private async void CheckPlayerPeak(ITDSPlayer _)
        {
            try
            {
                await CheckNewDay().ConfigureAwait(false);
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
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private async void PlayerLoggedIn(ITDSPlayer player)
        {
            try
            {
                await CheckNewDay().ConfigureAwait(false);
                ++DailyStats.AmountLogins;
                CheckPlayerPeak(player);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        private async void PlayerRegistered(ITDSPlayer _, Players dbPlayer)
        {
            try
            {
                await CheckNewDay().ConfigureAwait(false);
                ++DailyStats.AmountRegistrations;
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }
    }
}
