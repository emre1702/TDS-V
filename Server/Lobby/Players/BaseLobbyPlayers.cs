using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Utility;

namespace TDS_Server.LobbySystem.Players
{
    public class BaseLobbyPlayers : IBaseLobbyPlayers
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly List<ITDSPlayer> _players = new List<ITDSPlayer>();

        protected readonly IBaseLobby Lobby;

        public BaseLobbyPlayers(IBaseLobby lobby, IBaseLobbyEventsHandler events)
        {
            Lobby = lobby;

            events.Remove += async _ => await RemoveAllPlayers();
            events.NewBan += Events_NewBan;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="player"></param>
        /// <param name="teamIndex">Use -1 to choose a random non-spectator team.</param>
        /// <returns></returns>
        public virtual async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex = 0)
        {
            if (await Lobby.Bans.CheckIsBanned(player))
                return false;

            player.Lobby?.RemovePlayer(player);
            await _semaphore.Do(() => _players.Add(player));

            await Lobby.Events.TriggerPlayerJoined(player, teamIndex);
            player.SetLobby(Lobby);
            InformAboutHowToLeaveLobby(player);

            return true;
        }

        protected virtual void InformAboutHowToLeaveLobby(ITDSPlayer player)
        {
            NAPI.Task.Run(() =>
            {
                player.SendNotification(string.Format(player.Language.JOINED_LOBBY_MESSAGE, Lobby.Entity.Name, PlayerCommand.LobbyLeave));
            });
        }

        public virtual async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            var playerHasBeenInLobby = await _semaphore.Do(() => _players.Remove(player));
            if (!playerHasBeenInLobby)
                return false;

            player.Lobby = null;
            player.PreviousLobby = null;
            await player.SetPlayerLobbyStats(null);
            var lifes = player.Lifes;
            player.Lifes = 0;
            await Lobby.Teams.SetPlayerTeam(player, null);
            player.SetSpectates(null);

            await Lobby.Events.TriggerPlayerLeft(player, lifes);

            return true;
        }

        public bool IsLobbyOwner(ITDSPlayer player)
        {
            if (player.Entity is null)
                return false;
            return player.Lobby == this && Lobby.Entity.OwnerId == player.Entity.Id;
        }

        public Task<bool> Any() => _semaphore.Do(() => _players.Any());

        public List<ITDSPlayer> GetPlayers() => _players.ToList();

        public int Count => _players.Count;

        public Task<ITDSPlayer?> GetFirst(ITDSPlayer exceptPlayer) => _semaphore.Do(() => (ITDSPlayer?)_players.FirstOrDefault(p => p != exceptPlayer));

        public virtual Task OnPlayerLoggedOut(ITDSPlayer player)
            => RemovePlayer(player);

        public Task Do(Action<ITDSPlayer> func)
        {
            return _semaphore.Do(() =>
            {
                foreach (var player in _players)
                {
                    func(player);
                }
            });
        }

        public void DoWithoutLock(Action<ITDSPlayer> func)
        {
            foreach (var player in _players.ToList())
            {
                func(player);
            }
        }

        public Task DoInMain(Action<ITDSPlayer> func)
        {
            return _semaphore.Do(async () =>
            {
                await NAPI.Task.RunWait(() =>
                {
                    foreach (var player in _players)
                    {
                        func(player);
                    }
                });
            });
        }

        public Task<ITDSPlayer?> GetById(int playerId)
            => _semaphore.Do(() => (ITDSPlayer?)_players.FirstOrDefault(p => p.Id == playerId));

        public Task<ITDSPlayer?> GetLobbyOwner()
            => GetById(Lobby.Entity.OwnerId);

        public Task<IEnumerable<ITDSPlayer>> GetExcept(ITDSPlayer player)
            => _semaphore.Do(() => _players.Where(p => p != player));

        private async Task RemoveAllPlayers()
        {
            foreach (var player in _players.ToList())
                await RemovePlayer(player);
        }

        protected async Task SetNewRandomLobbyOwner()
        {
            var newOwner = await _semaphore.Do(() => SharedUtils.GetRandom(_players.Where(p => p.Entity is { })));
            Lobby.Entity.OwnerId = newOwner.Entity!.Id;
            newOwner.SetLobby(Lobby);
        }

        private async void Events_NewBan(PlayerBans ban)
        {
            await _semaphore.Do(() =>
            {
                var index = _players.FindIndex(p => p.Entity?.Id == ban.PlayerId);
                if (index >= 0)
                    _players.RemoveAt(index);
            });
        }
    }
}
