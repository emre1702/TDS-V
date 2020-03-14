using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Data.Models
{
    internal class DamageDto
    {
        public float Damage;
        public float HeadMultiplier;

        public DamageDto()
        {
        }

        public DamageDto(LobbyWeapons weapon)
        {
            Damage = weapon.Damage ?? 0;
            HeadMultiplier = weapon.HeadMultiplicator ?? 0;
        }
    }
}
