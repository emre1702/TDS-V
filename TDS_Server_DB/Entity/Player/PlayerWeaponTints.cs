using TDS_Common.Enum;
using TDS_Server_DB.Entity.Rest;

namespace TDS_Server_DB.Entity.Player
{
    public class PlayerWeaponTints
    {
        public int PlayerId { get; set; }
        public EWeaponHash WeaponHash { get; set; }
        public int TintId { get; set; }
        public bool IsMK2 { get; set; }

        public virtual Players Player { get; set; }
        public virtual Weapons Weapon { get; set; }
        public virtual WeaponsTints Tint { get; set; }
    }
}
