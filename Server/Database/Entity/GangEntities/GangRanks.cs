namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangRanks
    {
        public int GangId { get; set; }
        public short Rank { get; set; }
        public string Name { get; set; }

        public virtual Gangs Gang { get; set; }
    }
}
