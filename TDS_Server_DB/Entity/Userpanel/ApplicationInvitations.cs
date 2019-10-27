using TDS_Server_DB.Entity.Player;

namespace TDS_Server_DB.Entity.Userpanel
{
    public class ApplicationInvitations
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int AdminId { get; set; }
        public string Message { get; set; }

        public virtual Applications Application { get; set; }
        public virtual Players Admin { get; set; }
    }
}
