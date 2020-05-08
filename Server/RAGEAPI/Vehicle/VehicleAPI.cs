using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Data.Models.GTA;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Vehicle
{
    class VehicleAPI : IVehicleAPI
    {
        private readonly EntityConvertingHandler _entityConvertingHandler;

        internal VehicleAPI(EntityConvertingHandler entityConvertingHandler)
            => _entityConvertingHandler = entityConvertingHandler;

        public ITDSVehicle Create(VehicleHash model, Position3D pos, Position3D rot, int color1, int color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0)
        {
            var instance = GTANetworkAPI.NAPI.Vehicle.CreateVehicle((GTANetworkAPI.VehicleHash)model, pos.ToMod(), rot.ToMod(), color1, color2, numberPlate, alpha, locked, engine, dimension: dimension);
            var modVehicle = _entityConvertingHandler.GetEntity(instance)!;
            return Init.GetTDSVehicle(modVehicle)!;
        }

        public ITDSVehicle Create(int model, Position3D pos, Position3D rot, int color1, int color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0)
        {
            var instance = GTANetworkAPI.NAPI.Vehicle.CreateVehicle(model, pos.ToMod(), rot.ToMod(), color1, color2, numberPlate, alpha, locked, engine, dimension: dimension);
            var modVehicle = _entityConvertingHandler.GetEntity(instance)!;
            return Init.GetTDSVehicle(modVehicle)!;
        }

        public ITDSVehicle Create(uint model, Position3D pos, float rot, int color1, int color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0)
        {
            var instance = GTANetworkAPI.NAPI.Vehicle.CreateVehicle(model, pos.ToMod(), rot, color1, color2, numberPlate, alpha, locked, engine, dimension: dimension);
            var modVehicle = _entityConvertingHandler.GetEntity(instance)!;
            return Init.GetTDSVehicle(modVehicle)!;
        }

        public ITDSVehicle Create(VehicleHash model, Position3D pos, float rot, int color1, int color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0)
        {
            var instance = GTANetworkAPI.NAPI.Vehicle.CreateVehicle((GTANetworkAPI.VehicleHash)model, pos.ToMod(), rot, color1, color2, numberPlate, alpha, locked, engine, dimension: dimension);
            var modVehicle = _entityConvertingHandler.GetEntity(instance)!;
            return Init.GetTDSVehicle(modVehicle)!;
        }

        public ITDSVehicle Create(VehicleHash model, Position3D pos, float rot, Color color1, Color color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0)
        {
            var instance = GTANetworkAPI.NAPI.Vehicle.CreateVehicle((GTANetworkAPI.VehicleHash)model, pos.ToMod(), rot, new GTANetworkAPI.Color(color1.ToArgb()), new GTANetworkAPI.Color(color2.ToArgb()), numberPlate, alpha, locked, engine, dimension: dimension);
            var modVehicle = _entityConvertingHandler.GetEntity(instance)!;
            return Init.GetTDSVehicle(modVehicle)!;
        }

        public ITDSVehicle Create(int model, Position3D pos, float rot, int color1, int color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0)
        {
            var instance = GTANetworkAPI.NAPI.Vehicle.CreateVehicle(model, pos.ToMod(), rot, color1, color2, numberPlate, alpha, locked, engine, dimension: dimension);
            var modVehicle = _entityConvertingHandler.GetEntity(instance)!;
            return Init.GetTDSVehicle(modVehicle)!;
        }

        public void ExplodeVehicle(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.ExplodeVehicle((vehicle as Vehicle)!._instance);

        public float GetVehicleBodyHealth(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleBodyHealth((vehicle as Vehicle)!._instance);

        public bool GetVehicleBulletproofTyres(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleBulletproofTyres((vehicle as Vehicle)!._instance);

        public int GetVehicleClass(VehicleHash model)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleClass((GTANetworkAPI.VehicleHash)model);

        public string GetVehicleClassName(int classId)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleClassName(classId);

        public Color GetVehicleCustomPrimaryColor(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleCustomPrimaryColor((vehicle as Vehicle)!._instance).ToTDS();

        public Color GetVehicleCustomSecondaryColor(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleCustomSecondaryColor((vehicle as Vehicle)!._instance).ToTDS();

        public bool GetVehicleCustomTires(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleCustomTires((vehicle as Vehicle)!._instance);

        public int GetVehicleDashboardColor(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleDashboardColor((vehicle as Vehicle)!._instance);

        public string GetVehicleDisplayName(VehicleHash model)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleDisplayName((GTANetworkAPI.VehicleHash)model);

        public IEntity GetVehicleDriver(IVehicle vehicle)
            => _entityConvertingHandler.GetEntity(GTANetworkAPI.NAPI.Vehicle.GetVehicleDriver((vehicle as Vehicle)!._instance));

        public float GetVehicleEngineHealth(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleEngineHealth((vehicle as Vehicle)!._instance);

        public float GetVehicleEnginePowerMultiplier(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleEnginePowerMultiplier((vehicle as Vehicle)!._instance);

        public bool GetVehicleEngineStatus(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleEngineStatus((vehicle as Vehicle)!._instance);

        public float GetVehicleEngineTorqueMultiplier(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleEngineTorqueMultiplier((vehicle as Vehicle)!._instance);

        public bool GetVehicleExtra(IVehicle vehicle, int slot)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleExtra((vehicle as Vehicle)!._instance, slot);

        public float GetVehicleHealth(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleHealth((vehicle as Vehicle)!._instance);

        public int GetVehicleLivery(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleLivery((vehicle as Vehicle)!._instance);

        public bool GetVehicleLocked(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleLocked((vehicle as Vehicle)!._instance);

        public float GetVehicleMaxAcceleration(VehicleHash model)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleMaxAcceleration((GTANetworkAPI.VehicleHash)model);

        public float GetVehicleMaxBraking(VehicleHash model)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleMaxBraking((GTANetworkAPI.VehicleHash)model);

        public int GetVehicleMaxOccupants(VehicleHash model)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleMaxOccupants((GTANetworkAPI.VehicleHash)model);

        public int GetVehicleMaxPassengers(VehicleHash model)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleMaxPassengers((GTANetworkAPI.VehicleHash)model);

        public float GetVehicleMaxSpeed(VehicleHash model)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleMaxSpeed((GTANetworkAPI.VehicleHash)model);

        public float GetVehicleMaxTraction(VehicleHash model)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleMaxTraction((GTANetworkAPI.VehicleHash)model);

        public int GetVehicleMod(IVehicle vehicle, int slot)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleMod((vehicle as Vehicle)!._instance, slot);

        public Color GetVehicleNeonColor(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleNeonColor((vehicle as Vehicle)!._instance).ToTDS();

        public bool GetVehicleNeonState(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleNeonState((vehicle as Vehicle)!._instance);

        public string GetVehicleNumberPlate(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleNumberPlate((vehicle as Vehicle)!._instance);

        public int GetVehicleNumberPlateStyle(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehicleNumberPlateStyle((vehicle as Vehicle)!._instance);

        public List<IEntity> GetVehicleOccupants(IVehicle vehicle)
            => vehicle.Occupants;

        public List<IEntity> GetVehicleOccupants(IVehicle vehicle, int maxOccupants)
            => vehicle.Occupants.Take(maxOccupants).ToList();

        public int GetVehiclePearlescentColor(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehiclePearlescentColor((vehicle as Vehicle)!._instance);

        public int GetVehiclePrimaryColor(IVehicle vehicle)
            => GTANetworkAPI.NAPI.Vehicle.GetVehiclePrimaryColor((vehicle as Vehicle)!._instance);

        public VehiclePaint GetVehiclePrimaryPaint(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.GetVehiclePrimaryPaint((vehicle as Vehicle)!._instance).ToTDS();

        public int GetVehicleSecondaryColor(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.GetVehicleSecondaryColor((vehicle as Vehicle)!._instance);

        public VehiclePaint GetVehicleSecondaryPaint(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.GetVehicleSecondaryPaint((vehicle as Vehicle)!._instance).ToTDS();

        public bool GetVehicleSirenState(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.GetVehicleSirenState((vehicle as Vehicle)!._instance);

        public bool GetVehicleSpecialLightStatus(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.GetVehicleSpecialLightStatus((vehicle as Vehicle)!._instance);

        public ITDSVehicle? GetVehicleTrailer(IVehicle vehicle)
        => vehicle.Trailer;

        public ITDSVehicle? GetVehicleTraileredBy(IVehicle vehicle)
        => vehicle.TraileredBy;

        public int GetVehicleTrimColor(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.GetVehicleTrimColor((vehicle as Vehicle)!._instance);

        public Color GetVehicleTyreSmokeColor(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.GetVehicleTyreSmokeColor((vehicle as Vehicle)!._instance).ToTDS();

        public int GetVehicleWheelColor(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.GetVehicleWheelColor((vehicle as Vehicle)!._instance);

        public int GetVehicleWheelType(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.GetVehicleWheelType((vehicle as Vehicle)!._instance);

        public int GetVehicleWindowTint(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.GetVehicleWindowTint((vehicle as Vehicle)!._instance);

        public void RemoveVehicleMod(IVehicle vehicle, int modType)
        => GTANetworkAPI.NAPI.Vehicle.RemoveVehicleMod((vehicle as Vehicle)!._instance, modType);

        public void RepairVehicle(IVehicle vehicle)
        => GTANetworkAPI.NAPI.Vehicle.RepairVehicle((vehicle as Vehicle)!._instance);

        public void SetVehicleBodyHealth(IVehicle vehicle, float health)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleBodyHealth((vehicle as Vehicle)!._instance, health);

        public void SetVehicleBulletproofTyres(IVehicle vehicle, bool state)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleBulletproofTyres((vehicle as Vehicle)!._instance, state);

        public void SetVehicleCustomPrimaryColor(IVehicle vehicle, int red, int green, int blue)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleCustomPrimaryColor((vehicle as Vehicle)!._instance, red, green, blue);

        public void SetVehicleCustomSecondaryColor(IVehicle vehicle, int red, int green, int blue)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleCustomSecondaryColor((vehicle as Vehicle)!._instance, red, green, blue);

        public void SetVehicleCustomTires(IVehicle vehicle, bool enable)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleCustomTires((vehicle as Vehicle)!._instance, enable);

        public void SetVehicleDashboardColor(IVehicle vehicle, int type)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleDashboardColor((vehicle as Vehicle)!._instance, type);

        public void SetVehicleEngineHealth(IVehicle vehicle, float health)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleEngineHealth((vehicle as Vehicle)!._instance, health);

        public void SetVehicleEnginePowerMultiplier(IVehicle vehicle, float mult)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleEnginePowerMultiplier((vehicle as Vehicle)!._instance, mult);

        public void SetVehicleEngineStatus(IVehicle vehicle, bool state)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleEngineStatus((vehicle as Vehicle)!._instance, state);

        public void SetVehicleEngineTorqueMultiplier(IVehicle vehicle, float mult)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleEngineTorqueMultiplier((vehicle as Vehicle)!._instance, mult);

        public void SetVehicleExtra(IVehicle vehicle, int slot, bool enabled)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleExtra((vehicle as Vehicle)!._instance, slot, enabled);

        public void SetVehicleHealth(IVehicle vehicle, float health)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleHealth((vehicle as Vehicle)!._instance, health);

        public void SetVehicleLivery(IVehicle vehicle, int livery)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleLivery((vehicle as Vehicle)!._instance, livery);

        public void SetVehicleLocked(IVehicle vehicle, bool locked)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleLocked((vehicle as Vehicle)!._instance, locked);

        public void SetVehicleMod(IVehicle vehicle, int modType, int mod)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleMod((vehicle as Vehicle)!._instance, modType, mod);

        public void SetVehicleNeonColor(IVehicle vehicle, int r, int g, int b)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleNeonColor((vehicle as Vehicle)!._instance, r, g, b);

        public void SetVehicleNeonState(IVehicle vehicle, bool turnedOn)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleNeonState((vehicle as Vehicle)!._instance, turnedOn);

        public void SetVehicleNumberPlate(IVehicle vehicle, string plate)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleNumberPlate((vehicle as Vehicle)!._instance, plate);

        public void SetVehicleNumberPlateStyle(IVehicle vehicle, int style)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleNumberPlateStyle((vehicle as Vehicle)!._instance, style);

        public void SetVehiclePearlescentColor(IVehicle vehicle, int color)
        => GTANetworkAPI.NAPI.Vehicle.SetVehiclePearlescentColor((vehicle as Vehicle)!._instance, color);

        public void SetVehiclePrimaryColor(IVehicle vehicle, int color)
        => GTANetworkAPI.NAPI.Vehicle.SetVehiclePrimaryColor((vehicle as Vehicle)!._instance, color);

        public void SetVehiclePrimaryPaint(IVehicle vehicle, int paintType, int color)
        => GTANetworkAPI.NAPI.Vehicle.SetVehiclePrimaryPaint((vehicle as Vehicle)!._instance, paintType, color);

        public void SetVehicleSecondaryColor(IVehicle vehicle, int color)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleSecondaryColor((vehicle as Vehicle)!._instance, color);

        public void SetVehicleSecondaryPaint(IVehicle vehicle, int paintType, int color)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleSecondaryPaint((vehicle as Vehicle)!._instance, paintType, color);

        public void SetVehicleSirenState(IVehicle vehicle, bool state)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleSirenState((vehicle as Vehicle)!._instance, state);

        public void SetVehicleSpecialLightStatus(IVehicle vehicle, bool state)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleSpecialLightStatus((vehicle as Vehicle)!._instance, state);

        public void SetVehicleTrimColor(IVehicle vehicle, int type)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleTrimColor((vehicle as Vehicle)!._instance, type);

        public void SetVehicleTyreSmokeColor(IVehicle vehicle, Color color)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleTyreSmokeColor((vehicle as Vehicle)!._instance, color.ToMod());

        public void SetVehicleWheelColor(IVehicle vehicle, int color)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleWheelColor((vehicle as Vehicle)!._instance, color);

        public void SetVehicleWheelType(IVehicle vehicle, int type)
        => GTANetworkAPI.NAPI.Vehicle.SetVehicleWheelType((vehicle as Vehicle)!._instance, type);

        public void SetVehicleWindowTint(IVehicle vehicle, int type)
            => GTANetworkAPI.NAPI.Vehicle.SetVehicleWindowTint((vehicle as Vehicle)!._instance, type);

        public void SpawnVehicle(IVehicle vehicle, Position3D position, float heading = 0)
            => GTANetworkAPI.NAPI.Vehicle.SpawnVehicle((vehicle as Vehicle)!._instance, position.ToMod(), heading);
    }
}
