using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Command
{
    public partial class CommandInfos
    {
        public short Id { get; set; }
        public Language Language { get; set; }
        public string Info { get; set; }

        public virtual Commands IdNavigation { get; set; }
    }
}
