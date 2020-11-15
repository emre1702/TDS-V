using GTANetworkAPI;
using TDS_Server.Database.Entity.Rest;
using TDS_Server.Database.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Player
{
    public class PlayerWeaponStats : IPlayerDataTable
    {
        public int AmountHeadshots { get; set; }
        public int AmountHits { get; set; }
        public int AmountOfficialHeadshots { get; set; }
        public int AmountOfficialHits { get; set; }
        public int AmountOfficialShots { get; set; }
        public int AmountShots { get; set; }
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
