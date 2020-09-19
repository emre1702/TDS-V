using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.EventsHandlers;
using TDS_Server.LobbySystem.TeamHandlers;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Players
{
    public class BaseLobbyPlayers
    {
        protected IBaseLobbyEventsHandler Events { get; }

        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly List<ITDSPlayer> _players = new List<ITDSPlayer>();

        private readonly BaseLobbyTeamsHandler _teams;
        private readonly LobbyDb _entity;

        public BaseLobbyPlayers(LobbyDb entity, IBaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams)
        {
            Events = events;
            _teams = teams;
            _entity = entity;

            events.LobbyRemove += async _ => await RemoveAllPlayers();
        }

        public virtual async Task<bool> AddPlayer(ITDSPlayer player, uint? teamIndex)
        {
        }

        public virtual async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            var playerHasBeenInLobby = await _semaphore.DoAsync(() => _players.Remove(player));
            if (!playerHasBeenInLobby)
                return false;

            player.Lobby = null;
            player.PreviousLobby = null;
            await player.SetPlayerLobbyStats(null);
            player.Lifes = 0;
            _teams.SetPlayerTeam(player, null);
            player.SetSpectates(null);

            await Events.TriggerPlayerLeftLobby(player);

            return true;
        }

        public bool IsLobbyOwner(ITDSPlayer player)
        {
            if (player.Entity is null)
                return false;
            return player.Lobby == this && _entity.OwnerId == player.Entity.Id;
        }

        public Task<ITDSPlayer?> GetLobbyOwner()
        {
            return _semaphore.DoAsync(() => (ITDSPlayer?)_players.FirstOrDefault(p => p.Id == _entity.OwnerId));
        }

        public Task<bool> Any() => _semaphore.DoAsync(() => _players.Any());

        public Task Do(Action<ITDSPlayer> func)
        {
            return _semaphore.DoAsync(() =>
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

        private async Task RemoveAllPlayers()
        {
            foreach (var player in _players.ToList())
                await RemovePlayer(player);
        }
    }
}

/*
 *
 * using GTANetworkAPI;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Core;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        public bool SavePlayerLobbyStats { get; set; } = true;
        public bool SetPositionOnPlayerAdd => !IsGangActionLobby && !(this is GangLobby);
        public bool SpawnPlayer => SetPositionOnPlayerAdd;

        public virtual async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (Entity.Type != LobbyType.MainMenu && !IsGangActionLobby)
            {
                if (await IsPlayerBaned(player).ConfigureAwait(true))
                    return false;
            }

            ILobby? oldlobby = player.Lobby;
            if (oldlobby is { })
                await oldlobby.RemovePlayer(player);

            if (Entity.Type != LobbyType.MainMenu
                && Entity.Type != LobbyType.MapCreateLobby
                && Entity.Type != LobbyType.CharCreateLobby)
            {
                await AddPlayerLobbyStats(player).ConfigureAwait(true);
            }

            Players.TryAdd(player.Id, player);

            NAPI.Task.Run(() =>
            {
                TriggerEvent(ToClientEvent.JoinSameLobby, player.RemoteId);

                player.Lobby = this;

                if (Entity.Type == LobbyType.MainMenu
                    || Entity.Type == LobbyType.MapCreateLobby
                    || Entity.Type == LobbyType.GangLobby
                    || Entity.Type == LobbyType.CharCreateLobby)
                    player.SetInvincible(true);

                player.Dimension = Dimension;
                if (SetPositionOnPlayerAdd)
                    player.Position = SpawnPoint.Around(Entity.AroundSpawnPoint);
                player.Freeze(true);

                if (teamindex != null)
                    SetPlayerTeam(player, Teams[(int)teamindex.Value]);

                DataSyncHandler.SetData(player, PlayerDataKey.IsLobbyOwner, DataSyncMode.Player, IsPlayerLobbyOwner(player));

                player.TriggerEvent(ToClientEvent.JoinLobby, SyncedLobbySettings.Json, Serializer.ToClient(Players.Values.Select(p => p.RemoteId).ToList()),
                                                                                     Serializer.ToClient(Teams.Select(t => t.SyncedTeamData)));

                if (Entity.Type != LobbyType.MainMenu)
                {
                    LoggingHandler?.LogRest(LogType.Lobby_Join, player, false, Entity.IsOfficial);
                    player.SendNotification(string.Format(player.Language.JOINED_LOBBY_MESSAGE, Entity.Name, PlayerCommand.LobbyLeave));
                }

                EventsHandler.OnLobbyJoin(player, this);
            });

            return true;
        }

        public ITDSPlayer? GetPlayerById(int id)
        {
            return Players.Values.FirstOrDefault(p => p.Entity!.Id == id);
        }

        public virtual void SetPlayerTeam(ITDSPlayer player, ITeam? team)
        {
            if (player.Team is { })
            {
                if (player.Team == team)
                    return;
                player.Team.SyncRemovedPlayer(player);
            }

            player.SetTeam(team, true);
            team?.SyncAddedPlayer(player);
        }

        private async Task AddPlayerLobbyStats(ITDSPlayer player)
        {
            if (player.Entity is null)
                return;

            PlayerLobbyStats? stats = null;
            await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                stats = await dbContext.PlayerLobbyStats.FindAsync(player.Entity.Id, Entity.Id);
                if (stats is null)
                {
                    stats = new PlayerLobbyStats { LobbyId = Entity.Id };
                    player.Entity.PlayerLobbyStats.Add(stats);
                    await dbContext.SaveChangesAsync();
                }
            }).ConfigureAwait(false);
            await player.SetPlayerLobbyStats(stats);
        }
    }
}
*/
