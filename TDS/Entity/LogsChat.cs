using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class LogsChat
    {
        public uint Id { get; set; }
        public uint Source { get; set; }
        public uint? Target { get; set; }
        public string Message { get; set; }
        public uint Lobby { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
