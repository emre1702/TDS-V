using TDS_Server_DB.Entity.Player;

namespace TDS_Server_DB.Entity.Gang
{
    public partial class GangMembers
    {
        public uint Id { get; set; }
        public uint? GangId { get; set; }
        public uint? PlayerId { get; set; }

        public virtual Gangs Gang { get; set; }
        public virtual Players Player { get; set; }
    }
}
