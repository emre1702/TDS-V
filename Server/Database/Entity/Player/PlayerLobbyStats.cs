using TDS_Server.Database.Entity.LobbyEntities;

namespace TDS_Server.Database.Entity.Player
{
    public partial class PlayerLobbyStats
    {
        #region Public Properties

        public int Assists { get; set; }
        public int Damage { get; set; }
        public int Deaths { get; set; }
        public int Kills { get; set; }
        public virtual Lobbies Lobby { get; set; }
        public int LobbyId { get; set; }
        public int MostAssistsInARound { get; set; }
        public int MostDamageInARound { get; set; }
        public int MostKillsInARound { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
        public int TotalAssists { get; set; }
        public int TotalDamage { get; set; }
        public int TotalDeaths { get; set; }
        public int TotalKills { get; set; }
        public int TotalMapsBought { get; set; }
        public int TotalRounds { get; set; }

        #endregion Public Properties
    }
}
