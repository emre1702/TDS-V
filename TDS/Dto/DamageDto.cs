using TDS.Entity;

namespace TDS.Dto
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
