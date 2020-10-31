using GTANetworkAPI;
using System.Collections.Generic;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.DamageSystem;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.LobbySystem.Deathmatch;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies.Abstracts;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.Helper;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class FightLobbyDeathmatch : BaseLobbyDeathmatch, IFightLobbyDeathmatch
    {
        public int AmountLifes { get; set; }

        private readonly LangHelper _langHelper;
        public IDamageHandler Damage { get; }
        protected new IFightLobby Lobby => (IFightLobby)base.Lobby;

        public FightLobbyDeathmatch(IFightLobby lobby, IFightLobbyEventsHandler events, IDamageHandler damage, LangHelper langHelper)
            : base(lobby, events)
        {
            Damage = damage;
            _langHelper = langHelper;
            InitDamagesys(damage);
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
                PlayerDiedInFight(player, killer, weapon);
            else
                SetPlayerDeadCompletely(player);

            Lobby.Events.TriggerPlayerDied(player, killer, weapon, hadLifes);
        }

        private void PlayerDiedInFight(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            DeathInfoSync(player, killer, weapon);
            if (--player.Lifes == 0)
                SetPlayerDeadCompletely(player);
        }

        protected virtual void SetPlayerDeadCompletely(ITDSPlayer player)
        {
            var deathSpawnTimer = new TDSTimer(() =>
            {
                Lobby.Spectator.SetPlayerInSpectateMode(player);
            }, (uint)Lobby.Entity.FightSettings.SpawnAgainAfterDeathMs);
        }

        public void DeathInfoSync(ITDSPlayer player, ITDSPlayer? killer, uint weapon)
        {
            Dictionary<ILanguage, string> killstr;
            if (killer is { } && player != killer)
            {
                string? weaponName = System.Enum.GetName(typeof(WeaponHash), weapon);
                killstr = _langHelper.GetLangDictionary(lang => string.Format(lang.DEATH_KILLED_INFO, killer?.DisplayName ?? "-", player.DisplayName, weaponName ?? "?"));
            }
            else
            {
                killstr = _langHelper.GetLangDictionary(lang => string.Format(lang.DEATH_DIED_INFO, player.DisplayName));
            }

            Lobby.Players.DoInMain(target =>
            {
                target.TriggerEvent(ToClientEvent.Death, player.RemoteId, player.TeamIndex, killstr[target.Language], player.Lifes > 1);
            });
        }

        public void Kill(ITDSPlayer player, string reason)
        {
            NAPI.Task.RunSafe(() =>
            {
                player.Kill();
                player.SendChatMessage(reason);
            });
        }

        protected virtual void InitDamagesys(IDamageHandler damageHandler)
        {
            damageHandler.Init(Lobby, Lobby.Entity.LobbyWeapons, Lobby.Entity.LobbyKillingspreeRewards);
        }
    }
}
