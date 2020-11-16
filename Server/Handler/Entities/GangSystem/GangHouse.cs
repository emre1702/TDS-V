using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;
using TDS.Server.Database.Entity.GangEntities;

namespace TDS.Server.Handler.Entities.GangSystem
{
    public class GangHouse : IGangHouse
    {
        public ITDSBlip? Blip { get; set; }
        public GangHouses Entity { get; }
        public Vector3 Position { get; }
        public float SpawnRotation => Entity.Rot;
        public ITDSTextLabel? TextLabel { get; set; }
        public bool LoadedInGangLobby { get; set; }

        public string TextLabelText => Entity.OwnerGang switch
        {
            { } => $"{Entity.OwnerGang}",
            null => $"-\nLevel {Entity.NeededGangLevel}\n${_cost}"
        };

        private readonly int _cost;

        public GangHouse(GangHouses entity, int cost)
        {
            Entity = entity;
            _cost = cost;

            Position = new Vector3(entity.PosX, entity.PosY, entity.PosZ);
        }
    }
}
