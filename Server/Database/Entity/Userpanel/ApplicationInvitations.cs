using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.Entity.Userpanel
{
    public class ApplicationInvitations
    {
        #region Public Properties

        public virtual Players Admin { get; set; }
        public int AdminId { get; set; }
        public virtual Applications Application { get; set; }
        public int ApplicationId { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }

        #endregion Public Properties
    }
}
