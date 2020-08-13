using RAGE;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.MapObject;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.MapObject
{
    internal class MapObject : RAGE.Elements.MapObject, IMapObject
    {
        #region Public Constructors

        public MapObject(ushort id, ushort remoteId) : base(id, remoteId)
        { }

        public MapObject(uint hash, Vector3 position, Vector3 rotation, int alpha = 255, uint dimension = 0)
            : base(hash, position, rotation, alpha, dimension)
        { }

        #endregion Public Constructors

        #region Public Properties

        public int Alpha
        {
            get => GetAlpha();
            set => SetAlpha(value, false);
        }

        public float Heading
        {
            get => GetHeading();
            set => SetHeading(value);
        }

        public int Health
        {
            get => GetHealth();
            set => SetHealth(value);
        }

        public new Position Position
        {
            get => base.Position.ToPosition();
            set => base.Position = value.ToVector3();
        }

        public Position Rotation
        {
            get => GetRotation(2);
            set => SetRotation(value.X, value.Y, value.Z, 2, true);
        }

        public new EntityType Type => (EntityType)base.Type;

        #endregion Public Properties

        #region Public Methods

        public bool Equals(IEntity other)
        {
            return Id == other?.Id;
        }

        public new Position GetCollisionNormalOfLastHitFor()
            => base.GetCollisionNormalOfLastHitFor().ToPosition();

        public new Position GetCoords(bool alive)
            => base.GetCoords(alive).ToPosition();

        public new Position GetForwardVector()
            => base.GetForwardVector().ToPosition();

        public void GetMatrix(Position rightVector, Position forwardVector, Position upVector, Position position)
        {
            var right = rightVector.ToVector3();
            var forward = forwardVector.ToVector3();
            var up = upVector.ToVector3();
            var pos = position.ToVector3();
            base.GetMatrix(right, forward, up, pos);

            rightVector.CopyValuesFrom(right);
            forwardVector.CopyValuesFrom(forward);
            upVector.CopyValuesFrom(up);
            position.CopyValuesFrom(pos);
        }

        public void GetModelDimensions(Position a, Position b)
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

        public new Position GetOffsetFromGivenWorldCoords(float posX, float posY, float posZ)
            => base.GetOffsetFromGivenWorldCoords(posX, posY, posZ).ToPosition();

        public new Position GetOffsetFromInWorldCoords(float offsetX, float offsetY, float offsetZ)
            => base.GetOffsetFromInWorldCoords(offsetX, offsetY, offsetZ).ToPosition();

        public Position GetOffsetInWorldCoords(float offsetX, float offsetY, float offsetZ)
            => RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(Handle, offsetX, offsetY, offsetZ).ToPosition();

        public new Position GetRotation(int rotationOrder)
            => base.GetRotation(rotationOrder).ToPosition();

        public new Position GetRotationVelocity()
            => base.GetRotationVelocity().ToPosition();

        public new Position GetSpeedVector(bool relative)
            => base.GetSpeedVector(relative).ToPosition();

        public new Position GetVelocity()
            => base.GetVelocity().ToPosition();

        public new Position GetWorldPositionOfBone(int boneIndex)
            => base.GetWorldPositionOfBone(boneIndex).ToPosition();

        public bool IsPlayingAnim(string animDict, string animName)
            => IsPlayingAnim(animDict, animName, 3);

        public void SetNoCollisionEntity(int entity2)
            => SetNoCollisionEntity(entity2, true);

        public void SetVisible(bool toggle)
            => SetVisible(toggle, false);

        #endregion Public Methods
    }
}
