using System.Collections.Generic;
using TDS_Server.Database.Entity.Admin;

namespace TDS_Server.Database.Entity.Command
{
    public partial class Commands
    {
        #region Public Constructors

        public Commands()
        {
            CommandAlias = new HashSet<CommandAlias>();
            CommandInfos = new HashSet<CommandInfos>();
        }

        #endregion Public Constructors

        #region Public Properties

        public string Command { get; set; }
        public virtual ICollection<CommandAlias> CommandAlias { get; set; }
        public virtual ICollection<CommandInfos> CommandInfos { get; set; }
        public short Id { get; set; }
        public bool LobbyOwnerCanUse { get; set; }
        public short? NeededAdminLevel { get; set; }
        public virtual AdminLevels NeededAdminLevelNavigation { get; set; }
        public short? NeededDonation { get; set; }
        public bool VipCanUse { get; set; }

        #endregion Public Properties
    }
}
