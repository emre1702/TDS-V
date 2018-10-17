using System;
using System.Collections.Generic;

namespace TDS.Entities
{
    public partial class Languages
    {
        public Languages()
        {
            Playersettings = new HashSet<Playersettings>();
        }

        public byte Id { get; set; }
        public string Language { get; set; }

        public ICollection<Playersettings> Playersettings { get; set; }
    }
}
