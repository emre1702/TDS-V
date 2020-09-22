using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Handler.Events;

namespace TDS_Server.LobbySystem.EventsHandlers
{
    public class AllLobbiesEventsHandler
    {
        public AllLobbiesEventsHandler(EventsHandler eventsHandler)
        {
            eventsHandler.PlayerDeath += EventsHandler_PlayerDeath;
            eventsHandler.PlayerEnteredColshape += EventsHandler_PlayerEnteredColshape;
            eventsHandler.PlayerLoggedOutBefore += EventsHandler_PlayerLoggedOut;
            eventsHandler.PlayerSpawned += EventsHandler_PlayerSpawned;
        }

        private void EventsHandler_PlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint reason)
            => player.LobbyNew?.Deathmatch.OnPlayerDeath(player, killer, reason);

        private void EventsHandler_PlayerEnteredColshape(ITDSColShape colshape, ITDSPlayer player)
            => player.LobbyNew?.ColshapesHandler.OnPlayerEnterColshape(colshape, player);

        private ValueTask EventsHandler_PlayerLoggedOut(ITDSPlayer player)
            => new ValueTask(player.LobbyNew?.Players.OnPlayerLoggedOut(player) ?? Task.CompletedTask);

        private void EventsHandler_PlayerSpawned(ITDSPlayer player)
            => player.LobbyNew?.Deathmatch.OnPlayerSpawned(player);
    }
}
