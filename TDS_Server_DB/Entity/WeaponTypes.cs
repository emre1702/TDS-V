using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class WeaponTypes
    {
        public WeaponTypes()
        {
            Weapons = new HashSet<Weapons>();
        }

        public short Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Weapons> Weapons { get; set; }
    }
}
