using GTANetworkAPI;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Common.Default;
using TDS_Common.Enum;
using TDS_Server.Instance.Player;
using TDS_Server.Manager.Utility;
using TDS_Server_DB.Entity;

namespace TDS_Server.Instance.Lobby
{
    partial class MapCreateLobby
    {
        public override async Task<bool> AddPlayer(TDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, 0))
                return false;

            Workaround.SetPlayerInvincible(player.Client, true);
            player.Client.Position = new Vector3(-365.425, -131.809, 37.873);
            Workaround.FreezePlayer(player.Client, false);

            return true;
        }

        public void SetPosition(TDSPlayer player, float x, float y, float z, float rot)
        {
            player.Client.Position = new Vector3(x, y, z);
            player.Client.Rotation = new Vector3(0, 0, rot);
        }

        public async void GiveVehicle(TDSPlayer player, EFreeroamVehicleType vehType)
        {
            VehicleHash vehHash = await DbContext.FreeroamDefaultVehicle
                .Where(v => v.VehicleType == vehType)
                .Select(v => v.VehicleHash)
                .FirstOrDefaultAsync();
            if (vehHash == default)
                return;

            var pos = player.Client.Position;

            NAPI.Task.Run(() => {
                if (player.FreeroamVehicle != null)
                {
                    if (player.Client.IsInVehicle)
                        player.Client.WarpOutOfVehicle();
                    player.FreeroamVehicle.Delete();
                    player.FreeroamVehicle = null;
                }

                Vehicle? vehicle = NAPI.Vehicle.CreateVehicle(vehHash, pos, player.Client.Heading, 0, 0, player.Client.Name, dimension: Dimension);
                if (vehicle is null)
                    return;
                player.FreeroamVehicle = vehicle;

                Workaround.SetEntityInvincible(player.Client, vehicle, true);

                player.Client.SetIntoVehicle(vehicle, -1);
            });
        }
    }
}
