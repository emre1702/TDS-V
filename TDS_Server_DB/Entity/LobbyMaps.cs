using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class LobbyMaps
    {
        public int LobbyId { get; set; }
        public int MapId { get; set; }

        public virtual Lobbies Lobby { get; set; }
        public virtual Maps Map { get; set; }
    }
}
