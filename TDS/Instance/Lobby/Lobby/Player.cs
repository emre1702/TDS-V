using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Default;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Player;
using TDS.Instance.Utility;
using TDS.Manager.Player;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        private Dictionary<Character, Timer> deathSpawnTimer = new Dictionary<Character, Timer>(); 

        public async Task<bool> AddPlayer(Client player, uint teamid)
        {
            Character character = player.GetChar();
            using (var dbcontext = new TDSNewContext())
            {
                if (this.entity.Id != 0) {
                    #region Check ban
                    Playerbans ban = await dbcontext.Playerbans.FindAsync(character.Entity.Id, this.entity.Id);
                    if (ban != null)
                    {
                        if (ban.EndTimestamp.HasValue && ban.EndTimestamp.Value > DateTime.Now)
                        {
                            //Todo: Add output
                            return false;
                        }
                        else if (ban.EndTimestamp.HasValue)
                        {
                            dbcontext.Remove(ban);
                            await dbcontext.SaveChangesAsync();
                            
                        }
                    }
                    #endregion

                    this.AddPlayerLobbyStats(character, dbcontext);
                }
            }

            #region Remove from old lobby
            Lobby oldlobby = character.CurrentLobby;
            if (oldlobby != null)
            {
                oldlobby.RemovePlayer(character);
                
            }
            #endregion
            character.CurrentLobby = this;
            this.players.Add(character);

            character.Player.Dimension = this.dimension;
            character.Player.Position = this.spawnPoint.Around(this.entity.AroundSpawnPoint);

            this.SetPlayerTeam(character, this.teamsByID[teamid]);

            this.SendAllPlayerEvent(DCustomEvents.ClientPlayerJoinSameLobby, null, character.Player);
            NAPI.ClientEvent.TriggerClientEvent(character.Player, DCustomEvents.SyncPlayersSameLobby, JsonConvert.SerializeObject(this.players));
            Manager.Logs.Rest.Log(ELogType.Lobby_Join, character.Player, false, this.entity.IsOfficial);

            return true;
        }

        public async void RemovePlayer(Character character)
        {
            await this.SavePlayerLobbyStats(character);

            this.players.Remove(character);
            this.TeamPlayers[character.Team].Remove(character);

            character.CurrentLobby = null;
            character.CurrentLobbyStats = null;
            character.Lifes = 0;
            if (character.Player.Exists)
            {
                character.Player.Freeze(true);
                character.Player.StopSpectating();
                character.Player.Transparency = 255;
            }

            if (this.IsEmpty())
            {
                if (this.entity.IsTemporary)
                    this.Remove();
            }

            this.SendAllPlayerEvent(DCustomEvents.ClientPlayerLeaveSameLobby, null, character.Player);
            Manager.Logs.Rest.Log(ELogType.Lobby_Leave, character.Player, false, this.entity.IsOfficial);
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
            Playerlobbystats stats = await dbcontext.Playerlobbystats.FindAsync(character.Entity.Id, this.entity.Id);
            if (stats == null)
            {
                stats = new Playerlobbystats { Id = character.Entity.Id, Lobby = this.entity.Id };
                character.Entity.Playerlobbystats.Add(stats);
                await dbcontext.SaveChangesAsync();
            }
            dbcontext.Entry(stats).State = EntityState.Detached;
            character.CurrentLobbyStats = stats;
        }

        // Todo: improve that for enum! ordershort as string won't work!
        private void SendTeamOrder(Character character, string ordershort)
        {
            Teams team = character.Team;
            this.SendAllPlayerLangMessage(lang =>
            {
                return $"[TEAM] {{{team.ColorR}|{team.ColorG}|{team.ColorB}}} {character.Player.Name}{{150|0|0}}: {ordershort}";
            }, character.Team.Id);

            //string teamfontcolor = character.Lobby.TeamColorStrings[character.Team] ?? "w";
            //string beforemessage = "[TEAM] #" + teamfontcolor + "#" + character.Player.SocialClubName + "#r#: ";
            //SendAllPlayerLangMessage(ordershort, character.Team, beforemessage);
        }
    }
}
