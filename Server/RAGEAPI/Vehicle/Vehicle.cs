using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Vehicle;
using TDS_Server.Data.Models.GTA;
using TDS_Server.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.RAGEAPI.Vehicle
{
    class Vehicle : Entity.Entity, IVehicle
    {
        internal readonly GTANetworkAPI.Vehicle _instance;
        private readonly EntityConvertingHandler _entityConvertingHandler;

        public Vehicle(GTANetworkAPI.Vehicle instance, EntityConvertingHandler entityConvertingHandler) : base(instance)
            => (_instance, _entityConvertingHandler) = (instance, entityConvertingHandler);


        public List<IEntity> Occupants
            => _instance.Occupants.Select(o => _entityConvertingHandler.GetEntity(o)).ToList();

        public int MaxOccupants => _instance.MaxOccupants;

        public int WheelColor
        {
            get => _instance.WheelColor;
            set => _instance.WheelColor = value;
        }
        public bool EngineStatus
        {
            get => _instance.EngineStatus;
            set => _instance.EngineStatus = value;
        }
        public Color TyreSmokeColor
        {
            get => _instance.TyreSmokeColor.ToTDS();
            set => _instance.TyreSmokeColor = value.ToMod();
        }
        public VehiclePaint PrimaryPaint
        {
            get => _instance.PrimaryPaint.ToTDS();
            set => _instance.PrimaryPaint = value.ToMod();
        }
        public VehiclePaint SecondaryPaint
        {
            get => _instance.SecondaryPaint.ToTDS();
            set => _instance.SecondaryPaint = value.ToMod();
        }
        public int WindowTint
        {
            get => _instance.WindowTint;
            set => _instance.WindowTint = value;
        }
        public float EnginePowerMultiplier
        {
            get => _instance.EnginePowerMultiplier;
            set => _instance.EnginePowerMultiplier = value;
        }
        public float EngineTorqueMultiplier
        {
            get => _instance.EngineTorqueMultiplier;
            set => _instance.EngineTorqueMultiplier = value;
        }
        public Color NeonColor
        {
            get => _instance.NeonColor.ToTDS();
            set => _instance.NeonColor = value.ToMod();
        }
        public int DashboardColor
        {
            get => _instance.DashboardColor;
            set => _instance.DashboardColor = value;
        }
        public int WheelType
        {
            get => _instance.WheelType;
            set => _instance.WheelType = value;
        }
        public int TrimColor
        {
            get => _instance.TrimColor;
            set => _instance.TrimColor = value;
        }

        public string DisplayName => _instance.DisplayName;

        public int MaxPassengers => _instance.MaxPassengers;

        public float MaxSpeed => _instance.MaxSpeed;

        public float MaxAcceleration => _instance.MaxAcceleration;

        public float MaxTraction => _instance.MaxTraction;

        public float MaxBraking => _instance.MaxBraking;

        public bool Locked
        {
            get => _instance.Locked;
            set => _instance.Locked = value;
        }
        public bool Neons
        {
            get => _instance.Neons;
            set => _instance.Neons = value;
        }

        public int Class => _instance.Class;

        public int ClassName => _instance.ClassName;

        public int NumberPlateStyle
        {
            get => _instance.NumberPlateStyle;
            set => _instance.NumberPlateStyle = value;
        }

        public ITDSPlayer? Controller => Init.GetTDSPlayerIfLoggedIn(_entityConvertingHandler.GetEntity(_instance.Controller));

        public int PrimaryColor
        {
            get => _instance.PrimaryColor;
            set => _instance.PrimaryColor = value;
        }
        public int SecondaryColor
        {
            get => _instance.SecondaryColor;
            set => _instance.SecondaryColor = value;
        }
        public int PearlescentColor
        {
            get => _instance.PearlescentColor;
            set => _instance.PearlescentColor = value;
        }
        public Color CustomSecondaryColor
        {
            get => _instance.CustomSecondaryColor.ToTDS();
            set => _instance.CustomSecondaryColor = value.ToMod();
        }
        public float Health
        {
            get => _instance.Health;
            set => _instance.Health = value;
        }
        public int Livery
        {
            get => _instance.Livery;
            set => _instance.Livery = value;
        }
        public Color CustomPrimaryColor
        {
            get => _instance.CustomPrimaryColor.ToTDS();
            set => _instance.CustomPrimaryColor = value.ToMod();
        }

        public ITDSVehicle? TraileredBy => Init.GetTDSVehicle(_entityConvertingHandler.GetEntity(_instance.TraileredBy));

        public bool Siren => _instance.Siren;

        public string NumberPlate
        {
            get => _instance.NumberPlate;
            set => _instance.NumberPlate = value;
        }
        public bool SpecialLight
        {
            get => _instance.SpecialLight;
            set => _instance.SpecialLight = value;
        }
        public bool CustomTires
        {
            get => _instance.CustomTires;
            set => _instance.CustomTires = value;
        }
        public bool BulletproofTyres
        {
            get => _instance.BulletproofTyres;
            set => _instance.BulletproofTyres = value;
        }

        public ITDSVehicle? Trailer => Init.GetTDSVehicle(_entityConvertingHandler.GetEntity(_instance.Trailer));

        public bool GetExtra(int extra)
            => _instance.GetExtra(extra);

        public int GetMod(int slot)
            => _instance.GetMod(slot);

        public void RemoveMod(int slot)
            => _instance.RemoveMod(slot);

        public void Repair()
            => _instance.Repair();

        public void SetExtra(int extra, bool enabled)
            => _instance.SetExtra(extra, enabled);

        public void SetMod(int slot, int mod)
            => _instance.SetMod(slot, mod);

        public void Spawn(Position3D position, float heading = 0)
            => _instance.Spawn(position.ToVector3(), heading);
    }
}
