using System;
using System.Threading.Tasks;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Action;
using TDS.Server.Data.Interfaces.GangActionAreaSystem.Areas;
using TDS.Server.Data.Interfaces.GangsSystem;
using TDS.Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS.Server.Data.RoundEndReasons;

namespace TDS.Server.GangActionAreaSystem.Action
{
    internal class BaseAreaAction : IBaseGangActionAreaAction
    {
#nullable disable
        private IBaseGangActionArea _area;
#nullable restore

        public void Init(IBaseGangActionArea area)
        {
            _area = area;

            _area.Events.AddedToLobby += OnAddedToLobby;
        }

        public async ValueTask Attack(ITDSPlayer attackerPlayer)
        {
            var attacker = attackerPlayer.Gang;
            _area.GangsHandler.SetAttacker(attacker);
            if (_area.DatabaseHandler.Entity is { })
                _area.DatabaseHandler.Entity.LastAttacked = DateTime.UtcNow;

            _area.GangsHandler.Attacker!.Action.SetInAction(true);
            _area.GangsHandler.Owner?.Action.SetInAction(false);

            if (_area.GangsHandler.Owner is null)
                await AttackWithoutOwner();
            else
                await AttackWithOwner(attackerPlayer);
        }

        private async Task AttackWithoutOwner()
        {
            await SetConquered(false);
        }

        private async Task AttackWithOwner(ITDSPlayer attacker)
        {
            await _area.LobbyHandler.SetInGangActionLobby(attacker);
        }

        private async Task SetConquered(bool withAttack = true)
        {
            if (_area.DatabaseHandler.Entity is not { } entity)
                return;

            _area.GangsHandler.SetOwner(_area.GangsHandler.Attacker);

            entity.OwnerGangId = _area.GangsHandler.Attacker!.Entity.Id;
            entity.DefendCountSinceLastCapture = 0;
            if (withAttack)
                ++entity.AttackCount;

            await _area.DatabaseHandler.Database.Save();

            _area.Events.TriggerConquered(_area.GangsHandler.Attacker, _area.GangsHandler.Owner);

            Clear(_area.GangsHandler.Owner);
        }

        private async Task SetDefended()
        {
            if (_area.DatabaseHandler.Entity is not { } entity)
                return;

            ++entity.DefendCountSinceLastCapture;
            await _area.DatabaseHandler.Database.Save();

            Clear(_area.GangsHandler.Owner);
        }

        private void Clear(IGang? ownerBeforeAttack)
        {
            _area.GangsHandler.Attacker?.Action.SetActionEnded();
            ownerBeforeAttack?.Action.SetActionEnded();

            _area.GangsHandler.SetAttacker(null);
            _area.StartRequirements.HasCooldown = true;
        }

        private void OnAddedToLobby(IGangActionLobby lobby)
        {
            lobby.Events.RoundEnd += OnLobbyRoundEnd;
        }

        private async ValueTask OnLobbyRoundEnd()
        {
            switch (_area.LobbyHandler.InLobby!.CurrentRoundEndReason)
            {
                case DeathRoundEndReason deathRoundEndReason:
                    if (deathRoundEndReason.WinnerTeam == _area.LobbyHandler.InLobby.Teams.Attacker)
                        await SetConquered();
                    else
                        await SetDefended();
                    break;

                case TimeRoundEndReason:
                    await SetConquered();
                    break;

                default:
                    await SetDefended();
                    break;
            }

        }
    }
}
