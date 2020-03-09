namespace TDS_Server.Database.Entity.LobbyEntities
{
    public partial class LobbyRewards
    {
        public int LobbyId { get; set; }
        public double MoneyPerKill { get; set; }
        public double MoneyPerAssist { get; set; }
        public double MoneyPerDamage { get; set; }

        public virtual Lobbies Lobby { get; set; }
    }
}
