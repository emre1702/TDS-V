using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Database.Entity.Player;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;

namespace TDS_Server.LobbySystem.Players
{
    public class BaseLobbyPlayers : IBaseLobbyPlayers
    {
        protected IBaseLobbyEventsHandler Events { get; }

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly List<ITDSPlayer> _players = new List<ITDSPlayer>();

        private readonly BaseLobbyTeamsHandler _teams;
        protected readonly IBaseLobby Lobby;
        private readonly BaseLobbyBansHandler _bans;

        public BaseLobbyPlayers(IBaseLobby lobby, IBaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams, BaseLobbyBansHandler bans)
        {
            Events = events;
            _teams = teams;
            Lobby = lobby;
            _bans = bans;

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
            if (await _bans.CheckIsBanned(player))
                return false;

            player.Lobby?.RemovePlayer(player);
            await _semaphore.Do(() => _players.Add(player));

            Events.TriggerPlayerJoined(player, teamIndex);
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
            player.Lifes = 0;
            _teams.SetPlayerTeam(player, null);
            player.SetSpectates(null);

            await Events.TriggerPlayerLeft(player);

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
