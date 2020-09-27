using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Database.Entity.Player;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.TeamHandlers;

namespace TDS_Server.LobbySystem.Players
{
    public class FightLobbyPlayers : BaseLobbyPlayers
    {
        public FightLobbyPlayers(IFightLobby lobby, IBaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams, BaseLobbyBansHandler bans)
            : base(lobby, events, teams, bans)
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
            var worked = await base.RemovePlayer(player);
            if (!worked)
                return false;

            player.Team?.SpectateablePlayers?.Remove(player);
            player.LastKillAt = null;
            player.KillingSpree = 0;
            return true;
        }

        private async Task AddPlayerLobbyStats(ITDSPlayer player)
        {
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

        public void Kill(ITDSPlayer player, string reason)
        {
            NAPI.Task.Run(() =>
            {
                player.Kill();
                player.SendChatMessage(reason);
            });
        }
    }
}
