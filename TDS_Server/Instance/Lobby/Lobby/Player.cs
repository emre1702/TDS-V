using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Server.Dto;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Logs;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public virtual async Task<bool> AddPlayer(TDSPlayer character, uint teamindex)
        {
            using (var dbcontext = new TDSNewContext())
            {
                if (LobbyEntity.Id != 0)
                {
                    if (await IsPlayerBaned(character, dbcontext))
                        return false;
                }

                #region Remove from old lobby

                Lobby? oldlobby = character.CurrentLobby;
                oldlobby?.RemovePlayer(character);

                #endregion Remove from old lobby

                if (LobbyEntity.Id != 0)
                {
                    await AddPlayerLobbyStats(character);
                }
            }

            character.CurrentLobby = this;
            Players.Add(character);

            character.Client.Dimension = Dimension;
            character.Client.Position = SpawnPoint.Around(LobbyEntity.AroundSpawnPoint);
            Workaround.FreezePlayer(character.Client, true);

            SetPlayerTeam(character, Teams[teamindex]);

            SendAllPlayerEvent(DToClientEvent.JoinSameLobby, null, character.Client);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.JoinLobby, _syncedLobbySettings.Json, Players.Select(p => p.Client.Handle.Value).ToList(), JsonConvert.SerializeObject(Teams.Select(t => t.SyncedTeamData)));

            RestLogsManager.Log(ELogType.Lobby_Join, character.Client, false, LobbyEntity.IsOfficial);

            PlayerJoinedLobby?.Invoke(this, character);
            return true;
        }

        public virtual void RemovePlayer(TDSPlayer player)
        {
            if (player.Lifes == 0)
            {
                SavePlayerLobbyStats(player);
            }

            Players.Remove(player);

            player.CurrentLobby = null;
            player.PreviousLobby = this;
            player.CurrentLobbyStats = null;
            player.Lifes = 0;
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
            if (Id != 0)
                RestLogsManager.Log(ELogType.Lobby_Leave, player.Client, false, LobbyEntity.IsOfficial);

            PlayerLeftLobby?.Invoke(this, player);
        }

        private static async void SavePlayerLobbyStats(TDSPlayer character)
        {
            using var dbContext = new TDSNewContext();
            SavePlayerLobbyStats(character, dbContext);
            await dbContext.SaveChangesAsync();
        }

        private static void SavePlayerLobbyStats(TDSPlayer character, TDSNewContext dbContext)
        {
            if (character.CurrentLobbyStats == null)
                return;

            PlayerLobbyStats? to = character.CurrentLobbyStats;
            RoundStatsDto? from = character.CurrentRoundStats;
            if (to == null || from == null)
                return;
            to.Kills += from.Kills;
            to.Assists += from.Assists;
            to.Damage += from.Damage;
            to.TotalKills += from.Kills;
            to.TotalAssists += from.Assists;
            to.TotalDamage += from.Damage;
            from.Clear();

            dbContext.Entry(to).State = EntityState.Modified;
        }

        protected async void SaveAllPlayerLobbyStats()
        {
            using var dbContext = new TDSNewContext();
            FuncIterateAllPlayers((player, team) =>
            {
                if (team == null || team.Entity.Index == 0)
                    return;
                SavePlayerLobbyStats(player, dbContext);
            });
            await dbContext.SaveChangesAsync();
        }

        private async Task AddPlayerLobbyStats(TDSPlayer character)
        {
            if (character.Entity == null)
                return;

            using TDSNewContext dbContext = new TDSNewContext();
            PlayerLobbyStats? stats = await dbContext.PlayerLobbyStats.FindAsync(character.Entity.Id, LobbyEntity.Id);
            if (stats == null)
            {
                stats = new PlayerLobbyStats { PlayerId = character.Entity.Id, LobbyId = LobbyEntity.Id };
                character.Entity.PlayerLobbyStats.Add(stats);
                await dbContext.PlayerLobbyStats.AddAsync(stats);
                await dbContext.SaveChangesAsync();
            }
            character.CurrentLobbyStats = stats;
        }

        public bool IsPlayerLobbyOwner(TDSPlayer character)
        {
            if (character.Entity == null)
                return false;
            return character.CurrentLobby == this && LobbyEntity.Owner == character.Entity.Id;
        }
    }
}