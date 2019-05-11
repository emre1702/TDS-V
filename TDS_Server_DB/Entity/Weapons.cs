using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class Weapons
    {
        public long Hash { get; set; }
        public string Name { get; set; }
        public short Type { get; set; }
        public short DefaultDamage { get; set; }
        public float DefaultHeadMultiplicator { get; set; }

        public virtual WeaponTypes TypeNavigation { get; set; }
        public virtual LobbyWeapons LobbyWeapons { get; set; }
    }
}
