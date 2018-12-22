using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Offlinemessages
    {
        public uint Id { get; set; }
        public uint TargetId { get; set; }
        public uint SourceId { get; set; }
        public string Message { get; set; }
        public bool AlreadyLoadedOnce { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual Players Source { get; set; }
        public virtual Players Target { get; set; }
    }
}
