using AltV.Net.Async;
using AltV.Net.Data;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
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
            await AltAsync.Do(() => player.SetInvincible(false));

            return true;
        }

        public void DamagedPlayer(ITDSPlayer target, ITDSPlayer source, WeaponHash weapon, BodyPart bodyPart)
        {
            DmgSys.DamagePlayer(target, weapon, bodyPart, source);
        }

        public override async Task RemovePlayer(ITDSPlayer player)
        {
            await base.RemovePlayer(player);

            await AltAsync.Do(() =>
            {
                player.Team?.SpectateablePlayers?.Remove(player);
                player.LastKillAt = null;
                player.KillingSpree = 0;
            });
        }

        #endregion Public Methods
    }
}
