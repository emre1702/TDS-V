using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Players
{
    internal class BaseLobbyPlayers
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly List<ITDSPlayer> _players = new List<ITDSPlayer>();

        private readonly BaseLobbyTeamsHandler _teams;

        public BaseLobbyPlayers(BaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams)
        {
            _teams = teams;

            events.LobbyRemove += async _ => await RemoveAllPlayers();
        }

        protected virtual async Task RemovePlayer(ITDSPlayer player)
        {
            var playerHasBeenInLobby = await _semaphore.DoAsync(() => _players.Remove(player));
            if (!playerHasBeenInLobby)
                return;

            player.Lobby = null;
            player.PreviousLobby = null;
            await player.SetPlayerLobbyStats(null);
            player.Lifes = 0;
            _teams.SetPlayerTeam(player, null);
            player.SetSpectates(null);
        }

        private async Task RemoveAllPlayers()
        {
            foreach (var player in _players.ToList())
                await RemovePlayer(player);
        }
    }
}
