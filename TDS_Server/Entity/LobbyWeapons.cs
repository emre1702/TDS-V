using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class LobbyWeapons
    {
        public uint Hash { get; set; }
        public uint Lobby { get; set; }
        public uint Ammo { get; set; }
        public short? Damage { get; set; }
        public float? HeadMultiplicator { get; set; }

        public virtual Weapons HashNavigation { get; set; }
        public virtual Lobbies LobbyNavigation { get; set; }
    }
}
