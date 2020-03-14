using System;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Log
{
    public partial class LogAdmins
    {
        public long Id { get; set; }
        public LogType Type { get; set; }
        public int Source { get; set; }
        public int? Target { get; set; }
        public int? Lobby { get; set; }
        public bool AsDonator { get; set; }
        public bool AsVip { get; set; }
        public string Reason { get; set; }
        public DateTime Timestamp { get; set; }
        public string LengthOrEndTime { get; set; }
    }
}
