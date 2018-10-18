using System;
using System.Collections.Generic;

namespace TDS.Entities
{
    public partial class Playerlobbystats
    {
        public uint Id { get; set; }
        public uint Lobby { get; set; }
        public uint Kills { get; set; }
        public uint Deaths { get; set; }
        public uint Assists { get; set; }
        public uint Damage { get; set; }

        public Players IdNavigation { get; set; }
        public Lobbies LobbyNavigation { get; set; }
    }
}
