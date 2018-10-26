using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Playerstats
    {
        public uint Id { get; set; }
        public uint Money { get; set; }
        public DateTime LastLoginTimestamp { get; set; }

        public Players IdNavigation { get; set; }
    }
}
