using GTANetworkAPI;
using System.Threading.Tasks;
using TDS.Instance.Player;

namespace TDS.Instance.Lobby
{
    partial class FightLobby
    {
        public override void RemovePlayer(TDSPlayer character)
        {
            base.RemovePlayer(character);

            SpectateablePlayers[character.Team.Index].Remove(character);
        }

        private void KillPlayer(Client player, string reason)
        {
            player.Kill();
            player.SendChatMessage(reason);
        }

        public void DamagedPlayer(TDSPlayer target, TDSPlayer source, WeaponHash weapon, bool headshot)
        {
            DmgSys.DamagePlayer(target, weapon, headshot, source);
        }
    }
}
