using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class CommandsInfo
    {
        public byte Id { get; set; }
        public byte Language { get; set; }
        public string Info { get; set; }

        public virtual Commands IdNavigation { get; set; }
        public virtual Languages LanguageNavigation { get; set; }
    }
}
