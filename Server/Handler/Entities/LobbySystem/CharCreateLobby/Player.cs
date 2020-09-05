using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class CharCreateLobby
    {
        public override async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (player.Entity is null || player.Entity.CharDatas is null)
                return false;
            if (!await base.AddPlayer(player, 0))
                return false;

            var json = Serializer.ToClient(player.Entity.CharDatas);

            NAPI.Task.Run(() =>
            {
                player.SetInvincible(true);
                player.Freeze(true);

                player.TriggerEvent(ToClientEvent.StartCharCreator, json, Dimension);
            });

            return true;
        }

        public override async Task RemovePlayer(ITDSPlayer player)
        {
            await base.RemovePlayer(player);
        }
    }
}
