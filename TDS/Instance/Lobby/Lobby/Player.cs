using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TDS.Default;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        public virtual async Task<bool> AddPlayer(Character character, uint teamindex)
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

            character.Player.Dimension = dimension;
            character.Player.Position = spawnPoint.Around(LobbyEntity.AroundSpawnPoint);
            character.Player.Freeze(true);

            SetPlayerTeam(character, teams[teamindex]);

            SendAllPlayerEvent(DCustomEvent.ClientPlayerJoinSameLobby, null, character.Player);
            NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvent.ClientPlayerJoinLobby, Id);
            NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvent.SyncPlayersSameLobby, JsonConvert.SerializeObject(players));
            Manager.Logs.Rest.Log(ELogType.Lobby_Join, character.Player, false, LobbyEntity.IsOfficial);
            return true;
        }

        public virtual async void RemovePlayer(Character character)
        {
            await SavePlayerLobbyStats(character);

            players.Remove(character);
            teamPlayers[character.Team.Index].Remove(character);

            character.CurrentLobby = null;
            character.CurrentLobbyStats = null;
            character.Lifes = 0;
            if (character.Player.Exists)
            {
                character.Player.Freeze(true);
                character.Player.StopSpectating();
                character.Player.Transparency = 255;
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

            SendAllPlayerEvent(DCustomEvent.ClientPlayerLeaveSameLobby, null, character.Player);
            Manager.Logs.Rest.Log(ELogType.Lobby_Leave, character.Player, false, LobbyEntity.IsOfficial);
        }

        private async Task SavePlayerLobbyStats(Character character)
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

        private async void AddPlayerLobbyStats(Character character, TDSNewContext dbcontext)
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
        private void SendTeamOrder(Character character, string ordershort)
        {
            Teams team = character.Team;
            SendAllPlayerLangMessage(lang =>
            {
                return $"[TEAM] {{{team.ColorR}|{team.ColorG}|{team.ColorB}}} {character.Player.Name}{{150|0|0}}: {ordershort}";
            }, character.Team.Index);

            //string teamfontcolor = character.Lobby.TeamColorStrings[character.Team] ?? "w";
            //string beforemessage = "[TEAM] #" + teamfontcolor + "#" + character.Player.SocialClubName + "#r#: ";
            //SendAllPlayerLangMessage(ordershort, character.Team, beforemessage);
        }

        public bool IsPlayerLobbyOwner(Character character)
        {
            return character.CurrentLobby == this && LobbyEntity.Owner == character.Entity.Id;
        }
    }
}
