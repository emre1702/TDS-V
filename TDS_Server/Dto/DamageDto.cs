using TDS_Server.Entity;

namespace TDS_Server.Dto
{
    class DamageDto
    {
        public short Damage;
        public float HeadMultiplier;

        public DamageDto(LobbyWeapons weapon)
        {
            Damage = weapon.Damage;
            HeadMultiplier = weapon.HeadMultiplicator;
        }
    }
}
