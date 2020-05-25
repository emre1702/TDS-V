namespace TDS_Server.Database.Entity.Server
{
    public class ServerTotalStats
    {
        #region Public Properties

        public long ArenaRoundsPlayed { get; set; }
        public long CustomArenaRoundsPlayed { get; set; }
        public short Id { get; set; }
        public short PlayerPeak { get; set; }

        #endregion Public Properties
    }
}
