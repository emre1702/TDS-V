using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class LogsAdmin
    {
        public uint Id { get; set; }
        public byte Type { get; set; }
        public uint Source { get; set; }
        public uint? Target { get; set; }
        public uint? Lobby { get; set; }
        public bool AsDonator { get; set; }
        public bool AsVip { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
