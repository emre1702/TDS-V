using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class CommandsInfo
    {
        public ushort Id { get; set; }
        public byte Language { get; set; }
        public string Info { get; set; }

        public virtual Commands IdNavigation { get; set; }
        public virtual Languages LanguageNavigation { get; set; }
    }
}
