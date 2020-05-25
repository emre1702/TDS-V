using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Data.Models
{
    public class DamageDto
    {
        #region Public Fields

        public float Damage;
        public float HeadMultiplier;

        #endregion Public Fields

        #region Public Constructors

        public DamageDto()
        {
        }

        public DamageDto(LobbyWeapons weapon)
        {
            Damage = weapon.Damage ?? 0;
            HeadMultiplier = weapon.HeadMultiplicator ?? 0;
        }

        #endregion Public Constructors
    }
}
