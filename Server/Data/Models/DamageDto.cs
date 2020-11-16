using TDS.Server.Database.Entity.LobbyEntities;

namespace TDS.Server.Data.Models
{
    public class DamageDto
    {
        public float Damage { get; set; }
        public float HeadMultiplier { get; set; }

        public DamageDto()
        {
        }

        public DamageDto(LobbyWeapons weapon)
        {
            Damage = weapon.Damage ?? 0;
            HeadMultiplier = weapon.HeadMultiplicator ?? 0;
        }

        public DamageDto(DamageTestWeapon weaponDamageData)
        {
            Damage = weaponDamageData.Damage;
            HeadMultiplier = weaponDamageData.HeadshotDamageMultiplier;
        }
    }
}
