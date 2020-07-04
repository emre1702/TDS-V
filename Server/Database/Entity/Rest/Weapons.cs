using System.Collections.Generic;
using TDS_Server.Database.Entity.LobbyEntities;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Database.Entity.Server;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Rest
{
    public partial class Weapons
    {
        #region Public Properties

        public virtual ICollection<LobbyArmsRaceWeapons> ArmsRaceWeapons { get; set; }
        public int ClipSize { get; set; }
        public float Damage { get; set; }
        public float DamageExpMult { get; set; }
        public WeaponHash Hash { get; set; }
        public float HeadShotDamageModifier { get; set; }

        public float HeadshotsExpMult { get; set; }

        // HudDamage
        public float HitLimbsDamageModifier { get; set; }

        public float HitsExpMult { get; set; }
        public float KillsExpMult { get; set; }
        public virtual ICollection<LobbyWeapons> LobbyWeapons { get; set; }
        public float MaxHeadShotDistance { get; set; }
        public float MinHeadShotDistance { get; set; }
        public virtual ICollection<PlayerWeaponBodypartStats> PlayerWeaponBodypartStats { get; set; }
        public virtual ICollection<PlayerWeaponStats> PlayerWeaponStats { get; set; }
        public float Range { get; set; }

        // NetworkHitLimbsDamageModifier
        public float ReloadTime { get; set; }

        public float ShotsExpMult { get; set; }

        // ReloadTimeMP
        public float TimeBetweenShots { get; set; }

        public WeaponType Type { get; set; }

        #endregion Public Properties
    }
}
