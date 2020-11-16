using System;
using System.Collections.Generic;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.Entity.Userpanel
{
    public class Applications
    {
        #region Public Properties

        public virtual ICollection<ApplicationAnswers> Answers { get; set; }
        public bool Closed { get; set; }
        public DateTime CreateTime { get; set; }
        public int Id { get; set; }
        public virtual ICollection<ApplicationInvitations> Invitations { get; set; }
        public virtual Players Player { get; set; }
        public int PlayerId { get; set; }

        #endregion Public Properties
    }
}
