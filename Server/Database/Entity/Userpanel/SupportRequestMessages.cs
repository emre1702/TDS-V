using System;
using System.Collections.Generic;
using System.Text;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class SupportRequestMessages
    {
        public int RequestId { get; set; }
        public int MessageIndex { get; set; }
        public int AuthorId { get; set; }
        public string Text { get; set; }
        public DateTime CreateTime { get; set; }

        public virtual Players Author { get; set; }
        public virtual SupportRequests Request { get; set; }
    }
}
