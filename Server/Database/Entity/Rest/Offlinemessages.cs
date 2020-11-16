using System;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.Entity.Rest
{
    public partial class Offlinemessages
    {
        #region Public Properties

        public int Id { get; set; }
        public string Message { get; set; }
        public bool Seen { get; set; }
        public virtual Players Source { get; set; }
        public int SourceId { get; set; }
        public virtual Players Target { get; set; }
        public int TargetId { get; set; }
        public DateTime Timestamp { get; set; }

        #endregion Public Properties
    }
}
