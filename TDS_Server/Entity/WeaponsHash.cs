using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class WeaponsHash
    {
        public WeaponsHash()
        {
            LobbyWeapons = new HashSet<LobbyWeapons>();
        }

        public uint Hash { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LobbyWeapons> LobbyWeapons { get; set; }
    }
}
