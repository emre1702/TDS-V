using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Shared.Data.Enums;
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

            NAPI.Task.Run(() =>
            {
                player.SetInvincible(true);
                player.Freeze(false);

                if (Players.Count == 2)
                {
                    Players.Values.First(p => p != player).TriggerEvent(ToClientEvent.MapCreatorRequestAllObjectsForPlayer, player.Id);
                }
                else if (Players.Count > 2)
                {
                    player.TriggerEvent(ToClientEvent.MapCreatorSyncAllObjects, Serializer.ToBrowser(_currentMap), _lastId);
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

            NAPI.Task.Run(() =>
            {
                var pos = player.Position;
                if (player.FreeroamVehicle is { })
                {
                    if (player.IsInVehicle)
                        player.WarpOutOfVehicle();
                    player.FreeroamVehicle.Delete();
                    player.FreeroamVehicle = null;
                }

                var vehicle = NAPI.Vehicle.CreateVehicle(vehHash, pos, player.Rotation.Z, 0, 0, player.Name, dimension: Dimension) as ITDSVehicle;
                player.FreeroamVehicle = vehicle;

                player.SetEntityInvincible(vehicle!, true);

                player.SetIntoVehicle(vehicle, 0);
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
            player.Position = new Vector3(x, y, z);
            player.Rotation = new Vector3(0, 0, rot);
        }

        #endregion Public Methods
    }
}
