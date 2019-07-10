﻿using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Server;

namespace TDS_Server.Manager.Stats
{
    class ServerTotalStatsManager
    {
        public static TDSNewContext DbContext { get; private set; }
        public static ServerTotalStats Stats { get; private set; }

        public static void Init()
        {
            DbContext = new TDSNewContext();
            Stats = DbContext.ServerTotalStats.First();

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
            Stats.PlayerPeak = (short)Player.Player.AmountLoggedInPlayers;
        }

        public static Task Save()
        {
            return DbContext.SaveChangesAsync();
        }
    }
}