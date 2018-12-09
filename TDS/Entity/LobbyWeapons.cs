using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class LobbyWeapons
    {
        public uint Hash { get; set; }
        public uint Lobby { get; set; }
        public uint Ammo { get; set; }
        public short Damage { get; set; }
        public float HeadMultiplicator { get; set; }

        public Lobbies LobbyNavigation { get; set; }
    }
}
