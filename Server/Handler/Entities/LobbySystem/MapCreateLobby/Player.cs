using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class MapCreateLobby
    {
        public override async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, 0))
                return false;

            player.ModPlayer?.SetInvincible(true);
            player.ModPlayer?.Freeze(false);

            if (Players.Count > 1)
            {
                player.SendEvent(ToClientEvent.MapCreatorSyncAllObjects, Serializer.ToBrowser(_currentMap));
            }

            return true;
        }

        public override void RemovePlayer(ITDSPlayer player)
        {
            base.RemovePlayer(player);

            if (player.Entity?.Id == Entity.OwnerId && Players.Count >= 1)
            {
                var newOwner = SharedUtils.GetRandom(Players);
                Entity.OwnerId = newOwner.Entity!.Id;
                DataSyncHandler.SetData(newOwner, PlayerDataKey.IsLobbyOwner, PlayerDataSyncMode.Player, true);
            }
        }

        public void SetPosition(ITDSPlayer player, float x, float y, float z, float rot)
        {
            player.ModPlayer!.Position = new Position3D(x, y, z);
            player.ModPlayer!.Rotation = rot;
        }

        public async void GiveVehicle(ITDSPlayer player, FreeroamVehicleType vehType)
        {
            VehicleHash vehHash = await ExecuteForDBAsync(async (dbContext) =>
            {
                return await dbContext.FreeroamDefaultVehicle
                    .Where(v => v.VehicleType == vehType)
                    .Select(v => v.VehicleHash)
                    .FirstOrDefaultAsync();
            });
            if (vehHash == default)
                return;

            var pos = player.ModPlayer!.Position;

            ModAPI.Thread.RunInMainThread(() =>
            {
                if (player.FreeroamVehicle is { })
                {
                    if (player.ModPlayer.IsInVehicle)
                        player.ModPlayer.WarpOutOfVehicle();
                    player.FreeroamVehicle.Delete();
                    player.FreeroamVehicle = null;
                }

                IVehicle? vehicle = ModAPI.Vehicle.Create(vehHash, pos, player.ModPlayer.Rotation, 0, 0, player.ModPlayer.Name, dimension: Dimension);
                if (vehicle is null)
                    return;
                player.FreeroamVehicle = vehicle;

                player.SetEntityInvincible(vehicle, true);

                player.ModPlayer.SetIntoVehicle(vehicle, 0);
            });
        }
    }
}
