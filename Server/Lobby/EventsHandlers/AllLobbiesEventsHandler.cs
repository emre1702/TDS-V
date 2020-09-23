using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Handler.Events;
using TDS_Server.LobbySystem.Lobbies.Abstracts;

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
            var task = player.LobbyNew?.Deathmatch.OnPlayerDeath(player, killer, reason);
            if (task is { })
                await task;
        }

        private void EventsHandler_PlayerEnteredColshape(ITDSColShape colshape, ITDSPlayer player)
            => player.LobbyNew?.ColshapesHandler.OnPlayerEnterColshape(colshape, player);

        private ValueTask EventsHandler_PlayerLoggedOut(ITDSPlayer player)
            => new ValueTask(player.LobbyNew?.Players.OnPlayerLoggedOut(player) ?? Task.CompletedTask);

        private void EventsHandler_PlayerSpawned(ITDSPlayer player)
            => player.LobbyNew?.Deathmatch.OnPlayerSpawned(player);

        private void EventsHandler_PlayerWeaponSwitch(ITDSPlayer player, WeaponHash oldWeapon, WeaponHash newWeapon)
        {
            if (!(player.LobbyNew is RoundFightLobby roundFightLobby))
                return;
            roundFightLobby.Weapons.OnPlayerWeaponSwitch(player, oldWeapon, newWeapon);
        }
    }
}
