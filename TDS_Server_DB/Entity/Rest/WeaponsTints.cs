using System.Collections.Generic;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server_DB.Entity.Rest
{
    public class WeaponsTints
    {
        public int Id { get; set; }
        public bool IsMK2 { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PlayerWeaponTints> PlayerWeaponTints { get; set; }
    }
}
