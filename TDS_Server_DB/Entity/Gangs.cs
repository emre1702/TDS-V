using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class Gangs
    {
        public Gangs()
        {
            Players = new HashSet<Players>();
        }

        public int Id { get; set; }
        public int TeamId { get; set; }
        public string Short { get; set; }

        public virtual Teams Team { get; set; }
        public virtual ICollection<Players> Players { get; set; }
    }
}
