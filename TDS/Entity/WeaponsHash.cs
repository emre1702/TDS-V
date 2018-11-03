using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class WeaponsHash
    {
        public WeaponsHash()
        {
            WeaponsDamage = new HashSet<WeaponsDamage>();
        }

        public uint Hash { get; set; }
        public string Name { get; set; }

        public ICollection<WeaponsDamage> WeaponsDamage { get; set; }
    }
}
