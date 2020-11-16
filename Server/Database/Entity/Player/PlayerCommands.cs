using TDS.Server.Database.Entity.Command;
using TDS.Server.Database.Interfaces;

namespace TDS.Server.Database.Entity.Player
{
    public class PlayerCommands : IPlayerDataTable
    {
        public virtual Commands Command { get; set; }
        public short CommandId { get; set; }
        public string CommandText { get; set; }
        public long Id { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }
    }
}
