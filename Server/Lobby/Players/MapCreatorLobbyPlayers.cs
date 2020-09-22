using System.Threading.Tasks;
using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces.LobbySystem.EventsHandlers;
using TDS_Server.Data.Interfaces.LobbySystem.Lobbies;
using TDS_Server.LobbySystem.BansHandlers;
using TDS_Server.LobbySystem.TeamHandlers;
using TDS_Shared.Core;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Server.LobbySystem.Players
{
    public class MapCreatorLobbyPlayers : BaseLobbyPlayers
    {
        public MapCreatorLobbyPlayers(IBaseLobby lobby, IBaseLobbyEventsHandler events, BaseLobbyTeamsHandler teams, BaseLobbyBansHandler bans)
            : base(lobby, events, teams, bans)
        {
        }

        public override async Task<bool> AddPlayer(ITDSPlayer player, int teamIndex)
        {
            var worked = await base.AddPlayer(player, teamIndex);
            if (!worked)
                return false;

            NAPI.Task.Run(() =>
            {
                player.SetInvincible(true);
                player.Freeze(false);
            });
            return true;
        }

        public override async Task<bool> RemovePlayer(ITDSPlayer player)
        {
            bool isLobbyOwner = IsLobbyOwner(player);
            var worked = await base.RemovePlayer(player);
            if (!worked)
                return false;

            if (isLobbyOwner && await Any())
                await SetNewRandomLobbyOwner();

            return true;
        }

        /*
         *
         *

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

        public void SetPosition(ITDSPlayer player, float x, float y, float z, float rot)
        {
            player.Position = new Vector3(x, y, z);
            player.Rotation = new Vector3(0, 0, rot);
        }*/
    }
}
