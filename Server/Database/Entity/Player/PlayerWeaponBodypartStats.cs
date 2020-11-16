using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TDS.Server.Database.Entity.Rest;
using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.Player
{
    public class PlayerWeaponBodypartStats : IPlayerDataTable
    {
        public int AmountHits { get; set; }
        public int AmountOfficialHits { get; set; }
        public PedBodyPart BodyPart { get; set; }
        public long DealtDamage { get; set; }
        public long DealtOfficialDamage { get; set; }
        public int Kills { get; set; }
        public int OfficialKills { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
        public virtual Weapons Weapon { get; set; }
        public WeaponHash WeaponHash { get; set; }
    }
}
