using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Player
{
    public partial class PlayerRelations
    {
        public int PlayerId { get; set; }
        public int TargetId { get; set; }
        public PlayerRelation Relation { get; set; }

        public virtual Players Player { get; set; }
        public virtual Players Target { get; set; }
    }
}
