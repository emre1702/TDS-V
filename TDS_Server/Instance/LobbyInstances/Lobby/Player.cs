using GTANetworkAPI;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Default;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity.Player;
using TDS_Server.Manager.EventManager;
using TDS_Common.Manager.Utility;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Instance.Utility;
using TDS_Server.Enums;

namespace TDS_Server.Instance.LobbyInstances
{
    partial class Lobby
    {
        public bool SavePlayerLobbyStats { get; set; } = true;
        public bool SetPositionOnPlayerAdd => !IsGangActionLobby;
        public bool SpawnPlayer => SetPositionOnPlayerAdd;
        public bool FreezePlayerOnCountdown => !SetPositionOnPlayerAdd;

        public virtual async Task<bool> AddPlayer(TDSPlayer character, uint? teamindex)
        {
            if (LobbyEntity.Type != ELobbyType.MainMenu && !IsGangActionLobby)
            {
                if (await IsPlayerBaned(character).ConfigureAwait(true))
                    return false;
            }

            #region Remove from old lobby

            Lobby? oldlobby = character.CurrentLobby;
            oldlobby?.RemovePlayer(character);

            #endregion Remove from old lobby

            if (LobbyEntity.Type != ELobbyType.MainMenu
                && LobbyEntity.Type != ELobbyType.MapCreateLobby)
            {
                await AddPlayerLobbyStats(character).ConfigureAwait(true);
            }

            character.CurrentLobby = this;
            Players.Add(character);

            if (LobbyEntity.Type == ELobbyType.MainMenu
                || LobbyEntity.Type == ELobbyType.MapCreateLobby
                || LobbyEntity.Type == ELobbyType.GangLobby)
                Workaround.SetPlayerInvincible(character.Player!, true);

            character.Player!.Dimension = Dimension;
            if (SetPositionOnPlayerAdd)
                character.Player.Position = SpawnPoint.Around(LobbyEntity.AroundSpawnPoint);
            Workaround.FreezePlayer(character.Player, true);

            if (teamindex != null)
                character.Team = Teams[(int)teamindex.Value];

            PlayerDataSync.SetData(character, EPlayerDataKey.IsLobbyOwner, EPlayerDataSyncMode.Player, IsPlayerLobbyOwner(character));

            SendAllPlayerEvent(DToClientEvent.JoinSameLobby, null, character.Player.Handle.Value);

            NAPI.ClientEvent.TriggerClientEvent(character.Player, DToClientEvent.JoinLobby, _syncedLobbySettings.Json,
                                                                                            Serializer.ToClient(Players.Select(p => p.Player!.Handle.Value).ToList()),
                                                                                            Serializer.ToClient(Teams.Select(t => t.SyncedTeamData)));

            if (LobbyEntity.Type != ELobbyType.MainMenu)
            {
                RestLogsManager.Log(ELogType.Lobby_Join, character.Player, false, LobbyEntity.IsOfficial);
                character.SendNotification(string.Format(character.Language.JOINED_LOBBY_MESSAGE, LobbyEntity.Name, DPlayerCommand.LobbyLeave));
            }

            CustomEventManager.SetPlayerJoinedLobby(character, this);
            return true;
        }

        public virtual async void RemovePlayer(TDSPlayer player)
        {
            Players.Remove(player);

            player.CurrentLobby = null;
            player.PreviousLobby = this;
            await player.ExecuteForDB((dbContext) => 
            {
                player.CurrentLobbyStats = null;
            }).ConfigureAwait(false);
            player.Lifes = 0;
            player.Team?.SyncRemovedPlayer(player);
            player.Team = null;
            player.Spectates = null;
            if (player.Player!.Exists)
            {
                Workaround.FreezePlayer(player.Player, true);
                player.Player.Transparency = 255;
            }
            if (DeathSpawnTimer.ContainsKey(player))
            {
                DeathSpawnTimer[player].Kill();
                DeathSpawnTimer.Remove(player);
            }

            if (IsEmpty())
            {
                if (LobbyEntity.IsTemporary)
                    Remove();
            }

            SendAllPlayerEvent(DToClientEvent.LeaveSameLobby, null, player.Player.Handle.Value, player.Player.Name);
            if (LobbyEntity.Type != ELobbyType.MainMenu)
                RestLogsManager.Log(ELogType.Lobby_Leave, player.Player, false, LobbyEntity.IsOfficial);

            CustomEventManager.SetPlayerLeftLobby(player, this);
        }

        private async Task AddPlayerLobbyStats(TDSPlayer player)
        {
            if (player.Entity is null)
                return;

            await player.ExecuteForDBAsync(async (dbContext) =>
            {
                PlayerLobbyStats? stats = await dbContext.PlayerLobbyStats.FindAsync(player.Entity.Id, LobbyEntity.Id);
                if (stats is null)
                {
                    stats = new PlayerLobbyStats { LobbyId = LobbyEntity.Id };
                    player.Entity.PlayerLobbyStats.Add(stats);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
                player.CurrentLobbyStats = stats;
            }).ConfigureAwait(false);     
        }

        public virtual void SetPlayerTeam(TDSPlayer player, Team team)
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

        public bool IsPlayerLobbyOwner(TDSPlayer character)
        {
            if (character.Entity is null)
                return false;
            return character.CurrentLobby == this && LobbyEntity.OwnerId == character.Entity.Id;
        }

        public TDSPlayer? GetOwner()
        {
            return GetPlayerById(LobbyEntity.OwnerId);
        }

        public TDSPlayer? GetPlayerById(int id)
        {
            return Players.FirstOrDefault(p => p.Entity!.Id == id);
        }
    }
}
