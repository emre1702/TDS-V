using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Admincommands
    {
        public string Command { get; set; }
        public byte? NeededAdminLevel { get; set; }
        public byte? NeededDonatorLevel { get; set; }
        public bool VipcanUse { get; set; }

        public Adminlevels NeededAdminLevelNavigation { get; set; }
    }
}
