using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Commands
    {
        public Commands()
        {
            CommandsAlias = new HashSet<CommandsAlias>();
            CommandsInfo = new HashSet<CommandsInfo>();
        }

        public ushort Id { get; set; }
        public string Command { get; set; }
        public byte? NeededAdminLevel { get; set; }
        public byte? NeededDonation { get; set; }
        public bool? VipCanUse { get; set; }
        public bool? LobbyOwnerCanUse { get; set; }

        public Adminlevels NeededAdminLevelNavigation { get; set; }
        public ICollection<CommandsAlias> CommandsAlias { get; set; }
        public ICollection<CommandsInfo> CommandsInfo { get; set; }
    }
}
