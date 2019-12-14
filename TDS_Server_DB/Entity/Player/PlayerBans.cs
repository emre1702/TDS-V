using System;
using TDS_Server_DB.Entity.LobbyEntities;

namespace TDS_Server_DB.Entity.Player
{
    public partial class PlayerBans
    {
        public int PlayerId { get; set; }
        public int LobbyId { get; set; }
        public int? AdminId { get; set; }
        public string IP { get; set; }
        public string Serial { get; set; }
        public string SCName { get; set; }
        public ulong? SCId { get; set; }
        public string Reason { get; set; }
        public DateTime StartTimestamp { get; set; }
        public DateTime? EndTimestamp { get; set; }
        public bool PreventConnection { get; set; }

        public virtual Players Admin { get; set; }
        public virtual Lobbies Lobby { get; set; }
        public virtual Players Player { get; set; }
    }
}
