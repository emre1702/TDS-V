using TDS_Common.Enum;

namespace TDS_Server_DB.Entity.Lobby
{
    public partial class LobbyMapSettings
    {
        public int LobbyId { get; set; }
        public EMapLimitType MapLimitType { get; set; }

        public virtual Lobbies Lobby { get; set; }
    }
}
