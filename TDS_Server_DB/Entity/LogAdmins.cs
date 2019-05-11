using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class LogAdmins
    {
        public long Id { get; set; }
        public short Type { get; set; }
        public int Source { get; set; }
        public int? Target { get; set; }
        public int? Lobby { get; set; }
        public bool AsDonator { get; set; }
        public bool AsVip { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
