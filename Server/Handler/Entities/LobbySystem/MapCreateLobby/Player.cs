using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Shared.Data.Enums;
using TDS_Common.Manager.Utility;
using TDS_Server.Enums;
using TDS_Server.Instance.PlayerInstance;
using TDS_Server.Manager.PlayerManager;
using TDS_Server.Manager.Utility;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class MapCreateLobby
    {
        public override async Task<bool> AddPlayer(TDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, 0))
                return false;

            Workaround.SetPlayerInvincible(player.Player!, true);
            Workaround.FreezePlayer(player.Player!, false);

            if (Players.Count > 1)
            {
                NAPI.ClientEvent.TriggerClientEvent(player.Player, ToClientEvent.MapCreatorSyncAllObjects, Serializer.ToBrowser(_currentMap));
            }

            return true;
        }

        public override void RemovePlayer(TDSPlayer player)
        {
            base.RemovePlayer(player);

            if (player.Entity?.Id == Entity.OwnerId && Players.Count >= 1) {
                var newOwner = Players.ElementAt(CommonUtils.Rnd.Next(0, Players.Count));
                Entity.OwnerId = newOwner.Entity!.Id;
                PlayerDataSync.SetData(newOwner, PlayerDataKey.IsLobbyOwner, PlayerDataSyncMode.Player, true);
            }
        }

        public void SetPosition(TDSPlayer player, float x, float y, float z, float rot)
        {
            player.Player!.Position = new Vector3(x, y, z);
            player.Player!.Rotation = new Vector3(0, 0, rot);
        }

        public async void GiveVehicle(TDSPlayer player, EFreeroamVehicleType vehType)
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

            var pos = player.Player!.Position;

            NAPI.Task.Run(() => {
                if (player.FreeroamVehicle != null)
                {
                    if (player.Player.IsInVehicle)
                        player.Player.WarpOutOfVehicle();
                    player.FreeroamVehicle.Delete();
                    player.FreeroamVehicle = null;
                }

                Vehicle? vehicle = NAPI.Vehicle.CreateVehicle(vehHash, pos, player.Player.Heading, 0, 0, player.Player.Name, dimension: Dimension);
                if (vehicle is null)
                    return;
                player.FreeroamVehicle = vehicle;

                Workaround.SetEntityInvincible(player.Player, vehicle, true);

                player.Player.SetIntoVehicle(vehicle, -1);
            });
        }
    }
}
