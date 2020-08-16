using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Data;
using System.Drawing;
using System.Threading.Tasks;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Utility;

namespace TDS_Server.Entity.LobbySystem.GangLobbySystem
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

            var gangColor = SharedUtils.GetColorFromHtmlRgba(gang.Entity.Color) ?? Color.FromArgb(255, 255, 255);
            var rgbaColor = new Rgba(gangColor.R, gangColor.G, gangColor.B, gangColor.A);
            await AltAsync.Do(() =>
            {
                foreach (var dbVehicle in gang.Entity.Vehicles)
                {
                    var vehicle = (ITDSVehicle)Alt.CreateVehicle((uint)dbVehicle.Model, new Position(dbVehicle.SpawnPosX, dbVehicle.SpawnPosY, dbVehicle.SpawnPosZ),
                        new DegreeRotation(dbVehicle.SpawnRotX, dbVehicle.SpawnRotY, dbVehicle.SpawnRotZ));
                    vehicle.PrimaryColorRgb = rgbaColor;
                    vehicle.NumberplateText = gang.Entity.Short;
                    vehicle.Dimension = (int)Dimension;

                    vehicle.Freeze(true, this);
                    vehicle.SetInvincible(true, this);
                    vehicle.SetStreamSyncedMetaData(EntityDataKey.GangId.ToString(), gang.Entity.Id);

                    //Todo: Disable collision of the vehicle (maybe on clientside on EntityStreamIn?)
                }
            });
        }

        #endregion Public Methods
    }
}
