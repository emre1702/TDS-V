using System;
using System.Collections.Generic;

namespace TDS.Entities
{
    public partial class Players
    {
        public Players()
        {
            Playerlobbystats = new HashSet<Playerlobbystats>();
        }

        public uint Id { get; set; }
        public string Scname { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? AllowDataTransfer { get; set; }

        public Playersettings Playersettings { get; set; }
        public ICollection<Playerlobbystats> Playerlobbystats { get; set; }
    }
}
