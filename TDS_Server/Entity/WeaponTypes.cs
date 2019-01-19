using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class WeaponTypes
    {
        public WeaponTypes()
        {
            Weapons = new HashSet<Weapons>();
        }

        public sbyte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Weapons> Weapons { get; set; }
    }
}
