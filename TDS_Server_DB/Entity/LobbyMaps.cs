using System;
using System.Collections.Generic;

namespace TDS_Server.Entity
{
    public partial class LobbyMaps
    {
        public uint LobbyId { get; set; }
        public int MapId { get; set; }

        public virtual Lobbies Lobby { get; set; }
        public virtual Maps Map { get; set; }
    }
}
