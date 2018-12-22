using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class LogsError
    {
        public uint Id { get; set; }
        public uint? Source { get; set; }
        public string Info { get; set; }
        public string StackTrace { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
