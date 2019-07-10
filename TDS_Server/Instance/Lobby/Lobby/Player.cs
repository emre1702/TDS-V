using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Default;
using TDS_Server.Dto;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;
using TDS_Server_DB.Entity.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public virtual async Task<bool> AddPlayer(TDSPlayer character, uint? teamindex)
        {
            if (LobbyEntity.Type != ELobbyType.MainMenu)
            {
                if (await IsPlayerBaned(character))
                    return false;
            }

            #region Remove from old lobby

            Lobby? oldlobby = character.CurrentLobby;
            oldlobby?.RemovePlayer(character);

            #endregion Remove from old lobby

            if (LobbyEntity.Type != ELobbyType.MainMenu)
            {
                await AddPlayerLobbyStats(character);
            }

            character.CurrentLobby = this;
            Players.Add(character);

            if (LobbyEntity.Type == ELobbyType.MainMenu)
                Workaround.SetPlayerInvincible(character.Client, true);

            character.Client.Dimension = Dimension;
            character.Client.Position = SpawnPoint.Around(LobbyEntity.AroundSpawnPoint);
            Workaround.FreezePlayer(character.Client, true);

            if (teamindex != null)
                character.Team = Teams[teamindex.Value];

            SendAllPlayerEvent(DToClientEvent.JoinSameLobby, null, character.Client);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.JoinLobby, _syncedLobbySettings.Json,
                                                                                            JsonConvert.SerializeObject(Players.Select(p => p.Client.Handle.Value).ToList()),
                                                                                            JsonConvert.SerializeObject(Teams.Select(t => t.SyncedTeamData)));

            if (LobbyEntity.Type != ELobbyType.MainMenu)
            {
                RestLogsManager.Log(ELogType.Lobby_Join, character.Client, false, LobbyEntity.IsOfficial);
                NAPI.Notification.SendNotificationToPlayer(character.Client, string.Format(character.Language.JOINED_LOBBY_MESSAGE, LobbyEntity.Name, DPlayerCommand.LobbyLeave));
            }

            PlayerJoinedLobby?.Invoke(this, character);
            return true;
        }

        public virtual void RemovePlayer(TDSPlayer player)
        {
            Players.Remove(player);

            player.CurrentLobby = null;
            player.PreviousLobby = this;
            player.CurrentLobbyStats = null;
            player.Lifes = 0;
            player.Team?.SyncRemovedPlayer(player);
            player.Team = null;
            if (player.Client.Exists)
            {
                Workaround.FreezePlayer(player.Client, true);
                Workaround.UnspectatePlayer(player.Client);
                player.Client.Transparency = 255;
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

            SendAllPlayerEvent(DToClientEvent.LeaveSameLobby, null, player.Client);
            if (LobbyEntity.Type != ELobbyType.MainMenu)
                RestLogsManager.Log(ELogType.Lobby_Leave, player.Client, false, LobbyEntity.IsOfficial);

            PlayerLeftLobby?.Invoke(this, player);
        }

        private async Task AddPlayerLobbyStats(TDSPlayer character)
        {
            if (character.Entity == null)
                return;

            PlayerLobbyStats? stats = await character.DbContext.PlayerLobbyStats.FindAsync(character.Entity.Id, LobbyEntity.Id);
            if (stats == null)
            {
                stats = new PlayerLobbyStats { PlayerId = character.Entity.Id, LobbyId = LobbyEntity.Id };
                character.Entity.PlayerLobbyStats.Add(stats);
                await character.DbContext.PlayerLobbyStats.AddAsync(stats);
            }
            character.CurrentLobbyStats = stats;
        }

        public bool IsPlayerLobbyOwner(TDSPlayer character)
        {
            if (character.Entity == null)
                return false;
            return character.CurrentLobby == this && LobbyEntity.OwnerId == character.Entity.Id;
        }
    }
}