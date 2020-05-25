﻿using System.Collections.Generic;
using System.Drawing;
using TDS_Server.Data.Models.GTA;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Vehicle
{
#nullable enable

    public interface IVehicleAPI
    {
        #region Public Methods

        IVehicle Create(VehicleHash model, Position3D pos, Position3D rot, int color1, int color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0);

        IVehicle Create(int model, Position3D pos, Position3D rot, int color1, int color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0);

        IVehicle Create(uint model, Position3D pos, float rot, int color1, int color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0);

        IVehicle Create(VehicleHash model, Position3D pos, float rot, int color1, int color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0);

        IVehicle Create(VehicleHash model, Position3D pos, float rot, Color color1, Color color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0);

        IVehicle Create(int model, Position3D pos, float rot, int color1, int color2, string numberPlate = "", byte alpha = 255, bool locked = false, bool engine = true, uint dimension = 0);

        void ExplodeVehicle(IVehicle vehicle);

        float GetVehicleBodyHealth(IVehicle vehicle);

        bool GetVehicleBulletproofTyres(IVehicle vehicle);

        int GetVehicleClass(VehicleHash model);

        string GetVehicleClassName(int classId);

        Color GetVehicleCustomPrimaryColor(IVehicle vehicle);

        Color GetVehicleCustomSecondaryColor(IVehicle vehicle);

        bool GetVehicleCustomTires(IVehicle vehicle);

        int GetVehicleDashboardColor(IVehicle vehicle);

        string GetVehicleDisplayName(VehicleHash model);

        IEntity? GetVehicleDriver(IVehicle vehicle);

        float GetVehicleEngineHealth(IVehicle vehicle);

        float GetVehicleEnginePowerMultiplier(IVehicle vehicle);

        bool GetVehicleEngineStatus(IVehicle vehicle);

        float GetVehicleEngineTorqueMultiplier(IVehicle vehicle);

        bool GetVehicleExtra(IVehicle vehicle, int slot);

        float GetVehicleHealth(IVehicle vehicle);

        int GetVehicleLivery(IVehicle vehicle);

        bool GetVehicleLocked(IVehicle vehicle);

        float GetVehicleMaxAcceleration(VehicleHash model);

        float GetVehicleMaxBraking(VehicleHash model);

        int GetVehicleMaxOccupants(VehicleHash model);

        int GetVehicleMaxPassengers(VehicleHash model);

        float GetVehicleMaxSpeed(VehicleHash model);

        float GetVehicleMaxTraction(VehicleHash model);

        int GetVehicleMod(IVehicle vehicle, int slot);

        Color GetVehicleNeonColor(IVehicle vehicle);

        bool GetVehicleNeonState(IVehicle vehicle);

        string GetVehicleNumberPlate(IVehicle vehicle);

        int GetVehicleNumberPlateStyle(IVehicle vehicle);

        List<IEntity> GetVehicleOccupants(IVehicle vehicle);

        List<IEntity> GetVehicleOccupants(IVehicle vehicle, int maxOccupants);

        int GetVehiclePearlescentColor(IVehicle vehicle);

        int GetVehiclePrimaryColor(IVehicle vehicle);

        VehiclePaint GetVehiclePrimaryPaint(IVehicle vehicle);

        int GetVehicleSecondaryColor(IVehicle vehicle);

        VehiclePaint GetVehicleSecondaryPaint(IVehicle vehicle);

        bool GetVehicleSirenState(IVehicle vehicle);

        bool GetVehicleSpecialLightStatus(IVehicle vehicle);

        IVehicle? GetVehicleTrailer(IVehicle vehicle);

        IVehicle? GetVehicleTraileredBy(IVehicle vehicle);

        int GetVehicleTrimColor(IVehicle vehicle);

        Color GetVehicleTyreSmokeColor(IVehicle vehicle);

        int GetVehicleWheelColor(IVehicle vehicle);

        int GetVehicleWheelType(IVehicle vehicle);

        int GetVehicleWindowTint(IVehicle vehicle);

        void RemoveVehicleMod(IVehicle vehicle, int modType);

        void RepairVehicle(IVehicle vehicle);

        void SetVehicleBodyHealth(IVehicle vehicle, float health);

        void SetVehicleBulletproofTyres(IVehicle vehicle, bool state);

        void SetVehicleCustomPrimaryColor(IVehicle vehicle, int red, int green, int blue);

        void SetVehicleCustomSecondaryColor(IVehicle vehicle, int red, int green, int blue);

        void SetVehicleCustomTires(IVehicle vehicle, bool enable);

        void SetVehicleDashboardColor(IVehicle vehicle, int type);

        void SetVehicleEngineHealth(IVehicle vehicle, float health);

        void SetVehicleEnginePowerMultiplier(IVehicle vehicle, float mult);

        void SetVehicleEngineStatus(IVehicle vehicle, bool state);

        void SetVehicleEngineTorqueMultiplier(IVehicle vehicle, float mult);

        void SetVehicleExtra(IVehicle vehicle, int slot, bool enabled);

        void SetVehicleHealth(IVehicle vehicle, float health);

        void SetVehicleLivery(IVehicle vehicle, int livery);

        void SetVehicleLocked(IVehicle vehicle, bool locked);

        void SetVehicleMod(IVehicle vehicle, int modType, int mod);

        void SetVehicleNeonColor(IVehicle vehicle, int r, int g, int b);

        void SetVehicleNeonState(IVehicle vehicle, bool turnedOn);

        void SetVehicleNumberPlate(IVehicle vehicle, string plate);

        void SetVehicleNumberPlateStyle(IVehicle vehicle, int style);

        void SetVehiclePearlescentColor(IVehicle vehicle, int color);

        void SetVehiclePrimaryColor(IVehicle vehicle, int color);

        void SetVehiclePrimaryPaint(IVehicle vehicle, int paintType, int color);

        void SetVehicleSecondaryColor(IVehicle vehicle, int color);

        void SetVehicleSecondaryPaint(IVehicle vehicle, int paintType, int color);

        void SetVehicleSirenState(IVehicle vehicle, bool state);

        void SetVehicleSpecialLightStatus(IVehicle vehicle, bool state);

        void SetVehicleTrimColor(IVehicle vehicle, int type);

        void SetVehicleTyreSmokeColor(IVehicle vehicle, Color color);

        void SetVehicleWheelColor(IVehicle vehicle, int color);

        void SetVehicleWheelType(IVehicle vehicle, int type);

        void SetVehicleWindowTint(IVehicle vehicle, int type);

        void SpawnVehicle(IVehicle vehicle, Position3D position, float heading = 0);

        #endregion Public Methods
    }
}
