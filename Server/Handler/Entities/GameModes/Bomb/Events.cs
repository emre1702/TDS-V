using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.GameModes.Bomb
{
    partial class Bomb
    {
        public override void OnPlayerEnterColShape(IColShape shape, ITDSPlayer character)
        {
            base.OnPlayerEnterColShape(shape, character);
            if (_lobbyBombTakeCol.ContainsKey(Lobby))
            {
                if (character.Lifes > 0 && character.Team == _terroristTeam)
                {
                    TakeBomb(character);
                }
            }
        }

        public override void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer)
        {
            base.OnPlayerDeath(player, killer);
            if (_bombAtPlayer == player)
                DropBomb();
        }

        public override void SendPlayerRoundInfoOnJoin(ITDSPlayer player)
        {
            if (Lobby.CurrentRoundStatus != RoundStatus.Round)
                return;

            if (_bombDetonateTimer is null || _bomb is null)
                return;

            if (_bombDetonateTimer != null && _bomb != null)
                player.SendEvent(ToClientEvent.BombPlanted,
                    Serializer.ToClient(_bomb.Position),
                    false,
                    _bombDetonateTimer.ExecuteAfterMs - _bombDetonateTimer.RemainingMsToExecute);
        }

        public override void OnPlayerWeaponSwitch(ITDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon)
        {
            base.OnPlayerWeaponSwitch(character, oldweapon, newweapon);
            if (_bombAtPlayer == character)
            {
                ToggleBombAtHand(character, oldweapon, newweapon);
            }
        }
    }
}
