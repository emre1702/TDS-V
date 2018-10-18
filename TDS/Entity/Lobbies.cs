using System;
using System.Collections.Generic;

namespace TDS.Entities
{
    public partial class Lobbies
    {
        public Lobbies()
        {
            Playerlobbystats = new HashSet<Playerlobbystats>();
        }

        public uint Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool? IsTemporary { get; set; }
        public DateTime? CreateTimestamp { get; set; }

        public ICollection<Playerlobbystats> Playerlobbystats { get; set; }
    }
}
