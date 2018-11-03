using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class CommandsAlias
    {
        public string Alias { get; set; }
        public ushort? Command { get; set; }

        public Commands CommandNavigation { get; set; }
    }
}
