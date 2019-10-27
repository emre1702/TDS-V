using System;
using System.Collections.Generic;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server_DB.Entity.Userpanel
{
    public class Applications
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public DateTime CreateTime { get; set; }
        public bool Closed { get; set; }

        public virtual Players Player { get; set; }
        public virtual ICollection<ApplicationAnswers> Answers { get; set; }
        public virtual ICollection<ApplicationInvitations> Invitations { get; set; }
    }
}
