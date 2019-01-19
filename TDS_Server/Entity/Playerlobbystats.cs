using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class PlayerLobbyStats
    {
        public uint Id { get; set; }
        public uint Lobby { get; set; }
        public uint Kills { get; set; }
        public uint Deaths { get; set; }
        public uint Assists { get; set; }
        public uint Damage { get; set; }
        public uint TotalKills { get; set; }
        public uint TotalDeaths { get; set; }
        public uint TotalAssists { get; set; }
        public uint TotalDamage { get; set; }

        public virtual Players IdNavigation { get; set; }
        public virtual Lobbies LobbyNavigation { get; set; }
    }
}
