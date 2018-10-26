using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TDS.Default;
using TDS.Entity;
using TDS.Enum;
using TDS.Instance.Player;
using TDS.Manager.Player;

namespace TDS.Instance.Lobby
{
    partial class Lobby
    {
        public async Task<bool> AddPlayer(Client player, uint team)
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
                oldlobby.RemovePlayer(player);
                
            }
            #endregion
            character.CurrentLobby = this;

            return true;
        }

        public async void RemovePlayer(Client player)
        {
            this.SendAllPlayerEvent(DCustomEvents.ClientPlayerLeaveLobby, null, player.Value);
            Character character = player.GetChar();
            await this.SavePlayerLobbyStats(character);

            this.players.Remove(player);
            this.TeamPlayers[character.TeamID].Remove(player);

            player.Freeze(true);
            player.StopSpectating();
            player.Transparency = 255;
            character.CurrentLobby = null;
            character.CurrentLobbyStats = null;

            if (this.IsEmpty())
            {
                if (this.entity.IsTemporary)
                    this.Remove();
            }
            else
            {
                FuncIterateAllPlayers((p, teamid) =>
                {
                    if (p != player)
                        NAPI.ClientEvent.TriggerClientEvent(p, DCustomEvents.PlayerLeftSameLobby, player);
                });
            }
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
            #region CurrentLobbyStats
            Playerlobbystats stats = await dbcontext.Playerlobbystats.FindAsync(character.Entity.Id, this.entity.Id);
            if (stats == null)
            {
                stats = new Playerlobbystats { Id = character.Entity.Id, Lobby = this.entity.Id };
                character.Entity.Playerlobbystats.Add(stats);
                await dbcontext.SaveChangesAsync();
            }
            dbcontext.Entry(stats).State = EntityState.Detached;
            character.CurrentLobbyStats = stats;
            #endregion
        }
    }
}
