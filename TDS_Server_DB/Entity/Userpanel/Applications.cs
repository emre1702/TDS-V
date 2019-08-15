using System;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server_DB.Entity.Userpanel
{
    public class Applications
    {
        public int UserId { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual Players User { get; set; }
    }
}
