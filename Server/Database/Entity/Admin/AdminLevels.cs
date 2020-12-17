using System.Collections.Generic;
using TDS.Server.Database.Entity.Command;
using TDS.Server.Database.Entity.Player;

namespace TDS.Server.Database.Entity.Admin
{
    public class AdminLevels
    {
        public AdminLevels()
        {
            AdminLevelNames = new HashSet<AdminLevelNames>();
            Commands = new HashSet<Commands>();
            Players = new HashSet<Players>();
        }

        public virtual ICollection<AdminLevelNames> AdminLevelNames { get; set; }
        public short ColorB { get; set; }
        public short ColorG { get; set; }
        public short ColorR { get; set; }
        public virtual ICollection<Commands> Commands { get; set; }
        public short Level { get; set; }
        public virtual ICollection<Players> Players { get; set; }

    }
}
