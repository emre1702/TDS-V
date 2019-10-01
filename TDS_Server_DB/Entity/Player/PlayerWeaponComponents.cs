using GTANetworkAPI;
using TDS_Common.Enum;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server_DB.Entity.Player
{
    public class PlayerWeaponComponents
    {
        public int PlayerId { get; set; }
        public EWeaponHash WeaponHash { get; set; }
        public EWeaponComponent ComponentHash { get; set; }

        public virtual Players Player { get; set; }
        public virtual Weapons Weapon { get; set; }
        public virtual WeaponComponents Component { get; set; }
    }
}
