using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server_DB.Entity.Log
{
    public class LogKills
    {
        public long Id { get; set; }
        public int KillerId { get; set; }
        public int DeadId { get; set; }
        public uint WeaponId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
