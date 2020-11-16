using GTANetworkAPI;
using TDS.Server.Database.Entity.Rest;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.LobbyEntities
{
    public partial class LobbyWeapons
    {
        public int Lobby { get; set; }
        public WeaponHash Hash { get; set; }

        public int Ammo { get; set; }
        public float? Damage { get; set; }
        public float? HeadMultiplicator { get; set; }

        public virtual Lobbies LobbyNavigation { get; set; }
        public virtual Weapons HashNavigation { get; set; }
    }
}
