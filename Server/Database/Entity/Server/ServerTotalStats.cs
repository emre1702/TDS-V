using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server.Database.Entity.Server
{
    public class ServerTotalStats
    {
        public short Id { get; set; }
        public short PlayerPeak { get; set; }
        public long ArenaRoundsPlayed { get; set; }
        public long CustomArenaRoundsPlayed { get; set; }
    }
}
