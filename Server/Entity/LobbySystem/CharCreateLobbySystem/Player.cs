using AltV.Net.Async;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Shared.Default;

namespace TDS_Server.Entity.LobbySystem.CharCreateLobbySystem
{
    partial class CharCreateLobby
    {
        #region Public Methods

        public override async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (player.Entity is null || player.Entity.CharDatas is null)
                return false;
            if (!await base.AddPlayer(player, 0))
                return false;

            var json = Serializer.ToClient(player.Entity.CharDatas);

            await AltAsync.Do(() =>
            {
                player.SetInvincible(true);
                player.Freeze(true);

                player.SendEvent(ToClientEvent.StartCharCreator, json, Dimension);
            });

            return true;
        }

        public override async Task RemovePlayer(ITDSPlayer player)
        {
            await base.RemovePlayer(player);
        }

        #endregion Public Methods
    }
}
