﻿using System.Collections.Generic;
using TDS_Shared.Data.Enums;
using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.Entity.Rest
{
    public partial class Weapons
    {
        public WeaponHash Hash { get; set; }
        public WeaponType Type { get; set; }

        public int ClipSize { get; set; }
        public float MinHeadShotDistance { get; set; }
        public float MaxHeadShotDistance { get; set; }
        public float HeadShotDamageModifier { get; set; }
        public float Damage { get; set; }   // HudDamage
        public float HitLimbsDamageModifier { get; set; }   // NetworkHitLimbsDamageModifier
        public float ReloadTime { get; set; }   // ReloadTimeMP
        public float TimeBetweenShots { get; set; }
        public float Range { get; set; }

        public virtual ICollection<LobbyWeapons> LobbyWeapons { get; set; }
    }
}
