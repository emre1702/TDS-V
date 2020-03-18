using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Defaults;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Entities.TeamSystem;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class Lobby
    {
        public bool SavePlayerLobbyStats { get; set; } = true;
        public bool SetPositionOnPlayerAdd => !IsGangActionLobby;
        public bool SpawnPlayer => SetPositionOnPlayerAdd;
        public bool FreezePlayerOnCountdown => !SetPositionOnPlayerAdd;

        public virtual async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (Entity.Type != LobbyType.MainMenu && !IsGangActionLobby)
            {
                if (await IsPlayerBaned(player).ConfigureAwait(true))
                    return false;
            }

            #region Remove from old lobby

            ILobby? oldlobby = player.Lobby;
            oldlobby?.RemovePlayer(player);

            #endregion Remove from old lobby

            if (Entity.Type != LobbyType.MainMenu
                && Entity.Type != LobbyType.MapCreateLobby)
            {
                await AddPlayerLobbyStats(player).ConfigureAwait(true);
            }

            player.Lobby = this;
            Players.Add(player);

            if (Entity.Type == LobbyType.MainMenu
                || Entity.Type == LobbyType.MapCreateLobby
                || Entity.Type == LobbyType.GangLobby)
                player.ModPlayer?.SetInvincible(true);

            player.ModPlayer!.Dimension = Dimension;
            if (SetPositionOnPlayerAdd)
                player.ModPlayer.Position = SpawnPoint.Around(Entity.AroundSpawnPoint);
            player.ModPlayer.Freeze(true);

            if (teamindex != null)
                player.Team = Teams[(int)teamindex.Value];

            DataSyncHandler.SetData(player, PlayerDataKey.IsLobbyOwner, PlayerDataSyncMode.Player, IsPlayerLobbyOwner(player));

            ModAPI.Sync.SendEvent(ToClientEvent.JoinSameLobby, player.RemoteId);

            player.SendEvent(ToClientEvent.JoinLobby, SyncedLobbySettings.Json, Serializer.ToClient(Players.Select(p => p.RemoteId).ToList()),
                                                                                 Serializer.ToClient(Teams.Select(t => t.SyncedTeamData)));

            if (Entity.Type != LobbyType.MainMenu)
            {
                LoggingHandler.LogRest(LogType.Lobby_Join, player, false, Entity.IsOfficial);
                player.SendNotification(string.Format(player.Language.JOINED_LOBBY_MESSAGE, Entity.Name, PlayerCommand.LobbyLeave));
            }

            EventsHandler.OnLobbyJoin(player, this);
            return true;
        }

        public virtual async void RemovePlayer(ITDSPlayer player)
        {
            Players.Remove(player);

            player.Lobby = null;
            player.PreviousLobby = this;
            await player.ExecuteForDB((dbContext) =>
            {
                player.LobbyStats = null;
            }).ConfigureAwait(false);
            player.Lifes = 0;
            player.Team?.SyncRemovedPlayer(player);
            player.Team = null;
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

            if (IsEmpty())
            {
                if (Entity.IsTemporary)
                    Remove();
            }

            ModAPI.Sync.SendEvent(ToClientEvent.LeaveSameLobby,  player.RemoteId, player.DisplayName);
            if (Entity.Type != LobbyType.MainMenu)
                LoggingHandler.LogRest(LogType.Lobby_Leave, player, false, Entity.IsOfficial);

            EventsHandler.OnLobbyLeave(player, this);
        }

        private async Task AddPlayerLobbyStats(ITDSPlayer player)
        {
            if (player.Entity is null)
                return;

            await player.ExecuteForDBAsync(async (dbContext) =>
            {
                PlayerLobbyStats? stats = await dbContext.PlayerLobbyStats.FindAsync(player.Entity.Id, Entity.Id);
                if (stats is null)
                {
                    stats = new PlayerLobbyStats { LobbyId = Entity.Id };
                    player.Entity.PlayerLobbyStats.Add(stats);
                    await dbContext.SaveChangesAsync();
                }
                player.LobbyStats = stats;
            }).ConfigureAwait(false);
        }

        public virtual void SetPlayerTeam(ITDSPlayer player, ITeam team)
        {
            if (player.Team is { })
            {
                if (player.Team == team)
                    return;
                var oldTeam = player.Team;
                player.Team = null;
                oldTeam.SyncRemovedPlayer(player);
            }

            player.Team = team;
            team.SyncAddedPlayer(player);
        }

        public bool IsPlayerLobbyOwner(ITDSPlayer player)
        {
            if (player.Entity is null)
                return false;
            return player.Lobby is Lobby lobby && lobby == this && Entity.OwnerId == player.Entity.Id;
        }

        public ITDSPlayer? GetOwner()
        {
            return GetPlayerById(Entity.OwnerId);
        }

        public ITDSPlayer? GetPlayerById(int id)
        {
            return Players.FirstOrDefault(p => p.Entity!.Id == id);
        }
    }
}
