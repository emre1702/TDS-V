using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Database.Entity.LobbyEntities
{
    public partial class LobbyMaps
    {
        #region Public Properties

        public virtual Lobbies Lobby { get; set; }
        public int LobbyId { get; set; }
        public virtual Maps Map { get; set; }
        public int MapId { get; set; }

        #endregion Public Properties
    }
}
