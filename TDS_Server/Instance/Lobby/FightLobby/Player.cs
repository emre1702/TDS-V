using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Instance.Player;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby
    {
        public override void RemovePlayer(TDSPlayer character)
        {
            base.RemovePlayer(character);

            if (character.Team.Index != 0)
                SpectateablePlayers[character.Team.Index-1].Remove(character);
        }

        public void KillPlayer(Client player, string reason)
        {
            player.Kill();
            player.SendChatMessage(reason);
        }

        public void DamagedPlayer(TDSPlayer target, TDSPlayer source, WeaponHash weapon, bool headshot, int clientHasSentThisDamage)
        {
            DmgSys.DamagePlayer(target, weapon, headshot, source, clientHasSentThisDamage);
        }
    }
}
