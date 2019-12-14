using System.Collections.Generic;
using TDS_Common.Enum;
using TDS_Server_DB.Entity.LobbyEntities;

namespace TDS_Server_DB.Entity.Rest
{
    public partial class Weapons
    {
        public EWeaponHash Hash { get; set; }
        public EWeaponType Type { get; set; }
        public short DefaultDamage { get; set; }
        public float DefaultHeadMultiplicator { get; set; }

        public virtual ICollection<LobbyWeapons> LobbyWeapons { get; set; }
    }
}
