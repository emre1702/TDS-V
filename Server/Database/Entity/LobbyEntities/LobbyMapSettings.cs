using TDS_Common.Enum;

namespace TDS_Server.Database.Entity.LobbyEntities
{
    public partial class LobbyMapSettings
    {
        public int LobbyId { get; set; }
        public int MapLimitTime { get; set; }
        public EMapLimitType MapLimitType { get; set; }

        public virtual Lobbies Lobby { get; set; }
    }
}
