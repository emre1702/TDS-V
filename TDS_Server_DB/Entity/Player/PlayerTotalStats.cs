namespace TDS_Server_DB.Entity.Player
{
    public class PlayerTotalStats
    {
        public int PlayerId { get; set; }
        public long Money { get; set; }

        public virtual Players Player { get; set; }
    }
}
