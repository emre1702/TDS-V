using System.Collections.Generic;
using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Core.Damagesystem;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Players;
using TDS_Server.Handler.Helper;
using TDS_Server.LobbySystem.Lobbies.Abstracts;
using TDS_Server.LobbySystem.Spectator;
using TDS_Shared.Core;
using TDS_Shared.Default;
using LobbyDb = TDS_Server.Database.Entity.LobbyEntities.Lobbies;

namespace TDS_Server.LobbySystem.Deathmatch
{
    public class FightLobbyDeathmatch : BaseLobbyDeathmatch
    {
        private readonly LangHelper _langHelper;
        private readonly IBaseLobbyPlayers _players;
        private readonly FightLobbySpectator _spectator;
        private readonly LobbyDb _entity;

        internal Damagesys Damage { get; set; }

        public FightLobbyDeathmatch(IBaseLobbyEventsHandler events, FightLobby fightLobby, Damagesys damage, LangHelper langHelper, IBaseLobbyPlayers players, FightLobbySpectator spectator, LobbyDb entity)
            : base(events, fightLobby)
        {
            Damage = damage;
            _langHelper = langHelper;
            _players = players;
            _spectator = spectator;
            _entity = entity;
            damage.Init(fightLobby.Entity.LobbyWeapons, fightLobby.Entity.LobbyKillingspreeRewards);
        }

        protected override void ResetPlayer(ITDSPlayer player)
        {
            NAPI.Task.RunCustom(() =>
            {
                player.RemoveAllWeapons();
            });
        }

        public override async Task OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint weapon)
        {
            await base.OnPlayerDeath(player, killer, weapon);

            if (player.Team is null || player.Team.IsSpectator)
            {
                await _spectator.SpectateOtherAllTeams(player);
            }

            Damage.OnPlayerDeath(player, killer, weapon);

            if (player.Lifes > 0)
                PlayerDiedInFight(player, killer, weapon);
            else
                SetPlayerDeadCompletely(player);
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
                _spectator.SpectateOtherSameTeam(player);
                player.TriggerEvent(ToClientEvent.PlayerSpectateMode);
            }, (uint)_entity.FightSettings.SpawnAgainAfterDeathMs);
        }

        protected void DeathInfoSync(ITDSPlayer player, ITDSPlayer? killer, uint weapon)
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

            _players.DoInMain(target =>
            {
                target.TriggerEvent(ToClientEvent.Death, player.RemoteId, player.TeamIndex, killstr[target.Language], player.Lifes > 1);
            });
        }
    }
}
