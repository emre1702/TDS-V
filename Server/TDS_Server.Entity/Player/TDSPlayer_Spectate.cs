using System.Collections.Generic;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        private TDSPlayer? _spectates;

        public TDSPlayer? Spectates
        {
            get => _spectates;
            set
            {
                SpectateSystem.SetPlayerToSpectatePlayer(this, value);
                _spectates = value;
            }
        }
        public HashSet<TDSPlayer> Spectators { get; } = new HashSet<TDSPlayer>();
    }
}
