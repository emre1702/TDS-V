using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class AdminLevels
    {
        public AdminLevels()
        {
            AdminLevelNames = new HashSet<AdminLevelNames>();
            Commands = new HashSet<Commands>();
            Players = new HashSet<Players>();
        }

        public byte Level { get; set; }
        public byte ColorR { get; set; }
        public byte ColorG { get; set; }
        public byte ColorB { get; set; }

        public virtual ICollection<AdminLevelNames> AdminLevelNames { get; set; }
        public virtual ICollection<Commands> Commands { get; set; }
        public virtual ICollection<Players> Players { get; set; }
    }
}
