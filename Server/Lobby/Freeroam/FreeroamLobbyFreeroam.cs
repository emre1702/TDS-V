using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.Handler;
using TDS_Shared.Data.Enums;

namespace TDS_Server.LobbySystem.Freeroam
{
    public class FreeroamLobbyFreeroam
    {
        private readonly FreeroamDataHandler _freeroamDataHandler;
        private readonly IBaseLobby _lobby;

        public FreeroamLobbyFreeroam(IBaseLobby lobby, FreeroamDataHandler freeroamDataHandler)
            => (_lobby, _freeroamDataHandler) = (lobby, freeroamDataHandler);

        public void GiveVehicle(ITDSPlayer player, FreeroamVehicleType vehType)
        {
            var vehicleHash = _freeroamDataHandler.GetDefaultHash(vehType);
            if (!vehicleHash.HasValue)
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

                var vehicle = NAPI.Vehicle.CreateVehicle(vehicleHash.Value, pos, player.Rotation.Z, 0, 0, player.Name, dimension: _lobby.MapHandler.Dimension) as ITDSVehicle;
                player.FreeroamVehicle = vehicle;

                player.SetEntityInvincible(vehicle!, true);

                player.SetIntoVehicle(vehicle, 0);
            });
        }

        public void SetPosition(ITDSPlayer player, float x, float y, float z, float rot)
        {
            player.Position = new Vector3(x, y, z);
            player.Rotation = new Vector3(0, 0, rot);
        }
    }
}
