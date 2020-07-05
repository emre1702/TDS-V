using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Ped
{
    internal class Ped : RAGE.Elements.Ped, IPed
    {
        #region Public Constructors

        public Ped(ushort id, ushort remoteId) : base(id, remoteId)
        { }

        public Ped(uint model, RAGE.Vector3 position, float heading = 0, uint dimension = 0)
            : base(model, position, heading, dimension) { }

        #endregion Public Constructors

        #region Public Properties

        public int Alpha
        {
            get => GetAlpha();
            set => SetAlpha(value, false);
        }

        public int Armor
        {
            get => GetArmour();
            set => SetArmour(value);
        }

        //=> (_instance, _entityConvertingHandler) = (instance, entityConvertingHandler);
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
            get => RAGE.Game.Entity.GetEntityCoords(Handle, true).ToPosition3D();
            set => RAGE.Game.Entity.SetEntityCoordsNoOffset(Handle, value.X, value.Y, value.Z, true, true, true);
        }

        public Position3D Rotation
        {
            get => GetRotation(2);
            set => SetRotation(value.X, value.Y, value.Z, 2, true);
        }

        public new EntityType Type => (EntityType)base.Type;

        #endregion Public Properties

        #region Public Methods

        public void AddAmmoTo(WeaponHash weaponHash, int ammo)
            => AddAmmoTo((uint)weaponHash, ammo);

        public void AddOwnedExplosion(float x, float y, float z, ExplosionType explosionType, float damageScale, bool isAudible, bool isInvisible, float cameraShake)
            => AddOwnedExplosion(x, y, z, (int)explosionType, damageScale, isAudible, isInvisible, cameraShake);

        public bool Equals(IEntity other)
        {
            return Id == other?.Id;
        }

        public void ExplodeHead(WeaponHash weaponHash)
            => ExplodeHead((uint)weaponHash);

        public void ExplodeProjectiles(WeaponHash weaponHash, bool p2)
            => ExplodeProjectiles((uint)weaponHash, p2);

        public bool ForceMotionState(MotionState motionState, bool p2, bool p3, bool p4)
            => ForceMotionState((uint)motionState, p2, p3, p4);

        public int GetAmmoInClip(WeaponHash weaponHash)
        {
            int ammoInClip = 0;
            GetAmmoInClip((uint)weaponHash, ref ammoInClip);
            return ammoInClip;
        }

        public int GetAmmoInWeapon(WeaponHash weaponHash)
            => GetAmmoInWeapon((uint)weaponHash);

        public uint GetAmmoTypeFromWeapon(WeaponHash weaponHash)
            => GetAmmoTypeFromWeapon((uint)weaponHash);

        public uint GetAmmoTypeFromWeapon2(WeaponHash weaponHash)
            => GetAmmoTypeFromWeapon2((uint)weaponHash);

        public Position3D GetBoneCoords(PedBone boneId, float offsetX, float offsetY, float offsetZ)
            => GetBoneCoords((int)boneId, offsetX, offsetY, offsetZ).ToPosition3D();

        public int GetBoneIndex(PedBone boneId)
            => GetBoneIndex((int)boneId);

        public new Position3D GetCollisionNormalOfLastHitFor()
            => base.GetCollisionNormalOfLastHitFor().ToPosition3D();

        public new Position3D GetCoords(bool alive)
            => base.GetCoords(alive).ToPosition3D();

        public int? GetCurrentVehicleWeapon()
        {
            int weaponHashNumber = 0;
            if (!GetCurrentVehicleWeapon(ref weaponHashNumber))
                return null;
            return weaponHashNumber;
        }

        public WeaponHash? GetCurrentWeapon(bool p2)
        {
            int weaponHashNumber = 0;
            if (!GetCurrentWeapon(ref weaponHashNumber, p2))
                return null;
            return (WeaponHash)weaponHashNumber;
        }

        public new Position3D GetDeadPickupCoords(float p1, float p2)
            => base.GetDeadPickupCoords(p1, p2).ToPosition3D();

        public new Position3D GetDefensiveAreaPosition(bool p1)
            => base.GetDefensiveAreaPosition(p1).ToPosition3D();

        public new Position3D GetExtractedDisplacement(bool worldSpace)
            => base.GetExtractedDisplacement(worldSpace).ToPosition3D();

        public new Position3D GetForwardVector()
            => base.GetForwardVector().ToPosition3D();

        public int? GetHeadBlendData()
        {
            int headBlendData = 0;
            if (!GetHeadBlendData(ref headBlendData))
                return null;
            return headBlendData;
        }

        public PedBone? GetLastDamageBone()
        {
            int outBone = 0;
            if (!GetLastDamageBone(ref outBone))
                return null;

            return (PedBone)outBone;
        }

        public Position3D GetLastWeaponImpactCoord()
        {
            RAGE.Vector3 coords = new RAGE.Vector3();
            if (!GetLastWeaponImpactCoord(coords))
                return null;
            return coords?.ToPosition3D();
        }

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

        public int? GetMaxAmmo(WeaponHash weaponHash)
        {
            int ammo = 0;
            if (!GetMaxAmmo((uint)weaponHash, ref ammo))
                return null;
            return ammo;
        }

        public int GetMaxAmmoInClip(WeaponHash weaponHash, bool p2)
            => GetMaxAmmoInClip((uint)weaponHash, p2);

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

        public int GetParachuteTintIndex()
        {
            int tintIndex = 0;
            GetParachuteTintIndex(ref tintIndex);
            return tintIndex;
        }

        public int GetRagdollBoneIndex(PedBone bone)
            => GetRagdollBoneIndex((int)bone);

        public new Position3D GetRotation(int rotationOrder)
            => base.GetRotation(rotationOrder).ToPosition3D();

        public new Position3D GetRotationVelocity()
            => base.GetRotationVelocity().ToPosition3D();

        public new WeaponHash GetSelectedWeapon()
            => (WeaponHash)base.GetSelectedWeapon();

        public new Position3D GetSpeedVector(bool relative)
            => base.GetSpeedVector(relative).ToPosition3D();

        public new Position3D GetVelocity()
            => base.GetVelocity().ToPosition3D();

        public int GetWeaponTintIndex(WeaponHash weaponHash)
            => GetWeaponTintIndex((uint)weaponHash);

        public new Position3D GetWorldPositionOfBone(int boneIndex)
            => base.GetWorldPositionOfBone(boneIndex).ToPosition3D();

        public void GiveDelayedWeaponTo(WeaponHash weaponHash, int time, bool equipNow)
            => GiveDelayedWeaponTo((uint)weaponHash, time, equipNow);

        public void GiveWeaponComponentTo(WeaponHash weaponHash, uint componentHash)
            => GiveWeaponComponentTo((uint)weaponHash, componentHash);

        public void GiveWeaponTo(WeaponHash weaponHash, int ammoCount, bool isHidden, bool equipNow)
            => GiveWeaponTo((uint)weaponHash, ammoCount, isHidden, equipNow);

        public bool HasBeenDamagedByWeapon(WeaponHash weaponHash, int weaponType)
            => HasBeenDamagedByWeapon((uint)weaponHash, weaponType);

        public bool HasGotWeapon(WeaponHash weaponHash, bool p2)
            => HasGotWeapon((uint)weaponHash, p2);

        public bool HasGotWeaponComponent(WeaponHash weaponHash, uint componentHash)
            => HasGotWeaponComponent((uint)weaponHash, componentHash);

        public bool IsArmed(ArmedType type)
            => IsArmed((int)type);

        public bool IsDeadOrDying()
                    => IsDeadOrDying(true);

        public bool IsHeadshotReady()
            => IsheadshotReady();

        public bool IsHeadshotValid()
            => IsheadshotValid();

        public bool IsPlayingAnim(string animDict, string animName)
            => IsPlayingAnim(animDict, animName, 3);

        public bool IsWeaponComponentActive(WeaponHash weaponHash, uint componentHash)
            => IsWeaponComponentActive((uint)weaponHash, componentHash);

        public int RegisterHeadshot()
            => Registerheadshot();

        public void RemoveWeaponComponentFrom(WeaponHash weaponHash, uint componentHash)
            => RemoveWeaponComponentFrom((uint)weaponHash, componentHash);

        public void RemoveWeaponFrom(WeaponHash weaponHash)
            => RemoveWeaponFrom((uint)weaponHash);

        public void SetAmmo(WeaponHash weaponHash, int ammo, int p3)
           => SetAmmo((uint)weaponHash, ammo, p3);

        public bool SetAmmoInClip(WeaponHash weaponHash, int ammo)
            => SetAmmoInClip((uint)weaponHash, ammo);

        public void SetCanAttackFriendly(bool toggle)
            => SetCanAttackFriendly(toggle, false);

        public bool SetCurrentVehicleWeapon(WeaponHash weaponHash)
            => SetCurrentVehicleWeapon((uint)weaponHash);

        public void SetCurrentWeapon(WeaponHash weaponHash, bool equipNow)
           => SetCurrentWeapon((uint)weaponHash, equipNow);

        public void SetDropsInventoryWeapon(WeaponHash weaponHash, float xOffset, float yOffset, float zOffset, int p5)
            => SetDropsInventoryWeapon((uint)weaponHash, xOffset, yOffset, zOffset, p5);

        public void SetHeadBlendPaletteColor(Color color, int type)
            => RAGE.Game.Invoker.Invoke((ulong)NativeHash.SET_HEAD_BLEND_PALETTE_COLOR, Handle, (int)color.R, (int)color.G, (int)color.B, type);

        public void SetHeadBlendPaletteColor(ColorDto color, int type)
            => RAGE.Game.Invoker.Invoke((ulong)NativeHash.SET_HEAD_BLEND_PALETTE_COLOR, Handle, (int)color.R, (int)color.G, (int)color.B, type);

        public void SetInfiniteAmmo(bool toggle, WeaponHash weaponHash)
            => SetInfiniteAmmo(toggle, (uint)weaponHash);

        public void SetIntoVehicle(int vehicle, VehicleSeat seatIndex)
            => SetIntoVehicle(vehicle, (int)seatIndex - 1);

        public void SetNoCollisionEntity(int entity2)
            => SetNoCollisionEntity(entity2, true);

        public void SetVisible(bool toggle)
            => SetVisible(toggle, false);

        public void SetWeaponTintIndex(WeaponHash weaponHash, int tintIndex)
            => SetWeaponTintIndex((uint)weaponHash, tintIndex);

        public void TaskForceMotionState(MotionState state, bool p2)
            => TaskForceMotionState((uint)state, p2);

        #endregion Public Methods
    }
}
