using System.Collections.Generic;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Shared.Core;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class BaseLobbyDeathmatch
    {
        private Dictionary<ITDSPlayer, TDSTimer> _afterDeathSpawnTimer = new Dictionary<ITDSPlayer, TDSTimer>();

        public BaseLobbyDeathmatch(BaseLobbyEventsHandler events)
        {
            events.PlayerLeftLobbyAfter += ResetPlayer;
        }

        protected virtual void ResetPlayer(ITDSPlayer player)
        {
            RemoveAfterDeathSpawnTimer(player);
        }

        private void RemoveAfterDeathSpawnTimer(ITDSPlayer player)
        {
            lock (_afterDeathSpawnTimer)
            {
                if (_afterDeathSpawnTimer.ContainsKey(player))
                {
                    _afterDeathSpawnTimer[player].Kill();
                    _afterDeathSpawnTimer.Remove(player);
                }
            }
        }
    }
}
