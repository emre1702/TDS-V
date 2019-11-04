using System;
using System.Collections.Generic;
using System.Text;

namespace TDS_Server_DB.Entity.Gang
{
    public class GangRanks
    {
        public int GangId { get; set; }
        public short Rank { get; set; }
        public string Name { get; set; }

        public virtual Gangs Gang { get; set; }
    }
}
