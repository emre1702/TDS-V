using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;

namespace TDS_Server.Entity.LobbySystem.FightLobbySystem
{
    partial class FightLobby
    {
        #region Public Properties

        public short AmountLifes { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, teamindex))
                return false;
            ModAPI.Thread.QueueIntoMainThread(() => player.ModPlayer?.SetInvincible(false));

            return true;
        }

        public void DamagedPlayer(ITDSPlayer target, ITDSPlayer source, WeaponHash weapon, PedBodyPart bodyPart)
        {
            DmgSys.DamagePlayer(target, weapon, bodyPart, source);
        }

        public override async Task RemovePlayer(ITDSPlayer player)
        {
            await base.RemovePlayer(player);

            ModAPI.Thread.QueueIntoMainThread(() =>
            {
                player.Team?.SpectateablePlayers?.Remove(player);
                player.LastKillAt = null;
                player.KillingSpree = 0;
            });
        }

        #endregion Public Methods
    }
}
