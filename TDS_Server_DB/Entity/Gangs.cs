using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Gangs
    {
        public Gangs()
        {
            Players = new HashSet<Players>();
        }

        public uint Id { get; set; }
        public uint? TeamId { get; set; }
        public string Short { get; set; }

        public virtual Teams Team { get; set; }
        public virtual ICollection<Players> Players { get; set; }
    }
}
