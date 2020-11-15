using TDS_Server.Database.Interfaces;

namespace TDS_Server.Database.Entity.Player
{
    public class PlayerTotalStats : IPlayerDataTable
    {
        public long Money { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
    }
}
