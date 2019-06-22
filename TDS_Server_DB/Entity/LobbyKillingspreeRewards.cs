using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class LobbyKillingspreeRewards
    {
        public int LobbyId { get; set; }
        public short KillsAmount { get; set; }
        public short? HealthOrArmor { get; set; }
        public short? OnlyHealth { get; set; }
        public short? OnlyArmor { get; set; }

        public virtual Lobbies Lobby { get; set; }
    }
}
