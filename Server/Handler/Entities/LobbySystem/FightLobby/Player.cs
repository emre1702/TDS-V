using GTANetworkAPI;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class FightLobby
    {
        #region Public Properties

        public short AmountLifes { get; set; }

        #endregion Public Properties

        #region Public Methods

        public static void KillPlayer(ITDSPlayer player, string reason)
        {
            player.Kill();
            player.SendChatMessage(reason);
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, teamindex))
                return false;
            NAPI.Task.Run(() => player.SetInvincible(false));

            return true;
        }

        public void DamagedPlayer(ITDSPlayer target, ITDSPlayer source, WeaponHash weapon, PedBodyPart bodyPart)
        {
            DmgSys.DamagePlayer(target, weapon, bodyPart, source);
        }

        public override async Task RemovePlayer(ITDSPlayer player)
        {
            await base.RemovePlayer(player);

            NAPI.Task.Run(() =>
            {
                player.Team?.SpectateablePlayers?.Remove(player);
                player.LastKillAt = null;
                player.KillingSpree = 0;
            });
        }

        #endregion Public Methods
    }
}
