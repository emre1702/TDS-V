using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class CommandsInfo
    {
        public ushort Id { get; set; }
        public byte Language { get; set; }
        public string Info { get; set; }

        public Commands IdNavigation { get; set; }
        public Languages LanguageNavigation { get; set; }
    }
}
