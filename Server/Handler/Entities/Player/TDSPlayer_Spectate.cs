using System.Collections.Generic;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        private TDSPlayer? _spectates;

        public TDSPlayer? Spectates
        {
            get => _spectates;
            set
            {
                _spectateHandler.SetPlayerToSpectatePlayer(this, value);
                _spectates = value;
            }
        }
        public HashSet<TDSPlayer> Spectators { get; } = new HashSet<TDSPlayer>();
    }
}
