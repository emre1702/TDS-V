using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Common.Enum;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Core.Instance.LobbyInstances.FightLobby
{
    partial class FightLobby
    {
        public override async Task<bool> AddPlayer(TDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, teamindex))
                return false;
            Workaround.SetPlayerInvincible(player.Player!, false);

            return true;
        }

        public override void RemovePlayer(TDSPlayer character)
        {
            base.RemovePlayer(character);

            character.Team?.SpectateablePlayers?.Remove(character);
            character.LastKillAt = null;
            character.KillingSpree = 0;
        }

        public static void KillPlayer(Player player, string reason)
        {
            player.Kill();
            player.SendChatMessage(reason);
        }

        public void DamagedPlayer(TDSPlayer target, TDSPlayer source, WeaponHash weapon, ulong bone)
        {
            DmgSys.DamagePlayer(target, weapon, bone, source);
        }
    }
}
