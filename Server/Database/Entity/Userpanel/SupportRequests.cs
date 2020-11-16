using System;
using System.Collections.Generic;
using TDS.Server.Database.Entity.Player;
using TDS.Shared.Data.Enums.Userpanel;

namespace TDS.Server.Database.Entity.Userpanel
{
    public class SupportRequests
    {
        #region Public Properties

        public int AtleastAdminLevel { get; set; }
        public virtual Players Author { get; set; }
        public int AuthorId { get; set; }
        public DateTime? CloseTime { get; set; }
        public DateTime CreateTime { get; set; }
        public int Id { get; set; }
        public virtual ICollection<SupportRequestMessages> Messages { get; set; }
        public string Title { get; set; }
        public SupportType Type { get; set; }

        #endregion Public Properties
    }
}
