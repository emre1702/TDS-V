using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class LobbyWeapons
    {
        public long Hash { get; set; }
        public int Lobby { get; set; }
        public int Ammo { get; set; }
        public short? Damage { get; set; }
        public float? HeadMultiplicator { get; set; }

        public virtual Weapons HashNavigation { get; set; }
        public virtual Lobbies LobbyNavigation { get; set; }
    }
}
