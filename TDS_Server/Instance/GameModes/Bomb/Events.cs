using GTANetworkAPI;
using Newtonsoft.Json;
using TDS_Common.Default;
using TDS_Server.Enum;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.GameModes
{
    partial class Bomb
    {
        public override void OnPlayerEnterColShape(ColShape shape, TDSPlayer character)
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

        public override void OnPlayerDeath(TDSPlayer player)
        {
            base.OnPlayerDeath(player);
            if (_bombAtPlayer == player)
                DropBomb();
        }

        public override void SendPlayerRoundInfoOnJoin(TDSPlayer player)
        {
            if (Lobby.CurrentRoundStatus != ERoundStatus.Round)
                return;

            if (_bombDetonateTimer is null || _bomb is null)
                return;

            if (_bombDetonateTimer != null && _bomb != null)
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.BombPlanted,
                    JsonConvert.SerializeObject(_bomb.Position),
                    false,
                    _bombDetonateTimer.ExecuteAfterMs - _bombDetonateTimer.RemainingMsToExecute);
        }

        public override void OnPlayerWeaponSwitch(TDSPlayer character, WeaponHash oldweapon, WeaponHash newweapon)
        {
            base.OnPlayerWeaponSwitch(character, oldweapon, newweapon);
            if (_bombAtPlayer == character)
            {
                ToggleBombAtHand(character, oldweapon, newweapon);
            }
        }
    }
}
