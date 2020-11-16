using TDS.Server.Database.Entity.Rest;
using TDS.Server.Database.Interfaces;

namespace TDS.Server.Database.Entity.Player
{
    public partial class PlayerMapRatings : IPlayerDataTable
    {
        public virtual Maps Map { get; set; }
        public int MapId { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
        public short Rating { get; set; }
    }
}
