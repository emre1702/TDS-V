using System;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.Entity.Userpanel
{
    public class SupportRequestMessages
    {
        #region Public Properties

        public virtual Players Author { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreateTime { get; set; }
        public int MessageIndex { get; set; }
        public virtual SupportRequests Request { get; set; }
        public int RequestId { get; set; }
        public string Text { get; set; }

        #endregion Public Properties
    }
}
