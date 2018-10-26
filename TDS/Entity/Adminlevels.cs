using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Adminlevels
    {
        public Adminlevels()
        {
            Adminlevelnames = new HashSet<Adminlevelnames>();
        }

        public byte Level { get; set; }
        public string FontColor { get; set; }

        public ICollection<Adminlevelnames> Adminlevelnames { get; set; }
    }
}
