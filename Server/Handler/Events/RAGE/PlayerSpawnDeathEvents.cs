using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Events.RAGE
{
    public class PlayerSpawnDeathEvents : Script
    {
        [ServerEvent(Event.PlayerSpawn)]
        public void PlayerSpawn(ITDSPlayer player)
        {
            EventsHandler.Instance.OnPlayerSpawn(player);
        }

        [ServerEvent(Event.PlayerDeath)]
        public void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint reason)
        {
            EventsHandler.Instance.OnPlayerDeath(player, killer, reason);
        }
    }
}
