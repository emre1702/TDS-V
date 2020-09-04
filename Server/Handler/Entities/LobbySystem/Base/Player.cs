﻿using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        #region Public Properties

        public bool FreezePlayerOnCountdown => SetPositionOnPlayerAdd;
        public bool SavePlayerLobbyStats { get; set; } = true;
        public bool SetPositionOnPlayerAdd => !IsGangActionLobby && !(this is GangLobby);
        public bool SpawnPlayer => SetPositionOnPlayerAdd;

        #endregion Public Properties

        #region Public Methods

        public virtual async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (Entity.Type != LobbyType.MainMenu && !IsGangActionLobby)
            {
                if (await IsPlayerBaned(player).ConfigureAwait(true))
                    return false;
            }

            #region Remove from old lobby

            ILobby? oldlobby = player.Lobby;
            if (oldlobby is { })
                await oldlobby.RemovePlayer(player);

            #endregion Remove from old lobby

            if (Entity.Type != LobbyType.MainMenu
                && Entity.Type != LobbyType.MapCreateLobby
                && Entity.Type != LobbyType.CharCreateLobby)
            {
                await AddPlayerLobbyStats(player).ConfigureAwait(true);
            }

            Players.TryAdd(player.Id, player);

            ModAPI.Thread.QueueIntoMainThread(() =>
            {
                ModAPI.Sync.TriggerEvent(this, ToClientEvent.JoinSameLobby, player.RemoteId);

                player.Lobby = this;

                if (Entity.Type == LobbyType.MainMenu
                    || Entity.Type == LobbyType.MapCreateLobby
                    || Entity.Type == LobbyType.GangLobby
                    || Entity.Type == LobbyType.CharCreateLobby)
                    player.ModPlayer?.SetInvincible(true);

                player.ModPlayer!.Dimension = Dimension;
                if (SetPositionOnPlayerAdd)
                    player.ModPlayer.Position = SpawnPoint.Around(Entity.AroundSpawnPoint);
                player.ModPlayer.Freeze(true);

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

        public ITDSPlayer? GetOwner()
        {
            Players.TryGetValue(Entity.OwnerId, out ITDSPlayer? owner);
            return owner;
        }

        public ITDSPlayer? GetPlayerById(int id)
        {
            return Players.Values.FirstOrDefault(p => p.Entity!.Id == id);
        }

        public bool IsPlayerLobbyOwner(ITDSPlayer player)
        {
            if (player.Entity is null)
                return false;
            return player.Lobby is Lobby lobby && lobby == this && Entity.OwnerId == player.Entity.Id;
        }

        public virtual async Task RemovePlayer(ITDSPlayer player)
        {
            if (!Players.TryRemove(player.Id, out _))
                return;

            player.Lobby = null;
            player.PreviousLobby = this;
            await player.SetPlayerLobbyStats(null);
            ModAPI.Thread.QueueIntoMainThread(() =>
            {
                player.Lifes = 0;
                SetPlayerTeam(player, null);
                player.Spectates = null;
                if (player.ModPlayer is { })
                {
                    player.ModPlayer.Freeze(true);
                    player.ModPlayer.Transparency = 255;
                }

                if (DeathSpawnTimer.ContainsKey(player))
                {
                    DeathSpawnTimer[player].Kill();
                    DeathSpawnTimer.Remove(player);
                }
            });

            if (IsEmpty())
            {
                if (Entity.IsTemporary)
                    await Remove();
            }

            ModAPI.Thread.QueueIntoMainThread(() =>
            {
                ModAPI.Sync.TriggerEvent(ToClientEvent.LeaveSameLobby, player.RemoteId, player.Entity?.Name ?? player.DisplayName);
                if (Entity.Type != LobbyType.MainMenu)
                    LoggingHandler?.LogRest(LogType.Lobby_Leave, player, false, Entity.IsOfficial);

                EventsHandler.OnLobbyLeave(player, this);
            });
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

        #endregion Public Methods

        #region Private Methods

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

        #endregion Private Methods
    }
}