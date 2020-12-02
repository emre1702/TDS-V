using TDS.Server.Database.Interfaces;

namespace TDS.Server.Database.Entity.Player
{
    public class PlayerTotalStats : IPlayerDataTable
    {
        public int PlayerId { get; set; }
        public long Money { get; set; }

        public virtual Players Player { get; set; }
    }
}
