using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Instance.Lobby
{
    partial class FightLobby
    {
        public override async Task<bool> AddPlayer(TDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, teamindex))
                return false;
            Workaround.SetPlayerInvincible(player.Client, false);

            return true;
        }

        public override void RemovePlayer(TDSPlayer character)
        {
            base.RemovePlayer(character);

            character.Team?.SpectateablePlayers?.Remove(character);
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