using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class Adminlevels
    {
        public Adminlevels()
        {
            Adminlevelnames = new HashSet<Adminlevelnames>();
            Commands = new HashSet<Commands>();
        }

        public byte Level { get; set; }
        public sbyte ColorR { get; set; }
        public sbyte ColorG { get; set; }
        public sbyte ColorB { get; set; }

        public virtual ICollection<Adminlevelnames> Adminlevelnames { get; set; }
        public virtual ICollection<Commands> Commands { get; set; }
    }
}
