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
                Lobby oldlobby = character.CurrentLobby;
                oldlobby?.RemovePlayer(character);
                #endregion

                if (LobbyEntity.Id != 0)
                {
                    await AddPlayerLobbyStats(character, dbcontext);
                }
            }

            character.CurrentLobby = this;
            Players.Add(character);

            character.Client.Dimension = Dimension;
            character.Client.Position = SpawnPoint.Around(LobbyEntity.AroundSpawnPoint);
            character.Client.Freeze(true);

            SetPlayerTeam(character, Teams[teamindex]);

            SendAllPlayerEvent(DToClientEvent.JoinSameLobby, null, character.Client);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.JoinLobby, JsonConvert.SerializeObject(syncedLobbySettings), Players.Select(p => p.Client.Handle.Value).ToList(), JsonConvert.SerializeObject(SyncedTeamDatas));

            RestLogsManager.Log(ELogType.Lobby_Join, character.Client, false, LobbyEntity.IsOfficial);
            return true;
        }

        public virtual async void RemovePlayer(TDSPlayer character)
        {
            await SavePlayerLobbyStats(character);

            Players.Remove(character);
            if (character.Team != null)
                TeamPlayers[character.Team.Index].Remove(character);

            character.CurrentLobby = null;
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

        private async Task SavePlayerLobbyStats(TDSPlayer character)
        {
            if (character.CurrentLobbyStats == null)
                return;
            using (var dbcontext = new TDSNewContext())
            {
                Playerlobbystats stats = character.CurrentLobbyStats;
                dbcontext.Playerlobbystats.Attach(stats);
                await dbcontext.SaveChangesAsync();
            }
        }

        private async Task AddPlayerLobbyStats(TDSPlayer character, TDSNewContext dbcontext)
        {
            Playerlobbystats stats = await dbcontext.Playerlobbystats.FindAsync(character.Entity.Id, LobbyEntity.Id);
            if (stats == null)
            {
                stats = new Playerlobbystats { Id = character.Entity.Id, Lobby = LobbyEntity.Id };
                character.Entity.Playerlobbystats.Add(stats);
                await dbcontext.SaveChangesAsync();
            }
            character.CurrentLobbyStats = stats;
        }

#warning Todo: improve that for enum! ordershort as string won't work!
        private void SendTeamOrder(TDSPlayer character, string ordershort)
        {
            Teams team = character.Team;
            SendAllPlayerLangMessage(lang =>
            {
                return $"[TEAM] {{{team.ColorR}|{team.ColorG}|{team.ColorB}}} {character.Client.Name}{{150|0|0}}: {ordershort}";
            }, character.Team.Index);

            //string teamfontcolor = character.Lobby.TeamColorStrings[character.Team] ?? "w";
            //string beforemessage = "[TEAM] #" + teamfontcolor + "#" + character.Player.SocialClubName + "#r#: ";
            //SendAllPlayerLangMessage(ordershort, character.Team, beforemessage);
        }

        public bool IsPlayerLobbyOwner(TDSPlayer character)
        {
            return character.CurrentLobby == this && LobbyEntity.Owner == character.Entity.Id;
        }
    }
}
