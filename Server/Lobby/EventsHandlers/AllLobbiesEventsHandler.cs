using GTANetworkAPI;
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
            eventsHandler.PlayerWeaponSwitch += EventsHandler_PlayerWeaponSwitch;
        }

        private async void EventsHandler_PlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint reason)
        {
            // Triggering the event for lobby events handler is handled in Deathmatch.OnPlayerDeath
            var task = player.Lobby?.Deathmatch.OnPlayerDeath(player, killer, reason);
            if (task is { })
                await task.ConfigureAwait(false);
        }

        private void EventsHandler_PlayerEnteredColshape(ITDSColshape colshape, ITDSPlayer player)
            => player.Lobby?.ColshapesHandler.OnPlayerEnterColshape(colshape, player);

        private ValueTask EventsHandler_PlayerLoggedOut(ITDSPlayer player)
            => new ValueTask(player.Lobby?.Players.OnPlayerLoggedOut(player) ?? Task.CompletedTask);

        private void EventsHandler_PlayerSpawned(ITDSPlayer player)
            => player.Lobby?.Events.TriggerPlayerSpawned(player);

        private void EventsHandler_PlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            if (player.Lobby?.Events is FightLobbyEventsHandler events)
                events.TriggerPlayerWeaponSwitch(player, oldWeapon, newWeapon);
        }
    }
}
