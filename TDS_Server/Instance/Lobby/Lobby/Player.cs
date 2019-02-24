using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TDS_Server.Default;
using TDS_Server.Entity;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;
using TDS_Common.Default;
using System.Linq;
using TDS_Server.Manager.Logs;
using TDS_Server.Dto;

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        private static readonly TDSNewContext playerLobbyStatsContext = new TDSNewContext();

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
                Lobby oldlobby = character.CurrentLobby;
                oldlobby?.RemovePlayer(character);
                #endregion

                if (LobbyEntity.Id != 0)
                {
                    await AddPlayerLobbyStats(character);
                }
            }

            character.CurrentLobby = this;
            Players.Add(character);

            character.Client.Dimension = Dimension;
            character.Client.Position = SpawnPoint.Around(LobbyEntity.AroundSpawnPoint);
            character.Client.Freeze(true);

            SetPlayerTeam(character, Teams[teamindex]);

            SendAllPlayerEvent(DToClientEvent.JoinSameLobby, null, character.Client);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.JoinLobby, JsonConvert.SerializeObject(syncedLobbySettings), Players.Select(p => p.Client.Handle.Value).ToList(), JsonConvert.SerializeObject(Teams.Select(t => t.SyncedTeamData)));

            RestLogsManager.Log(ELogType.Lobby_Join, character.Client, false, LobbyEntity.IsOfficial);
            return true;
        }

        public virtual async void RemovePlayer(TDSPlayer character)
        {
            if (character.Lifes == 0)
            {
                SavePlayerLobbyStats(character);
                await playerLobbyStatsContext.SaveChangesAsync();
            }
            
            Players.Remove(character);
            if (character.Team != null)
                character.Team.Players.Remove(character);

            character.CurrentLobby = null;
            character.PreviousLobby = this;
            character.CurrentLobbyStats = null;
            character.Lifes = 0;
            if (character.Client.Exists)
            {
                character.Client.Freeze(true);
                character.Client.StopSpectating();
                character.Client.Transparency = 255;
            }
            if (DeathSpawnTimer.ContainsKey(character))
            {
                DeathSpawnTimer[character].Kill();
                DeathSpawnTimer.Remove(character);
            }

            if (IsEmpty())
            {
                if (LobbyEntity.IsTemporary)
                    Remove();
            }

            SendAllPlayerEvent(DToClientEvent.LeaveSameLobby, null, character.Client);
            if (Id != 0)
                RestLogsManager.Log(ELogType.Lobby_Leave, character.Client, false, LobbyEntity.IsOfficial);
        }

        private static void SavePlayerLobbyStats(TDSPlayer character)
        {
            if (character.CurrentLobbyStats == null)
                return;

            PlayerLobbyStats to = character.CurrentLobbyStats;
            RoundStatsDto from = character.CurrentRoundStats;
            to.Kills += from.Kills;
            to.Assists += from.Assists;
            to.Damage += from.Damage;
            to.TotalKills += from.Kills;
            to.TotalAssists += from.Assists;
            to.TotalDamage += from.Damage;
            from.Clear();
        }

        protected async void SaveAllPlayerLobbyStats()
        {
            FuncIterateAllPlayers((player, team) =>
            {
                if (team.Entity.Index == 0)
                    return;
                SavePlayerLobbyStats(player);
            });
            await playerLobbyStatsContext.SaveChangesAsync();
        }

        private async Task AddPlayerLobbyStats(TDSPlayer character)
        {
            PlayerLobbyStats stats = await playerLobbyStatsContext.PlayerLobbyStats.FindAsync(character.Entity.Id, LobbyEntity.Id);
            if (stats == null)
            {
                stats = new PlayerLobbyStats { Id = character.Entity.Id, Lobby = LobbyEntity.Id };
                character.Entity.PlayerLobbyStats.Add(stats);
                await playerLobbyStatsContext.PlayerLobbyStats.AddAsync(stats);
                await playerLobbyStatsContext.SaveChangesAsync();
            }
            character.CurrentLobbyStats = stats;
        }

        public bool IsPlayerLobbyOwner(TDSPlayer character)
        {
            return character.CurrentLobby == this && LobbyEntity.Owner == character.Entity.Id;
        }
    }
}
