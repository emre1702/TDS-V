using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Database.Entity.Player
{
    public partial class PlayerMapRatings
    {
        public int PlayerId { get; set; }
        public int MapId { get; set; }
        public short Rating { get; set; }

        public virtual Maps Map { get; set; }
        public virtual Players Player { get; set; }
    }
}
