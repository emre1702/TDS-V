using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Defaults;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Players;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler;
using TDS.Server.Handler.Extensions;
using TDS.Shared.Data.Utility;

namespace TDS.Server.LobbySystem.Players
{
    public class BaseLobbyPlayers : IBaseLobbyPlayers, IDisposable
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly List<ITDSPlayer> _players = new List<ITDSPlayer>();

        protected IBaseLobby Lobby { get; }
        protected IBaseLobbyEventsHandler Events { get; }

        public BaseLobbyPlayers(IBaseLobby lobby, IBaseLobbyEventsHandler events)
        {
            Lobby = lobby;
            Events = events;

            events.Remove += OnLobbyRemove;
            events.NewBan += Events_NewBan;
            events.RemoveAfter += RemoveEvents;
        }

        protected virtual void RemoveEvents(IBaseLobby lobby)
        {
            if (Events.Remove is { })
                Events.Remove -= OnLobbyRemove;
            Events.NewBan -= Events_NewBan;
            Events.RemoveAfter -= RemoveEvents;
        }

        private async Task OnLobbyRemove(IBaseLobby lobby)
        {
            await RemoveAllPlayers().ConfigureAwait(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="player"></param>
        /// <param name="teamIndex">Use -1 to choose a random non-spectator team.</param>
        /// <returns></returns>
        public virtual async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex = 0)
        {
            await Lobby.IsCreatingTask.Task.ConfigureAwait(false);
            if (await Lobby.Bans.CheckIsBanned(player).ConfigureAwait(false))
                return false;

            await (player.Lobby?.Players.RemovePlayer(player) ?? Task.CompletedTask).ConfigureAwait(false);
            await _semaphore.Do(() => _players.Add(player)).ConfigureAwait(false);

            await Lobby.Events.TriggerPlayerJoined(player, teamIndex).ConfigureAwait(false);
            player.LobbyHandler.SetLobby(Lobby);
            InformAboutHowToLeaveLobby(player);

            return true;
        }

        protected virtual void InformAboutHowToLeaveLobby(ITDSPlayer player)
        {
            NAPI.Task.RunSafe(() =>
            {
                player.SendNotification(string.Format(player.Language.JOINED_LOBBY_MESSAGE, Lobby.Entity.Name, UserCommand.LobbyLeave));
            });
        }

        public virtual async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            var playerHasBeenInLobby = await _semaphore.Do(() => _players.Remove(player)).ConfigureAwait(false);
            if (!playerHasBeenInLobby)
                return false;

            player.LobbyHandler.SetLobby(null);
            await player.LobbyHandler.SetPlayerLobbyStats(null).ConfigureAwait(false);
            var lifes = player.Lifes;
            player.Lifes = 0;
            Lobby.Teams.SetPlayerTeam(player, null);
            player.SpectateHandler.SetSpectates(null);

            await Lobby.Events.TriggerPlayerLeft(player, lifes).ConfigureAwait(false);

            return true;
        }

        public bool IsLobbyOwner(ITDSPlayer player)
        {
            if (player.Entity is null)
                return false;
            return player.Lobby == Lobby && Lobby.Entity.OwnerId == player.Entity.Id;
        }

        public Task<bool> Any() => _semaphore.Do(() => _players.Any());

        public Task<bool> Any(Func<ITDSPlayer, bool> func) => _semaphore.Do(() => _players.Any(func));

        public List<ITDSPlayer> GetPlayers() => _players.ToList();

        public int Count => _players.Count;

        public Task<ITDSPlayer?> GetFirst(ITDSPlayer exceptPlayer) => _semaphore.Do(() => (ITDSPlayer?)_players.FirstOrDefault(p => p != exceptPlayer));

        public virtual Task OnPlayerLoggedOut(ITDSPlayer player)
            => RemovePlayer(player);

        public Task DoForAll(Action<ITDSPlayer> func)
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
                }).ConfigureAwait(false);
            });
        }

        public Task<ITDSPlayer?> GetById(int playerId)
            => _semaphore.Do(() => (ITDSPlayer?)_players.FirstOrDefault(p => p.Id == playerId));

        public Task<ITDSPlayer?> GetLobbyOwner()
            => GetById(Lobby.Entity.OwnerId);

        public Task<IEnumerable<ITDSPlayer>> GetExcept(ITDSPlayer player)
            => _semaphore.Do(() => _players.Where(p => p != player));

        public Task<IOrderedEnumerable<string>> GetOrderedNames()
            => _semaphore.Do(() => _players.Select(p => p.DisplayName).OrderBy(n => n));

        private async Task RemoveAllPlayers()
        {
            foreach (var player in _players.ToList())
                await RemovePlayer(player).ConfigureAwait(false);
        }

        protected async Task SetNewRandomLobbyOwner()
        {
            var newOwner = await _semaphore.Do(() => SharedUtils.GetRandom(_players.Where(p => p.Entity is { }))).ConfigureAwait(false);
            Lobby.Entity.OwnerId = newOwner.Entity!.Id;
            newOwner.LobbyHandler.SyncLobbyOwnerInfo();
        }

        private async void Events_NewBan(PlayerBans ban)
        {
            try
            {
                await _semaphore.Do(() =>
                {
                    var index = _players.FindIndex(p => p.Entity?.Id == ban.PlayerId);
                    if (index >= 0)
                        _players.RemoveAt(index);
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance.LogError(ex);
            }
        }

        public void Dispose()
        {
            _players.Clear();
            _semaphore.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
