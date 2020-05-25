using TDS_Server.Database.Entity.Rest;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.LobbyEntities
{
    public partial class LobbyWeapons
    {
        #region Public Properties

        public int Ammo { get; set; }
        public float? Damage { get; set; }
        public WeaponHash Hash { get; set; }
        public virtual Weapons HashNavigation { get; set; }
        public float? HeadMultiplicator { get; set; }
        public int Lobby { get; set; }
        public virtual Lobbies LobbyNavigation { get; set; }

        #endregion Public Properties
    }
}
