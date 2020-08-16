using AltV.Net.Data;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Server.Database.Entity.GangEntities;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Server.Entity.GangSystem
{
    public class GangHouse : IGangHouse
    {
        #region Private Fields

        private readonly int _cost;

        #endregion Private Fields

        #region Public Constructors

        public GangHouse(GangHouses entity, int cost)
        {
            Entity = entity;
            _cost = cost;

            Position = new Position(entity.PosX, entity.PosY, entity.PosZ);
        }

        #endregion Public Constructors

        #region Public Properties

        public ITDSBlip? Blip { get; set; }
        public GangHouses Entity { get; }
        public Position Position { get; }
        public float SpawnRotation => Entity.Rot;
        public ITDSTextLabel? TextLabel { get; set; }

        #endregion Public Properties

        #region Public Methods

        public string GetTextLabelText()
            => Entity.OwnerGang switch
            {
                { } => $"{Entity.OwnerGang}",
                null => $"-\nLevel {Entity.NeededGangLevel}\n${_cost}"
            };

        #endregion Public Methods
    }
}
