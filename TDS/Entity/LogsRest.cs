using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class LogsRest
    {
        public uint Id { get; set; }
        public byte Type { get; set; }
        public uint Source { get; set; }
        public string Serial { get; set; }
        public string Ip { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
