using System.Collections.Generic;
using TDS_Common.Enum;
using TDS_Server_DB.Entity.Lobby;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server_DB.Entity.Rest
{
    public partial class Weapons
    {
        public EWeaponHash Hash { get; set; }
        public EWeaponType Type { get; set; }
        public short DefaultDamage { get; set; }
        public float DefaultHeadMultiplicator { get; set; }

        public virtual ICollection<WeaponComponents> Components { get; set; }
        public virtual LobbyWeapons LobbyWeapons { get; set; }
        public virtual ICollection<PlayerWeaponTints> PlayerWeaponTints { get; set; }
        public virtual ICollection<PlayerWeaponComponents> PlayerWeaponComponents { get; set; }
    }
}
