using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Playermapratings
    {
        public uint Id { get; set; }
        public string MapName { get; set; }
        public byte Rating { get; set; }

        public virtual Players IdNavigation { get; set; }
    }
}
