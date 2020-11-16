using System;

namespace TDS.Server.Database.Entity.Rest
{
    public class Announcements
    {
        #region Public Properties

        public DateTime Created { get; set; }
        public int Id { get; set; }
        public string Text { get; set; }

        #endregion Public Properties
    }
}
