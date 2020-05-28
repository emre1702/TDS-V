namespace TDS_Server.Database.Entity.LobbyEntities
{
    public partial class LobbyKillingspreeRewards
    {
        #region Public Properties

        public short? HealthOrArmor { get; set; }
        public short KillsAmount { get; set; }
        public virtual Lobbies Lobby { get; set; }
        public int LobbyId { get; set; }

        #endregion Public Properties
    }
}
