namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangRanks
    {
        #region Public Properties

        public virtual Gangs Gang { get; set; }
        public int GangId { get; set; }
        public string Name { get; set; }
        public short Rank { get; set; }

        #endregion Public Properties
    }
}
