using System.Collections.Generic;
using TDS_Server.Database.Entity.Command;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.Database.Entity.Admin
{
    public partial class AdminLevels
    {
        #region Public Constructors

        public AdminLevels()
        {
            AdminLevelNames = new HashSet<AdminLevelNames>();
            Commands = new HashSet<Commands>();
            Players = new HashSet<Players>();
        }

        #endregion Public Constructors

        #region Public Properties

        public virtual ICollection<AdminLevelNames> AdminLevelNames { get; set; }
        public short ColorB { get; set; }
        public short ColorG { get; set; }
        public short ColorR { get; set; }
        public virtual ICollection<Commands> Commands { get; set; }
        public short Level { get; set; }
        public virtual ICollection<Players> Players { get; set; }

        #endregion Public Properties
    }
}
