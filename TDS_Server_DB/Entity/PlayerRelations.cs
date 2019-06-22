using TDS_Common.Enum;

namespace TDS_Server_DB.Entity
{
    public partial class PlayerRelations
    {
        public int PlayerId { get; set; }
        public int TargetId { get; set; }
        public EPlayerRelation Relation { get; set; }

        public virtual Players Player { get; set; }
        public virtual Players Target { get; set; }
    }
}
