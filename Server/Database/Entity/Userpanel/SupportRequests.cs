using System;
using System.Collections.Generic;
using TDS_Shared.Data.Enums.Userpanel;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class SupportRequests
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; }
        public SupportType Type { get; set; }
        public int AtleastAdminLevel { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? CloseTime { get; set; }

        public virtual Players Author { get; set; }
        public virtual ICollection<SupportRequestMessages> Messages { get; set; }
    }
}
