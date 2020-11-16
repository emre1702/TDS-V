using TDS.Server.Database.Interfaces;
using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.Player
{
    public class PlayerRelations : IPlayerDataTable
    {
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
        public PlayerRelation Relation { get; set; }
        public virtual Players Target { get; set; }
        public int TargetId { get; set; }
    }
}
