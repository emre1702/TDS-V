using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class GangMembers
    {
        public uint Id { get; set; }
        public uint? GangId { get; set; }
        public uint? PlayerId { get; set; }

        public virtual Gangs Gang { get; set; }
        public virtual Players Player { get; set; }
    }
}
