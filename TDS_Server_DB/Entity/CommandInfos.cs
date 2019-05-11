using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class CommandInfos
    {
        public short Id { get; set; }
        public short Language { get; set; }
        public string Info { get; set; }

        public virtual Commands IdNavigation { get; set; }
        public virtual Languages LanguageNavigation { get; set; }
    }
}
