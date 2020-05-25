using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Player
{
    public partial class PlayerRelations
    {
        #region Public Properties

        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
        public PlayerRelation Relation { get; set; }
        public virtual Players Target { get; set; }
        public int TargetId { get; set; }

        #endregion Public Properties
    }
}
