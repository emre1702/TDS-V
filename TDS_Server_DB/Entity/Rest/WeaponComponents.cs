using GTANetworkAPI;
using System.Collections.Generic;
using TDS_Common.Enum;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server_DB.Entity.Rest
{
    public class WeaponComponents
    {
        public WeaponComponent Hash { get; set; }
        public EWeaponHash WeaponHash { get; set; }
        public EWeaponComponentCategory Category { get; set; }

        public virtual Weapons Weapon { get; set; }
        public virtual ICollection<PlayerWeaponComponents> PlayerWeaponComponents { get; set; }
    }
}
