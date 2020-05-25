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
        #region Public Methods

        public override async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, 0))
                return false;

            ModAPI.Thread.RunInMainThread(() =>
            {
                player.ModPlayer?.SetInvincible(true);
                player.ModPlayer?.Freeze(false);

                if (Players.Count == 2)
                {
                    Players.Values.First(p => p != player).SendEvent(ToClientEvent.MapCreatorRequestAllObjectsForPlayer, player.Id);
                }
                else if (Players.Count > 2)
                {
                    player.SendEvent(ToClientEvent.MapCreatorSyncAllObjects, Serializer.ToBrowser(_currentMap), _lastId);
                }
            });

            return true;
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

            ModAPI.Thread.RunInMainThread(() =>
            {
                var pos = player.ModPlayer!.Position;
                if (player.FreeroamVehicle is { })
                {
                    if (player.ModPlayer.IsInVehicle)
                        player.ModPlayer.WarpOutOfVehicle();
                    player.FreeroamVehicle.Delete();
                    player.FreeroamVehicle = null;
                }

                var vehicle = ModAPI.Vehicle.Create(vehHash, pos, player.ModPlayer.Rotation.Z, 0, 0, player.ModPlayer.Name, dimension: Dimension);
                player.FreeroamVehicle = vehicle;

                player.SetEntityInvincible(vehicle, true);

                player.ModPlayer.SetIntoVehicle(vehicle, 0);
            });
        }

        public override async Task RemovePlayer(ITDSPlayer player)
        {
            await base.RemovePlayer(player);

            if (player.Entity?.Id == Entity.OwnerId && Players.Count >= 1)
            {
                var newOwner = SharedUtils.GetRandom(Players.Values);
                Entity.OwnerId = newOwner.Entity!.Id;
                DataSyncHandler.SetData(newOwner, PlayerDataKey.IsLobbyOwner, DataSyncMode.Player, true);
            }
        }

        public void SetPosition(ITDSPlayer player, float x, float y, float z, float rot)
        {
            player.ModPlayer!.Position = new Position3D(x, y, z);
            player.ModPlayer!.Rotation = new Position3D(0, 0, rot);
        }

        #endregion Public Methods
    }
}
