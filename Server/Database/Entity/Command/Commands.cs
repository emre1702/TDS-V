using System.Collections.Generic;
using TDS.Server.Database.Entity.Admin;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.Entity.Command
{
    public class Commands
    {
        public Commands()
        {
            CommandAlias = new List<CommandAlias>();
            CommandInfos = new List<CommandInfos>();
            PlayerCommands = new List<PlayerCommands>();
        }

        public string Command { get; set; }
        public short Id { get; set; }
        public bool LobbyOwnerCanUse { get; set; }
        public short? NeededAdminLevel { get; set; }
        public virtual AdminLevels NeededAdminLevelNavigation { get; set; }
        public short? NeededDonation { get; set; }
        public bool VipCanUse { get; set; }

        public virtual ICollection<CommandAlias> CommandAlias { get; set; }
        public virtual ICollection<CommandInfos> CommandInfos { get; set; }
        public virtual ICollection<PlayerCommands> PlayerCommands { get; set; }
    }
}
