using System.Collections.Generic;
using System.Drawing;
using TDS_Server.Data.Models.GTA;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Data.Interfaces.ModAPI.Vehicle
{
    #nullable enable
    public interface IVehicle : IEntity
    {
        public int WheelColor { get; set; }
        public bool EngineStatus { get; set; }
        public Color TyreSmokeColor { get; set; }
        public VehiclePaint PrimaryPaint { get; set; }
        public VehiclePaint SecondaryPaint { get; set; }
        public int WindowTint { get; set; }
        public float EnginePowerMultiplier { get; set; }
        public float EngineTorqueMultiplier { get; set; }
        public Color NeonColor { get; set; }
        public int DashboardColor { get; set; }
        public int WheelType { get; set; }
        public int TrimColor { get; set; }
        public string DisplayName { get; }
        public List<IEntity> Occupants { get; }
        public int MaxOccupants { get; }
        public int MaxPassengers { get; }
        public float MaxSpeed { get; }
        public float MaxAcceleration { get; }
        public float MaxTraction { get; }
        public float MaxBraking { get; }
        public bool Locked { get; set; }
        public bool Neons { get; set; }
        public int Class { get; }
        public int ClassName { get; }
        public int NumberPlateStyle { get; set; }
        public ITDSPlayer? Controller { get; }
        public int PrimaryColor { get; set; }
        public int SecondaryColor { get; set; }
        public int PearlescentColor { get; set; }
        public Color CustomSecondaryColor { get; set; }
        public float Health { get; set; }
        public int Livery { get; set; }
        public Color CustomPrimaryColor { get; set; }
        public ITDSVehicle? TraileredBy { get; }
        public bool Siren { get; }
        public string NumberPlate { get; set; }
        public bool SpecialLight { get; set; }
        public bool CustomTires { get; set; }
        public bool BulletproofTyres { get; set; }
        public ITDSVehicle? Trailer { get; }

        public bool GetExtra(int extra);
        public int GetMod(int slot);
        public void RemoveMod(int slot);
        public void Repair();
        public void SetExtra(int extra, bool enabled);
        public void SetMod(int slot, int mod);
        public void Spawn(Position3D position, float heading = 0);
    }
}
