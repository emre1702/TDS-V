using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Playerbans
    {
        public uint Id { get; set; }
        public uint ForLobby { get; set; }
        public uint? Admin { get; set; }
        public string Reason { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime? EndTimestamp { get; set; }

        public virtual Players AdminNavigation { get; set; }
        public virtual Lobbies ForLobbyNavigation { get; set; }
        public virtual Players IdNavigation { get; set; }
    }
}
