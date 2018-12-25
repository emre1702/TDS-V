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

namespace TDS_Server.Instance.Lobby
{
    partial class Lobby
    {
        public virtual async Task<bool> AddPlayer(TDSPlayer character, uint teamindex)
        {
            using (var dbcontext = new TDSNewContext())
            {
                if (LobbyEntity.Id != 0) {
                    if (await IsPlayerBaned(character, dbcontext))
                        return false;

                    AddPlayerLobbyStats(character, dbcontext);
                }
            }

            #region Remove from old lobby
            Lobby oldlobby = character.CurrentLobby;
            if (oldlobby != null)
                oldlobby.RemovePlayer(character);
            #endregion
            character.CurrentLobby = this;
            players.Add(character);

            character.Client.Dimension = Dimension;
            character.Client.Position = SpawnPoint.Around(LobbyEntity.AroundSpawnPoint);
            character.Client.Freeze(true);

            SetPlayerTeam(character, Teams[teamindex]);

            SendAllPlayerEvent(DToClientEvent.JoinSameLobby, null, character.Client);
            NAPI.ClientEvent.TriggerClientEvent(character.Client, DToClientEvent.JoinLobby, syncedLobbySettings, players.Select(p => p.Client).ToList(), SyncedTeamDatas);

            Manager.Logs.Rest.Log(ELogType.Lobby_Join, character.Client, false, LobbyEntity.IsOfficial);
            return true;
        }

        public virtual async void RemovePlayer(TDSPlayer character)
        {
            await SavePlayerLobbyStats(character);

            players.Remove(character);
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
            Manager.Logs.Rest.Log(ELogType.Lobby_Leave, character.Client, false, LobbyEntity.IsOfficial);
        }

        private async Task SavePlayerLobbyStats(TDSPlayer character)
        {
            if (character.CurrentLobbyStats == null)
                return;
            using (var dbcontext = new TDSNewContext())
            {
                Playerlobbystats stats = character.CurrentLobbyStats;
                dbcontext.Playerlobbystats.Attach(stats);
                dbcontext.Entry(stats).State = EntityState.Modified;
                await dbcontext.SaveChangesAsync();
                dbcontext.Entry(stats).State = EntityState.Detached;
            }
        }

        private async void AddPlayerLobbyStats(TDSPlayer character, TDSNewContext dbcontext)
        {
            Playerlobbystats stats = await dbcontext.Playerlobbystats.FindAsync(character.Entity.Id, LobbyEntity.Id);
            if (stats == null)
            {
                stats = new Playerlobbystats { Id = character.Entity.Id, Lobby = LobbyEntity.Id };
                character.Entity.Playerlobbystats.Add(stats);
                await dbcontext.SaveChangesAsync();
            }
            dbcontext.Entry(stats).State = EntityState.Detached;
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
