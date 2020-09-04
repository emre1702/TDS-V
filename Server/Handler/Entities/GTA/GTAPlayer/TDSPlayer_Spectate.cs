using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        private ITDSPlayer? _spectates;

        #region Public Properties

        public override ITDSPlayer? Spectates
        {
            get => _spectates;
            set
            {
                _spectateHandler.SetPlayerToSpectatePlayer(this, value);
                _spectates = value;
            }
        }

        public override HashSet<ITDSPlayer> Spectators { get; } = new HashSet<ITDSPlayer>();

        #endregion Public Properties
    }
}
