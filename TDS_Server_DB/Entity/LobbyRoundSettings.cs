using System;
using System.Collections.Generic;

namespace TDS_Server_DB.Entity
{
    public partial class LobbyRoundSettings
    {
        public int LobbyId { get; set; }
        public int RoundTime { get; set; }
        public int CountdownTime { get; set; }
        public int BombDetonateTimeMs { get; set; }
        public int BombDefuseTimeMs { get; set; }
        public int BombPlantTimeMs { get; set; }
        public bool MixTeamsAfterRound { get; set; }

        public virtual Lobbies Lobby { get; set; }
    }
}
