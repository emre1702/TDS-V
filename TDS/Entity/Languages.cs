using System;
using System.Collections.Generic;

namespace TDS.Entity
{
    public partial class Languages
    {
        public Languages()
        {
            Adminlevelnames = new HashSet<Adminlevelnames>();
            Playersettings = new HashSet<Playersettings>();
        }

        public byte Id { get; set; }
        public string Language { get; set; }

        public ICollection<Adminlevelnames> Adminlevelnames { get; set; }
        public ICollection<Playersettings> Playersettings { get; set; }
    }
}
