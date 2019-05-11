using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class CommandAlias
    {
        public string Alias { get; set; }
        public short Command { get; set; }

        public virtual Commands CommandNavigation { get; set; }
    }
}
