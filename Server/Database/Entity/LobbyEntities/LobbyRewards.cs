namespace TDS_Server.Database.Entity.LobbyEntities
{
    public partial class LobbyRewards
    {
        #region Public Properties

        public virtual Lobbies Lobby { get; set; }
        public int LobbyId { get; set; }
        public double MoneyPerAssist { get; set; }
        public double MoneyPerDamage { get; set; }
        public double MoneyPerKill { get; set; }

        #endregion Public Properties
    }
}
