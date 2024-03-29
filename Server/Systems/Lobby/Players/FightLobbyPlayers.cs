﻿using System.Threading.Tasks;
using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Data.Interfaces.LobbySystem.Players;
using TDS.Server.Data.Models;
using TDS.Server.Database.Entity.Player;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.LobbySystem.Players
{
    public class FightLobbyPlayers : BaseLobbyPlayers, IFightLobbyPlayers
    {
        protected new IFightLobby Lobby => (IFightLobby)base.Lobby;

        public bool SavePlayerLobbyStats { get; protected set; }

        public FightLobbyPlayers(IFightLobby lobby, IFightLobbyEventsHandler events)
            : base(lobby, events)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            var worked = await base.AddPlayer(player, teamIndex).ConfigureAwait(false);
            if (!worked)
                return false;

            await AddPlayerLobbyStats(player).ConfigureAwait(false);

            NAPI.Task.RunSafe(() =>
            {
                player.SetInvincible(false);
                player.SetInvisible(false);
            });

            return true;
        }

        public override async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            var lifes = player.Lifes;
            var worked = await base.RemovePlayer(player).ConfigureAwait(false);
            if (!worked)
                return false;

            player.Deathmatch.LastKillAt = null;
            player.Deathmatch.KillingSpree = 0;
            player.CurrentRoundStats = null;

            if (lifes > 0)
            {
                Lobby.Deathmatch.Damage.DeathHandler.RewardLastHitter(player, out var killer);
                Lobby.Deathmatch.Damage.DeathHandler.PlayerDeath(player, killer ?? player, (uint)WeaponHash.Unarmed, lifes);
            }

            return true;
        }

        private async Task AddPlayerLobbyStats(ITDSPlayer player)
        {
            player.CurrentRoundStats = new RoundStatsDto(player);
            PlayerLobbyStats? stats = null;
            await player.Database.ExecuteForDBAsync(async (dbContext) =>
            {
                stats = await dbContext.PlayerLobbyStats.FindAsync(player.Entity!.Id, Lobby.Entity.Id).ConfigureAwait(false);
                if (stats is null)
                {
                    stats = new PlayerLobbyStats { LobbyId = Lobby.Entity.Id };
                    player.Entity.PlayerLobbyStats.Add(stats);
                    await dbContext.SaveChangesAsync().ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
            await player.LobbyHandler.SetPlayerLobbyStats(stats).ConfigureAwait(false);
        }
    }
}
