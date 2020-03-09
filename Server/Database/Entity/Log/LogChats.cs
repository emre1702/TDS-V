using System;

namespace TDS_Server.Database.Entity.Log
{
    public partial class LogChats
    {
        public long Id { get; set; }
        public int Source { get; set; }
        public int? Target { get; set; }
        public string Message { get; set; }
        public int? Lobby { get; set; }
        public bool IsAdminChat { get; set; }
        public bool IsTeamChat { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
