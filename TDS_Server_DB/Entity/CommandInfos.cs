using System;
using System.Collections.Generic;
using TDS_Common.Enum;

namespace TDS_Server_DB.Entity
{
    public partial class CommandInfos
    {
        public short Id { get; set; }
        public ELanguage Language { get; set; }
        public string Info { get; set; }

        public virtual Commands IdNavigation { get; set; }
    }
}
