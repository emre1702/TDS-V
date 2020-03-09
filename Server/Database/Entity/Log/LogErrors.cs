using System;

namespace TDS_Server.Database.Entity.Log
{
    public partial class LogErrors
    {
        public long Id { get; set; }
        public int? Source { get; set; }
        public string Info { get; set; }
        public string StackTrace { get; set; }
        public DateTime Timestamp { get; set; }

        public override string ToString()
        {
            return $"[{Timestamp.ToString()}] Error (source = {Source?.ToString() ?? "?"}) :{Environment.NewLine}{Info}{Environment.NewLine}{StackTrace}";
        }
    }
}
