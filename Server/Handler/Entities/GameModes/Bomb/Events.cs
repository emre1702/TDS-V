using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.Gamemodes
{
    partial class Bomb
    {
        public override void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer)
        {
            base.OnPlayerDeath(player, killer);
            if (_bombAtPlayer == player)
                DropBomb();
        }

        public override void OnPlayerEnterColshape(ITDSColShape shape, ITDSPlayer character)
        {
            base.OnPlayerEnterColshape(shape, character);
            if (_lobbyBombTakeCol.ContainsKey(Lobby))
            {
                if (character.Lifes > 0 && character.Team == _terroristTeam)
                {
                    TakeBomb(character);
                }
            }
        }

        public override void OnPlayerWeaponSwitch(ITDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon)
        {
            base.OnPlayerWeaponSwitch(character, oldweapon, newweapon);
            if (_bombAtPlayer == character)
            {
                ToggleBombAtHand(character, oldweapon, newweapon);
            }
        }

        public override void SendPlayerRoundInfoOnJoin(ITDSPlayer player)
        {
            if (Lobby.CurrentRoundStatus != RoundStatus.Round)
                return;

            if (_bombDetonateTimer is null || _bomb is null)
                return;

            if (_bombDetonateTimer != null && _bomb != null)
                player.TriggerEvent(ToClientEvent.BombPlanted,
                    Serializer.ToClient(_bomb.Position),
                    false,
                    _bombDetonateTimer.ExecuteAfterMs - _bombDetonateTimer.RemainingMsToExecute);
        }
    }
}
