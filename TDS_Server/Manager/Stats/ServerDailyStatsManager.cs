using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.EventManager;
using TDS_Server_DB.Entity.Server;

namespace TDS_Server.Manager.Stats
{
    class ServerDailyStatsManager : EntityWrapperClass
    {
        
        private static ServerDailyStatsManager? _instance;

        public ServerDailyStats Stats { get; private set; }

        public ServerDailyStatsManager()
        {
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
            _instance = new ServerDailyStatsManager();

            CustomEventManager.OnPlayerLoggedIn += PlayerLoggedIn;
            CustomEventManager.OnPlayerLoggedOut += CheckPlayerPeak;
            CustomEventManager.OnPlayerRegistered += PlayerRegistered;
        }

        private static async Task CheckNewDay()
        {
            if (_instance is null)
                return;

            if (_instance.Stats.Date.Date == DateTime.Today)
                return;

            await _instance.ExecuteForDBAsync(async dbContext =>
            {
                dbContext.Entry(_instance.Stats).State = EntityState.Detached;
                _instance.Stats = new ServerDailyStats { Date = DateTime.Today };
                dbContext.ServerDailyStats.Add(_instance.Stats);
                await dbContext.SaveChangesAsync();
            });
           
        }

        public static async void AddArenaRound(ERoundEndReason roundEndReason, bool isOfficial)
        {
            if (_instance is null)
                return;

            if (roundEndReason == ERoundEndReason.Command
                || roundEndReason == ERoundEndReason.Empty
                || roundEndReason == ERoundEndReason.NewPlayer
                || roundEndReason == ERoundEndReason.Error)
                return;
            await CheckNewDay();
            if (isOfficial)
                ++_instance.Stats.ArenaRoundsPlayed;
            else
                ++_instance.Stats.CustomArenaRoundsPlayed;
        }

        private static async void PlayerLoggedIn(TDSPlayer player)
        {
            if (_instance is null)
                return;

            await CheckNewDay();
            ++_instance.Stats.AmountLogins;
            CheckPlayerPeak(player);
        }

        private static async void CheckPlayerPeak(TDSPlayer _)
        {
            if (_instance is null)
                return;
            if (PlayerManager.PlayerManager.AmountLoggedInPlayers <= _instance.Stats.PlayerPeak)
                return;
            await CheckNewDay();
            _instance.Stats.PlayerPeak = (short)PlayerManager.PlayerManager.AmountLoggedInPlayers;
        }

        private static async void PlayerRegistered(Player _)
        {
            if (_instance is null)
                return;
            await CheckNewDay();
            ++_instance.Stats.AmountRegistrations;
        }

        public static async Task Save()
        {
            if (_instance is null)
                return;

            await _instance.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync();
            });
            
            await CheckNewDay();
        }
    }
}
