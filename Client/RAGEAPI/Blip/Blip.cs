using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Blip
{
    internal class Blip : RAGE.Elements.Blip, IBlip
    {
        #region Public Constructors

        public Blip(ushort id, ushort remoteId) : base(id, remoteId)
        { }

        #endregion Public Constructors

        #region Public Properties

        public int Alpha
        {
            get => GetAlpha();
            set => SetAlpha(value);
        }

        public new Position3D Position
        {
            get => base.Position.ToPosition3D();
            set => base.Position = value.ToVector3();
        }

        public int Rotation
        {
            set => SetRotation(value);
        }

        public new EntityType Type => (EntityType)base.Type;

        #endregion Public Properties

        #region Public Methods

        public bool Equals(IEntity other)
        {
            return Handle == other?.Handle;
        }

        public void GetModelDimensions(Position3D a, Position3D b)
        {
            var aV = a.ToVector3();
            var bV = b.ToVector3();
            RAGE.Game.Misc.GetModelDimensions(Model, aV, bV);

            a.X = aV.X;
            a.Y = aV.Y;
            a.Z = aV.Z;
            b.X = bV.X;
            b.Y = bV.Y;
            b.Z = bV.Z;
        }

        public Position3D GetOffsetInWorldCoords(float offsetX, float offsetY, float offsetZ)
            => RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(Handle, offsetX, offsetY, offsetZ).ToPosition3D();

        #endregion Public Methods
    }
}
