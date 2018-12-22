using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class WeaponsDamage
    {
        public uint Hash { get; set; }
        public uint Lobby { get; set; }
        public short Damage { get; set; }
        public byte HeadMultiplicator { get; set; }

        public WeaponsHash HashNavigation { get; set; }
        public Lobbies LobbyNavigation { get; set; }
    }
}
