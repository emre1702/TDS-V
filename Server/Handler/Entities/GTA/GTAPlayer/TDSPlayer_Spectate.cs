using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Internal;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Entities.GTA.GTAPlayer
{
    partial class TDSPlayer
    {
        private readonly HashSet<ITDSPlayer> _spectators = new HashSet<ITDSPlayer>();

        public override void SetSpectates(ITDSPlayer? target)
        {
            _spectateHandler.SetPlayerToSpectatePlayer(this, target);
            Spectates = target;
        }

        public override void AddSpectator(ITDSPlayer spectator)
        {
            lock (_spectators)
                _spectators.Add(spectator);
        }

        public override void RemoveSpectator(ITDSPlayer spectator)
        {
            lock (_spectators)
                _spectators.Remove(spectator);
        }

        public override bool HasSpectators()
        {
            lock (_spectators)
                return _spectators.Any();
        }

        public override List<ITDSPlayer> GetSpectators()
        {
            lock (_spectators)
                return _spectators.ToList();
        }
    }
}
