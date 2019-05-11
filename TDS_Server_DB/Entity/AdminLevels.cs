using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class AdminLevels
    {
        public AdminLevels()
        {
            AdminLevelNames = new HashSet<AdminLevelNames>();
            Commands = new HashSet<Commands>();
            Players = new HashSet<Players>();
        }

        public short Level { get; set; }
        public short ColorR { get; set; }
        public short ColorG { get; set; }
        public short ColorB { get; set; }

        public virtual ICollection<AdminLevelNames> AdminLevelNames { get; set; }
        public virtual ICollection<Commands> Commands { get; set; }
        public virtual ICollection<Players> Players { get; set; }
    }
}
