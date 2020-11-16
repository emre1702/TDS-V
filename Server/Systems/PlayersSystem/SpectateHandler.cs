using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.PlayersSystem;

namespace TDS.Server.PlayersSystem
{
    public class SpectateHandler : IPlayerSpectateHandler
    {
        private readonly HashSet<ITDSPlayer> _spectators = new HashSet<ITDSPlayer>();

        private readonly Handler.SpectateHandler _globalSpectateHandler;

#nullable disable
        private ITDSPlayer _player;
#nullable enable

        public SpectateHandler(Handler.SpectateHandler globalSpectateHandler)
        {
            _globalSpectateHandler = globalSpectateHandler;
        }

        public void Init(ITDSPlayer player)
        {
            _player = player;
        }

        public void SetSpectates(ITDSPlayer? target)
        {
            _globalSpectateHandler.SetPlayerToSpectatePlayer(_player, target);
            _player.Spectates = target;
        }

        public void AddSpectator(ITDSPlayer spectator)
        {
            lock (_spectators)
                _spectators.Add(spectator);
        }

        public void RemoveSpectator(ITDSPlayer spectator)
        {
            lock (_spectators)
                _spectators.Remove(spectator);
        }

        public bool HasSpectators()
        {
            lock (_spectators)
                return _spectators.Any();
        }

        public List<ITDSPlayer> GetSpectators()
        {
            lock (_spectators)
                return _spectators.ToList();
        }
    }
}
