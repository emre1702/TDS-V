using TDS_Common.Enum;

namespace TDS_Server_DB.Entity
{
    public partial class Weapons
    {
        public EWeaponHash Hash { get; set; }
        public EWeaponType Type { get; set; }
        public short DefaultDamage { get; set; }
        public float DefaultHeadMultiplicator { get; set; }

        public virtual LobbyWeapons LobbyWeapons { get; set; }
    }
}
