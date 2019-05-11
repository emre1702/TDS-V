using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class LogErrors
    {
        public long Id { get; set; }
        public int? Source { get; set; }
        public string Info { get; set; }
        public string StackTrace { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
