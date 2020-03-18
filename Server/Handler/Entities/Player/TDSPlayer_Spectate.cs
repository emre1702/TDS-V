using System.Collections.Generic;
using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler.Entities.Player
{
    partial class TDSPlayer
    {
        private ITDSPlayer? _spectates;

        public ITDSPlayer? Spectates
        {
            get => _spectates;
            set
            {
                _spectateHandler.SetPlayerToSpectatePlayer(this, value);
                _spectates = value;
            }
        }
        public HashSet<ITDSPlayer> Spectators { get; } = new HashSet<ITDSPlayer>();
    }
}
