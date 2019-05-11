using System;
using System.Collections.Generic;
using System.Net;

namespace TDS_Server_DB.Entity
{
    public partial class LogRests
    {
        public long Id { get; set; }
        public short Type { get; set; }
        public int Source { get; set; }
        public string Serial { get; set; }
        public IPAddress Ip { get; set; }
        public int? Lobby { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
