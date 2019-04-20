using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class LobbyTypes
    {
        public LobbyTypes()
        {
            Lobbies = new HashSet<Lobbies>();
        }

        public byte Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Lobbies> Lobbies { get; set; }
    }
}
