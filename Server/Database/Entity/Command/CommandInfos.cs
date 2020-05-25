using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.Command
{
    public partial class CommandInfos
    {
        #region Public Properties

        public short Id { get; set; }
        public virtual Commands IdNavigation { get; set; }
        public string Info { get; set; }
        public Language Language { get; set; }

        #endregion Public Properties
    }
}
