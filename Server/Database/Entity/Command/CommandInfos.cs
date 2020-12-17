using TDS.Shared.Data.Enums;

namespace TDS.Server.Database.Entity.Command
{
    public class CommandInfos
    {
        public short Id { get; set; }
        public string Info { get; set; }
        public Language Language { get; set; }

        public virtual Commands IdNavigation { get; set; }
    }
}
