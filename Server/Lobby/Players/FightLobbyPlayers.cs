using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Data.Models;
using TDS_Server.Database.Entity.Player;

namespace TDS_Server.LobbySystem.Players
{
    public class FightLobbyPlayers : BaseLobbyPlayers, IFightLobbyPlayers
    {
        protected new IFightLobby Lobby => (IFightLobby)base.Lobby;

        public FightLobbyPlayers(IFightLobby lobby, IBaseLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            var worked = await base.AddPlayer(player, teamIndex);
            if (!worked)
                return false;

            await AddPlayerLobbyStats(player);

            NAPI.Task.Run(() =>
            {
                player.SetInvincible(false);
            });

            return true;
        }

        public override async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            var lifes = player.Lifes;
            var worked = await base.RemovePlayer(player);
            if (!worked)
                return false;

            player.Team?.SpectateablePlayers?.Remove(player);
            player.LastKillAt = null;
            player.KillingSpree = 0;
            player.CurrentRoundStats = null;

            if (lifes > 0)
            {
                Lobby.Deathmatch.Damage.RewardLastHitter(player, out var killer);
                Lobby.Deathmatch.DeathInfoSync(player, killer, (uint)WeaponHash.Unarmed);
            }

            return true;
        }

        private async Task AddPlayerLobbyStats(ITDSPlayer player)
        {
            player.CurrentRoundStats = new RoundStatsDto(player);
            PlayerLobbyStats? stats = null;
            await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                stats = await dbContext.PlayerLobbyStats.FindAsync(player.Entity!.Id, Lobby.Entity.Id);
                if (stats is null)
                {
                    stats = new PlayerLobbyStats { LobbyId = Lobby.Entity.Id };
                    player.Entity.PlayerLobbyStats.Add(stats);
                    await dbContext.SaveChangesAsync();
                }
            }).ConfigureAwait(false);
            await player.SetPlayerLobbyStats(stats);
        }
    }
}
