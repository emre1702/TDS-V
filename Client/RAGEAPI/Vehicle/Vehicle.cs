using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Vehicle
{
    internal class Vehicle : RAGE.Elements.Vehicle, IVehicle
    {
        #region Public Constructors

        public Vehicle(ushort id, ushort remoteId) : base(id, remoteId)
        { }

        #endregion Public Constructors

        #region Public Properties

        public int Alpha
        {
            get => GetAlpha();
            set => SetAlpha(value, false);
        }

        public new IPlayer Controller => base.Controller as IPlayer;

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

        public new Position3D Position
        {
            get => base.Position.ToPosition3D();
            set => base.Position = value.ToVector3();
        }

        public Position3D Rotation
        {
            get => GetRotation(2);
            set => SetRotation(value.X, value.Y, value.Z, 2, true);
        }

        public new EntityType Type => (EntityType)base.Type;

        #endregion Public Properties

        #region Public Methods

        public bool Equals(IEntity other)
        {
            return Handle == other?.Handle;
        }

        public new Position3D GetCollisionNormalOfLastHitFor()
            => base.GetCollisionNormalOfLastHitFor().ToPosition3D();

        public new Position3D GetCoords(bool alive)
            => base.GetCoords(alive).ToPosition3D();

        public new Position3D GetDeformationAtPos(float offsetX, float offsetY, float offsetZ)
                            => base.GetDeformationAtPos(offsetX, offsetY, offsetZ).ToPosition3D();

        public new Position3D GetEntryPositionOfDoor(int doorIndex)
            => base.GetEntryPositionOfDoor(doorIndex).ToPosition3D();

        public new Position3D GetForwardVector()
            => base.GetForwardVector().ToPosition3D();

        public void GetMatrix(Position3D rightVector, Position3D forwardVector, Position3D upVector, Position3D position)
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

        public new Position3D GetOffsetFromGivenWorldCoords(float posX, float posY, float posZ)
            => base.GetOffsetFromGivenWorldCoords(posX, posY, posZ).ToPosition3D();

        public new Position3D GetOffsetFromInWorldCoords(float offsetX, float offsetY, float offsetZ)
            => base.GetOffsetFromInWorldCoords(offsetX, offsetY, offsetZ).ToPosition3D();

        public Position3D GetOffsetInWorldCoords(float offsetX, float offsetY, float offsetZ)
            => RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(Handle, offsetX, offsetY, offsetZ).ToPosition3D();

        public new Position3D GetRotation(int rotationOrder)
            => base.GetRotation(rotationOrder).ToPosition3D();

        public new Position3D GetRotationVelocity()
            => base.GetRotationVelocity().ToPosition3D();

        public new Position3D GetSpeedVector(bool relative)
            => base.GetSpeedVector(relative).ToPosition3D();

        public new Position3D GetVelocity()
            => base.GetVelocity().ToPosition3D();

        public new Position3D GetWorldPositionOfBone(int boneIndex)
            => base.GetWorldPositionOfBone(boneIndex).ToPosition3D();

        public bool IsPlayingAnim(string animDict, string animName)
            => IsPlayingAnim(animDict, animName, 3);

        public bool IsSeatFree(VehicleSeat seat)
                                                                                                            => IsSeatFree((int)seat - 1, 1);

        public void SetNoCollisionEntity(int entity2)
            => SetNoCollisionEntity(entity2, true);

        public void SetVisible(bool toggle)
                    => SetVisible(toggle, false);

        #endregion Public Methods
    }
}
