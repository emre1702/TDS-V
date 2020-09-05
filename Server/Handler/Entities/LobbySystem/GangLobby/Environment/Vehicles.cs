﻿using GTANetworkAPI;
using System.Threading.Tasks;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.Entities.LobbySystem
{
    partial class GangLobby
    {
        #region Public Methods

        public async Task LoadGangVehicles(IGang gang)
        {
            if (gang.Entity.Vehicles is { })
                return;

            await gang.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.Entry(gang.Entity).Collection(e => e.Vehicles).LoadAsync();
            });

            if (gang.Entity.Vehicles is null || gang.Entity.Vehicles.Count == 0)
                return;

            NAPI.Task.Run(() =>
            {
                foreach (var dbVehicle in gang.Entity.Vehicles)
                {
                    var vehicle = NAPI.Vehicle.CreateVehicle(dbVehicle.Model, new Vector3(dbVehicle.SpawnPosX, dbVehicle.SpawnPosY, dbVehicle.SpawnPosZ),
                        new Vector3(dbVehicle.SpawnRotX, dbVehicle.SpawnRotY, dbVehicle.SpawnRotZ),
                        dbVehicle.Color1, dbVehicle.Color2, gang.Entity.Short, 255, dimension: Dimension) as ITDSVehicle;
                    vehicle!.Lobby = this;
                    vehicle.Freeze(true);
                    vehicle.SetInvincible(true);
                    vehicle.SetCollisionsless(true);
                    DataSyncHandler.SetData(vehicle, EntityDataKey.GangId, DataSyncMode.Lobby, gang.Entity.Id, toLobby: this);
                    //Todo: Disable collision of the vehicle (maybe on clientside an EntityStreamIn?)
                }
            });
        }

        #endregion Public Methods
    }
}
