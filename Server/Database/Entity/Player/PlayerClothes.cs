using TDS.Server.Database.Interfaces;

namespace TDS.Server.Database.Entity.Player
{
    public class PlayerClothes : IPlayerDataTable
    {
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
    }
}
