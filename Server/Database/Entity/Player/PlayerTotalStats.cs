namespace TDS_Server.Database.Entity.Player
{
    public class PlayerTotalStats
    {
        #region Public Properties

        public long Money { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }

        #endregion Public Properties
    }
}
