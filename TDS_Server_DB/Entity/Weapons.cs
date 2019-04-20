using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Weapons
    {
        public Weapons()
        {
            LobbyWeapons = new HashSet<LobbyWeapons>();
        }

        public uint Hash { get; set; }
        public string Name { get; set; }
        public sbyte Type { get; set; }
        public short DefaultDamage { get; set; }
        public float DefaultHeadMultiplicator { get; set; }

        public virtual WeaponTypes TypeNavigation { get; set; }
        public virtual ICollection<LobbyWeapons> LobbyWeapons { get; set; }
    }
}
