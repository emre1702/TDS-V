using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Playerbans
    {
        public uint Id { get; set; }
        public uint ForLobby { get; set; }
        public string Scname { get; set; }
        public string Serial { get; set; }
        public string Ip { get; set; }
        public uint? Admin { get; set; }
        public string Reason { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime? EndTimestamp { get; set; }

        public Players AdminNavigation { get; set; }
        public Lobbies ForLobbyNavigation { get; set; }
        public Players IdNavigation { get; set; }
    }
}
