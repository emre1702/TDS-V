using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.LobbyEntities
{
    public partial class LobbyMapSettings
    {
        #region Public Properties

        public virtual Lobbies Lobby { get; set; }
        public int LobbyId { get; set; }
        public int MapLimitTime { get; set; }
        public MapLimitType MapLimitType { get; set; }

        #endregion Public Properties
    }
}
