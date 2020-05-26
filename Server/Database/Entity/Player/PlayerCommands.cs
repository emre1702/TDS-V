using TDS_Server.Database.Entity.Command;

namespace TDS_Server.Database.Entity.Player
{
    public class PlayerCommands
    {
        #region Public Properties

        public virtual Commands Command { get; set; }
        public short CommandId { get; set; }
        public string CommandText { get; set; }
        public long Id { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }

        #endregion Public Properties
    }
}
