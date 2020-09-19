using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Players
{
    public class BaseLobbyPlayers
    {
        protected BaseLobbyEventsHandler Events { get; }

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly List<ITDSPlayer> _players = new List<ITDSPlayer>();

        private readonly BaseLobbyTeamsHandler _teams;
        private readonly LobbyDb _entity;

        public BaseLobbyPlayers(LobbyDb entity, BaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams)
        {
            Events = events;
            _teams = teams;
            _entity = entity;

            events.LobbyRemove += async _ => await RemoveAllPlayers();
        }

        public virtual async Task<(bool Worked, bool RemovedLobby)> RemovePlayer(ITDSPlayer player)
        {
            var playerHasBeenInLobby = await _semaphore.DoAsync(() => _players.Remove(player));
            if (!playerHasBeenInLobby)
                return (false, false);

            player.Lobby = null;
            player.PreviousLobby = null;
            await player.SetPlayerLobbyStats(null);
            player.Lifes = 0;
            _teams.SetPlayerTeam(player, null);
            player.SetSpectates(null);

            await Events.TriggerPlayerLeftLobby(player);

            if ()
        }

        public bool IsLobbyOwner(ITDSPlayer player)
        {
            if (player.Entity is null)
                return false;
            return player.Lobby == this && _entity.OwnerId == player.Entity.Id;
        }

        public bool Any() => _players.Any();

        private async Task RemoveAllPlayers()
        {
            foreach (var player in _players.ToList())
                await RemovePlayer(player);
        }
    }
}
