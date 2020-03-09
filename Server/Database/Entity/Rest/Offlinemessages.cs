using System;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Database.Entity.Rest
{
    public partial class Offlinemessages
    {
        public int Id { get; set; }
        public int TargetId { get; set; }
        public int SourceId { get; set; }
        public string Message { get; set; }
        public bool Seen { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual Players Source { get; set; }
        public virtual Players Target { get; set; }
    }
}
