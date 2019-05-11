using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class Commands
    {
        public Commands()
        {
            CommandAlias = new HashSet<CommandAlias>();
            CommandInfos = new HashSet<CommandInfos>();
        }

        public short Id { get; set; }
        public string Command { get; set; }
        public short? NeededAdminLevel { get; set; }
        public short? NeededDonation { get; set; }
        public bool? VipCanUse { get; set; }
        public bool? LobbyOwnerCanUse { get; set; }

        public virtual AdminLevels NeededAdminLevelNavigation { get; set; }
        public virtual ICollection<CommandAlias> CommandAlias { get; set; }
        public virtual ICollection<CommandInfos> CommandInfos { get; set; }
    }
}
