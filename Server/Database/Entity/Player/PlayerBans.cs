using System;
using TDS.Server.Database.Entity.LobbyEntities;
using TDS.Server.Database.Interfaces;

namespace TDS.Server.Database.Entity.Player
{
    public class PlayerBans : IPlayerDataTable
    {

        public virtual Players Admin { get; set; }
        public int? AdminId { get; set; }
        public DateTime? EndTimestamp { get; set; }
        public string IP { get; set; }
        public virtual Lobbies Lobby { get; set; }
        public int LobbyId { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
        public bool PreventConnection { get; set; }
        public string Reason { get; set; }
        public ulong? SCId { get; set; }
        public string SCName { get; set; }
        public string Serial { get; set; }
        public DateTime StartTimestamp { get; set; }

    }
}
