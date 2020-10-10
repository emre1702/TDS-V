using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Interfaces;
using TDS_Server.Database.Entity.GangEntities;

namespace TDS_Server.Handler.Entities.GangSystem
{
    public class GangHouse : IGangHouse
    {
        private readonly int _cost;

        public GangHouse(GangHouses entity, int cost)
        {
            Entity = entity;
            _cost = cost;

            Position = new Vector3(entity.PosX, entity.PosY, entity.PosZ);
        }

        public ITDSBlip? Blip { get; set; }
        public GangHouses Entity { get; }
        public Vector3 Position { get; }
        public float SpawnRotation => Entity.Rot;
        public ITDSTextLabel? TextLabel { get; set; }
        public bool LoadedInGangLobby { get; set; }

        public string GetTextLabelText()
            => Entity.OwnerGang switch
            {
                { } => $"{Entity.OwnerGang}",
                null => $"-\nLevel {Entity.NeededGangLevel}\n${_cost}"
            };
    }
}
