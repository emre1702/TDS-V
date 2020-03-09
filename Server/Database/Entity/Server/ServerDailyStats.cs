using NpgsqlTypes;
using System;

namespace TDS_Server.Database.Entity.Server
{
    public class ServerDailyStats
    {
        public DateTime Date { get; set; }
        public short PlayerPeak { get; set; }
        public int ArenaRoundsPlayed { get; set; }
        public int CustomArenaRoundsPlayed { get; set; }
        public int AmountLogins { get; set; }
        public int AmountRegistrations { get; set; }
    }
}
