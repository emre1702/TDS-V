﻿using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Data.Interfaces.DamageSystem;
using TDS.Server.Data.Interfaces.Entities;
using TDS.Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS.Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Helper;
using TDS.Shared.Core;
using TDS.Shared.Default;

namespace TDS.Server.LobbySystem.Deathmatch
{
    public class FightLobbyDeathmatch : BaseLobbyDeathmatch, IFightLobbyDeathmatch
    {
        public int AmountLifes { get; set; }

        public IDamageHandler Damage { get; }
        protected new IFightLobby Lobby => (IFightLobby)base.Lobby;

        public FightLobbyDeathmatch(IFightLobby lobby, IFightLobbyEventsHandler events, IDamageHandler damage)
            : base(lobby, events)
        {
            Damage = damage;
            AmountLifes = lobby.Entity.FightSettings.AmountLifes;
        }

        protected override ValueTask ResetPlayer((ITDSPlayer Player, int HadLifes) data)
        {
            NAPI.Task.RunSafe(() =>
            {
                data.Player.RemoveAllWeapons();
            });
            return default;
        }

        public override async Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            await base.OnPlayerDeath(player, killer, weapon).ConfigureAwait(false);

            if (player.Team is null || player.Team.IsSpectator)
            {
                await Lobby.Spectator.SpectateOtherAllTeams(player).ConfigureAwait(false);
            }

            var hadLifes = player.Lifes;
            Damage.DeathHandler.PlayerDeath(player, killer, weapon, hadLifes);

            if (player.Lifes > 0)
                PlayerDiedInFight(player);
            else
                SetPlayerDeadCompletely(player);

            Lobby.Events.TriggerPlayerDied(player, killer, weapon, hadLifes);
        }

        private void PlayerDiedInFight(ITDSPlayer player)
        {
            if (--player.Lifes == 0)
                SetPlayerDeadCompletely(player);
        }

        protected virtual void SetPlayerDeadCompletely(ITDSPlayer player)
        {
            Lobby.Spectator.SetPlayerInSpectateMode(player);
        }

        public void Kill(ITDSPlayer player, string reason)
        {
            NAPI.Task.RunSafe(() =>
            {
                player.Kill();
                player.SendChatMessage(reason);
            });
        }

        public virtual void InitDamageHandler(IDamageHandler damageHandler)
        {
            damageHandler.Init(Lobby, Lobby.Entity.LobbyWeapons, Lobby.Entity.LobbyKillingspreeRewards);
        }
    }
}