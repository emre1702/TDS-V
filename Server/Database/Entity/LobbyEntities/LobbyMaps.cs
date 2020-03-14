﻿using TDS_Server.Database.Entity.Rest;

namespace TDS_Server.Database.Entity.LobbyEntities
{
    public partial class LobbyMaps
    {
        public int LobbyId { get; set; }
        public int MapId { get; set; }

        public virtual Lobbies Lobby { get; set; }
        public virtual Maps Map { get; set; }
    }
}
