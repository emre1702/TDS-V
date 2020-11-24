using System;
using System.Threading.Tasks;
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

        public async ValueTask Attack(IGang attacker)
        {
            _area.GangsHandler.SetAttacker(attacker);
            if (_area.DatabaseHandler.Entity is { })
                _area.DatabaseHandler.Entity.LastAttacked = DateTime.UtcNow;
            if (_area.GangsHandler.Owner is null)
                await AttackWithoutOwner();
            else
                await AttackWithOwner();
        }

        private async Task AttackWithoutOwner()
        {
            await SetConquered(false);
        }

        private async Task AttackWithOwner()
        {
            await _area.LobbyHandler.SetInGangActionLobby();
            _area.GangsHandler.Attacker!.Action.InAction = true;
            _area.GangsHandler.Owner!.Action.InAction = true;
        }

        private async Task SetConquered(bool withAttack = true)
        {
            if (_area.DatabaseHandler.Entity is not { } entity)
                return;

            entity.OwnerGangId = _area.GangsHandler.Attacker!.Entity.Id;
            entity.DefendCountSinceLastCapture = 0;
            if (withAttack)
                ++entity.AttackCount;

            await _area.DatabaseHandler.Database.Save();

            _area.Events.TriggerConquered(_area.GangsHandler.Attacker, _area.GangsHandler.Owner);

            Clear();
        }

        private async Task SetDefended()
        {
            if (_area.DatabaseHandler.Entity is not { } entity)
                return;

            ++entity.DefendCountSinceLastCapture;
            await _area.DatabaseHandler.Database.Save();

            Clear();
        }

        private void Clear()
        {
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
