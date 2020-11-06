﻿using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.DamageSystem;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Helper;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class RoundFightLobbyDeathmatch : FightLobbyDeathmatch, IRoundFightLobbyDeathmatch
    {
        protected new IRoundFightLobby Lobby => (IRoundFightLobby)base.Lobby;
        protected new IRoundFightLobbyEventsHandler Events => (IRoundFightLobbyEventsHandler)base.Events;

        public RoundFightLobbyDeathmatch(IRoundFightLobby lobby, IRoundFightLobbyEventsHandler events, IDamageHandler damageHandler, LangHelper langHelper)
            : base(lobby, events, damageHandler, langHelper)
        {
        }

        public async Task RemovePlayerFromAlive(ITDSPlayer player)
        {
            player.Team?.Players.RemoveAlive(player);
            await Lobby.Spectator.SetPlayerCantBeSpectatedAnymore(player).ConfigureAwait(false);
        }

        public override async Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            var lifes = player.Lifes;
            await base.OnPlayerDeath(player, killer, weapon).ConfigureAwait(false);

            if (lifes == 1 && player.Lifes == 0)
            {
                await RemovePlayerFromAlive(player).ConfigureAwait(false);
                await Lobby.Rounds.CheckForEnoughAlive().ConfigureAwait(false);
            }
            else if (lifes > 0)
                Lobby.Players.RespawnPlayer(player);
        }
    }
}