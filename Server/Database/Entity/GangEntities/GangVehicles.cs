using TDS_Shared.Data.Enums;

namespace TDS_Server.Database.Entity.GangEntities
{
    public class GangVehicles
    {
        public int Id { get; set; }
        public int Color1 { get; set; }
        public int Color2 { get; set; }
        public int GangId { get; set; }
        public VehicleHash Model { get; set; }
        public float SpawnPosX { get; set; }
        public float SpawnPosY { get; set; }
        public float SpawnPosZ { get; set; }
        public float SpawnRotX { get; set; }
        public float SpawnRotY { get; set; }
        public float SpawnRotZ { get; set; }


        public virtual Gangs Gang { get; set; }
    }
}
