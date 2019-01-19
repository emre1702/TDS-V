using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Commands
    {
        public Commands()
        {
            CommandsAlias = new HashSet<CommandsAlias>();
            CommandsInfo = new HashSet<CommandsInfo>();
        }

        public byte Id { get; set; }
        public string Command { get; set; }
        public byte? NeededAdminLevel { get; set; }
        public byte? NeededDonation { get; set; }
        public bool? VipCanUse { get; set; }
        public bool? LobbyOwnerCanUse { get; set; }

        public virtual AdminLevels NeededAdminLevelNavigation { get; set; }
        public virtual ICollection<CommandsAlias> CommandsAlias { get; set; }
        public virtual ICollection<CommandsInfo> CommandsInfo { get; set; }
    }
}
