using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class FightLobby
    {
        public override async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, teamindex))
                return false;
            player.ModPlayer?.SetInvincible(false);

            return true;
        }

        public override void RemovePlayer(ITDSPlayer player)
        {
            base.RemovePlayer(player);

            player.Team?.SpectateablePlayers?.Remove(player);
            player.LastKillAt = null;
            player.KillingSpree = 0;
        }

        public static void KillPlayer(ITDSPlayer player, string reason)
        {
            player.ModPlayer?.Kill();
            player.SendMessage(reason);
        }

        public void DamagedPlayer(ITDSPlayer target, ITDSPlayer source, WeaponHash weapon, ulong bone)
        {
            DmgSys.DamagePlayer(target, weapon, bone, source);
        }
    }
}
