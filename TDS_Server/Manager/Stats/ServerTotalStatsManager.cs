using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Instance.Utility;
using TDS_Server.Manager.EventManager;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Server;

namespace TDS_Server.Manager.Stats
{
    class ServerTotalStatsManager : EntityWrapperClass
    {
        private static ServerTotalStatsManager? _instance;
        public ServerTotalStats Stats { get; private set; }

        public ServerTotalStatsManager()
        {
            // Only to remove the nullable warning
            Stats = new ServerTotalStats();

            ExecuteForDB(dbContext =>
            {
                Stats = dbContext.ServerTotalStats.First();
            }).Wait();
        }

        public static void Init()
        {
            _instance = new ServerTotalStatsManager();

            CustomEventManager.OnPlayerLoggedIn += CheckPlayerPeak;
            CustomEventManager.OnPlayerLoggedOut += CheckPlayerPeak;
        }

        public static void AddArenaRound(ERoundEndReason roundEndReason, bool isOfficial)
        {
            if (_instance is null)
                return;

            if (roundEndReason == ERoundEndReason.Command
                || roundEndReason == ERoundEndReason.Empty
                || roundEndReason == ERoundEndReason.NewPlayer)
                return;
            if (isOfficial)
                ++_instance.Stats.ArenaRoundsPlayed;
            else
                ++_instance.Stats.CustomArenaRoundsPlayed;
        }

        public static void CheckPlayerPeak(TDSPlayer _)
        {
            if (_instance is null)
                return;
            if (PlayerManager.PlayerManager.AmountLoggedInPlayers <= _instance.Stats.PlayerPeak)
                return;
            _instance.Stats.PlayerPeak = (short)PlayerManager.PlayerManager.AmountLoggedInPlayers;
        }

        public static async Task Save()
        {
            if (_instance is null)
                return;

            await _instance.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.SaveChangesAsync();
            });
        }
    }
}
