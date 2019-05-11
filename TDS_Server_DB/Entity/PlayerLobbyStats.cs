using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class PlayerLobbyStats
    {
        public int PlayerId { get; set; }
        public int LobbyId { get; set; }
        public int Kills { get; set; }
        public int Assists { get; set; }
        public int Deaths { get; set; }
        public int Damage { get; set; }
        public int TotalKills { get; set; }
        public int TotalAssists { get; set; }
        public int TotalDeaths { get; set; }
        public int TotalDamage { get; set; }

        public virtual Lobbies Lobby { get; set; }
        public virtual Players Player { get; set; }
    }
}
