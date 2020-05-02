using System.Drawing;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Blip;
using TDS_Server.Data.Interfaces.ModAPI.TextLabel;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Shared.Data.Default;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Handler.Entities.GangSystem
{
    public class GangHouse : IGangHouse
    {
        public GangHouses Entity { get; }
        public Position3D Position { get; }
        public float SpawnRotation => Entity.Rot;
        public ITextLabel? TextLabel { get; set; }
        public IBlip? Blip { get; set; }

        private readonly int _cost;

        private readonly IModAPI _modAPI;

        public GangHouse(GangHouses entity, int cost, IModAPI modAPI)
        {
            _modAPI = modAPI;

            Entity = entity;
            _cost = cost;

            Position = new Position3D(entity.PosX, entity.PosY, entity.PosZ);
        }

        public string GetTextLabelText()
            => Entity.OwnerGang switch
            {
                { } => $"{Entity.OwnerGang}",
                null => $"-\nLevel {Entity.NeededGangLevel}\n${_cost}"
            };
    }
}
