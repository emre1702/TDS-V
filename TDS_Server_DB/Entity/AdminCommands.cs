using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class AdminCommands
    {
        public string Command { get; set; }
        public byte? NeededAdminLevel { get; set; }
        public byte? NeededDonatorLevel { get; set; }
        public bool VipCanUse { get; set; }

        public AdminLevels NeededAdminLevelNavigation { get; set; }
    }
}
