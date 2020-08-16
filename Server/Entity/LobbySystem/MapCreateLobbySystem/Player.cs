using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;
using TDS_Shared.Default;

namespace TDS_Server.Entity.LobbySystem.MapCreateLobbySystem
{
    partial class MapCreateLobby
    {
        #region Public Methods

        public override async Task<bool> AddPlayer(ITDSPlayer player, uint? teamindex)
        {
            if (!await base.AddPlayer(player, 0))
                return false;

            await AltAsync.Do(() =>
            {
                player.SetInvincible(true);
                player.Freeze(false);

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

            await AltAsync.Do(() =>
            {
                var pos = player.Position;
                if (player.FreeroamVehicle is { })
                {
                    if (player.IsInVehicle)
                        player.WarpOutOfVehicle();
                    player.FreeroamVehicle.Delete();
                    player.FreeroamVehicle = null;
                }

                var vehicle = (ITDSVehicle)Alt.CreateVehicle((uint)vehHash, pos, player.Rotation);
                vehicle.Dimension = (int)Dimension;
                vehicle.NumberplateText = player.Name;
                player.FreeroamVehicle = vehicle;

                player.SetEntityInvincible(vehicle, true);

                player.SetIntoVehicle(vehicle, -1);
            });
        }

        public override async Task RemovePlayer(ITDSPlayer player)
        {
            await base.RemovePlayer(player);

            if (player.Entity?.Id == Entity.OwnerId && Players.Count >= 1)
            {
                var newOwner = SharedUtils.GetRandom(Players.Values);
                Entity.OwnerId = newOwner.Entity!.Id;
                newOwner.SetClientMetaData(PlayerDataKey.IsLobbyOwner.ToString(), true);
            }
        }

        public void SetPosition(ITDSPlayer player, float x, float y, float z, float rot)
        {
            player.Position = new Position(x, y, z);
            player.Rotation = new Position(0, 0, rot);
        }

        #endregion Public Methods
    }
}
