using System;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Blip;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Entity
{
    class EntityBase : Entity, IEntityBase
    {
        private readonly RAGE.Elements.GameEntityBase _instance;

        public EntityBase(RAGE.Elements.GameEntityBase instance) : base(instance)
        {
            _instance = instance;
        }

        public Position3D Rotation
        {
            get => _instance.GetRotation(2).ToPosition3D();
            set => _instance.SetRotation(value.X, value.Y, value.Z, 2, true);
        }
        public int Alpha
        {
            get => _instance.GetAlpha();
            set => _instance.SetAlpha(value, false);
        }
        public float Heading
        {
            get => _instance.GetHeading();
            set => _instance.SetHeading(value);
        }

        public int Health 
        { 
            get => _instance.GetHealth(); 
            set => _instance.SetHealth(value); 
        }

        public void ActivatePhysics()
        {
            _instance.ActivatePhysics();
        }


        public bool Equals(IEntityBase other)
        {
            return Handle == other.Handle;
        }

        public void FreezePosition(bool freeze)
        {
            _instance.FreezePosition(freeze);
        }

        public void GetModelDimensions(Position3D a, Position3D b)
        {
            var aV = a.ToVector3();
            var bV = b.ToVector3();
            RAGE.Game.Misc.GetModelDimensions(_instance.Model, aV, bV);
            
            a.X = aV.X;
            a.Y = aV.Y;
            a.Z = aV.Z;
            b.X = bV.X;
            b.Y = bV.Y;
            b.Z = bV.Z;
        }

        public Position3D GetOffsetInWorldCoords(float offsetX, float offsetY, float offsetZ)
        {
            return RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(_instance.Handle, offsetX, offsetY, offsetZ).ToPosition3D();
        }

        /// <summary>
        /// Disabling the collision also disables raycasting.
        /// </summary>
        /// <param name="toggle"></param>
        /// <param name="keepPhysics"></param>
        public void SetCollision(bool toggle, bool keepPhysics)
        {
            RAGE.Game.Entity.SetEntityCollision(_instance.Handle, toggle, keepPhysics);
        }

        public void SetInvincible(bool toggle)
        {
            RAGE.Game.Entity.SetEntityInvincible(_instance.Handle, toggle);
        }

        public int AddBlipFor()
        {
            return _instance.AddBlipFor();
        }

        public int GetBlipFrom()
        {
            return RAGE.Game.Ui.GetBlipFromEntity(_instance.Handle);
        }

        public void ClearLastDamageEntity()
        {
            _instance.ClearLastDamageEntity();
        }

        public float GetAnimCurrentTime(string animDict, string animName)
        {
            return _instance.GetAnimCurrentTime(animDict, animName);
        }

        public bool HasAnimEventFired(uint actionHash)
        {
            return _instance.HasAnimEventFired(actionHash);
        }

        public bool IsPlayingAnim(string animDict, string animName)
        {
            return _instance.IsPlayingAnim(animDict, animName, 3);
        }

        public void ResetAlpha()
        {
            _instance.ResetAlpha();
        }

        public void SetVisible(bool toggle)
        {
            _instance.SetVisible(toggle, false);
        }

        public int AddIcon(string icon)
            => _instance.AddIcon(icon);

        public void ApplyForceTo(int forceFlags, float x, float y, float z, float offX, float offY, float offZ, int p8, bool isLocal, bool p10, bool isMassRel, bool p12, bool p13)
            => _instance.ApplyForceTo(forceFlags, x, y, z, offX, offY, offZ, p8, isLocal, p10, isMassRel, p12, p13);

        public void ApplyForceToCenterOfMass(int forceType, float x, float y, float z, bool p5, bool isRel, bool highForce, bool p8)
            => _instance.ApplyForceToCenterOfMass(forceType, x, y, z, p5, isRel, highForce, p8);

        public void AttachTvAudioTo()
            => _instance.AttachTvAudioTo(); 

        public bool CellCamIsCharVisibleNoFaceCheck()
            => _instance.CellCamIsCharVisibleNoFaceCheck();

        public void ClearLastWeaponDamage()
            => _instance.ClearLastWeaponDamage();

        public void ClearRoomFor()
            => _instance.ClearRoomFor();

        public bool DecorExistOn(string propertyName)
            => _instance.DecorExistOn(propertyName);

        public bool DecorGetBool(string propertyName)
            => _instance.DecorGetBool(propertyName);

        public float DecorGetFloat(string propertyName)
            => _instance.DecorGetFloat(propertyName);

        public int DecorGetInt(string propertyName)
            => _instance.DecorGetInt(propertyName);

        public bool DecorRemove(string propertyName)
            => _instance.DecorRemove(propertyName);

        public bool DecorSetBool(string propertyName, bool value)
            => _instance.DecorSetBool(propertyName, value);

        public bool DecorSetFloat(string propertyName, float value)
            => _instance.DecorSetFloat(propertyName, value);

        public bool DecorSetInt(string propertyName, int value)
            => _instance.DecorSetInt(propertyName, value);

        public bool DecorSetTime(string propertyName, int timestamp)
            => _instance.DecorSetTime(propertyName, timestamp);

        public void Detach(bool p1, bool collision)
            => _instance.Detach(p1, collision);

        public bool DoesBelongToThisScript(bool p1)
            => _instance.DoesBelongToThisScript(p1);

        public bool DoesExist()
            => _instance.DoesExist();

        public bool DoesHaveDrawable()
            => _instance.DoesHaveDrawable();

        public bool DoesHavePhysics()
            => _instance.DoesHavePhysics();

        public void ForceAiAndAnimationUpdate()
            => _instance.ForceAiAndAnimationUpdate();

        public void ForceRoomFor(int interiorID, uint roomHashKey)
            => _instance.ForceRoomFor(interiorID, roomHashKey);

        public int GetAlpha()
            => _instance.GetAlpha();

        public float GetAnimTotalTime(string animDict, string animName)
            => _instance.GetAnimTotalTime(animDict, animName);

        public int GetAttachedTo()
            => _instance.GetAttachedTo();

        public int GetBoneIndexByName(string boneName)
            => _instance.GetBoneIndexByName(boneName);

        public bool GetCollisionDisabled()
            => _instance.GetCollisionDisabled();

        public Position3D GetCollisionNormalOfLastHitFor()
            => _instance.GetCollisionNormalOfLastHitFor().ToPosition3D();

        public Position3D GetCoords(bool alive)
            => _instance.GetCoords(alive).ToPosition3D();

        public Position3D GetForwardVector()
            => _instance.GetForwardVector().ToPosition3D();

        public float GetForwardX()
            => _instance.GetForwardX();

        public float GetForwardY()
            => _instance.GetForwardY();

        public uint GetHashNameForComponent(int componentId, int drawableVariant, int textureVariant)
            => _instance.GetHashNameForComponent(componentId, drawableVariant, textureVariant);

        public uint GetHashNameForProp(int componentId, int propIndex, int propTextureIndex)
            => _instance.GetHashNameForProp(componentId, propIndex, propTextureIndex);

        public float GetHeading()
            => _instance.GetHeading();

        public int GetHealth()
            => _instance.GetHealth();

        public float GetHeight(float X, float Y, float Z, bool atTop, bool inWorldCoords)
            => _instance.GetHeight(X, Y, Z, atTop, inWorldCoords);

        public float GetHeightAboveGround()
            => _instance.GetHeightAboveGround();

        public int GetInteriorFrom()
            => _instance.GetInteriorFrom();

        public uint GetKeyForInRoom()
            => _instance.GetKeyForInRoom();

        public uint GetLastMaterialHitBy()
            => _instance.GetLastMaterialHitBy();

        public int GetLodDist()
            => _instance.GetLodDist();

        public void GetMatrix(Position3D rightVector, Position3D forwardVector, Position3D upVector, Position3D position)
        {
            var right = rightVector.ToVector3();
            var forward = forwardVector.ToVector3();
            var up = upVector.ToVector3();
            var pos = position.ToVector3();
            _instance.GetMatrix(right, forward, up, pos);

            rightVector.CopyValuesFrom(right);
            forwardVector.CopyValuesFrom(forward);
            upVector.CopyValuesFrom(up);
            position.CopyValuesFrom(pos);
        }

        public int GetMaxHealth()
            => _instance.GetMaxHealth();

        public uint GetModel()
            => _instance.GetModel();

        public int GetNearestPlayerTo()
            => _instance.GetNearestPlayerTo();

        public int GetNearestPlayerToOnTeam(int team)
            => _instance.GetNearestPlayerToOnTeam(team);

        public int GetObjectIndexFromIndex()
            => _instance.GetObjectIndexFromIndex();

        public Position3D GetOffsetFromGivenWorldCoords(float posX, float posY, float posZ)
            => _instance.GetOffsetFromGivenWorldCoords(posX, posY, posZ).ToPosition3D();

        public Position3D GetOffsetFromInWorldCoords(float offsetX, float offsetY, float offsetZ)
            => _instance.GetOffsetFromInWorldCoords(offsetX, offsetY, offsetZ).ToPosition3D();

        public int GetPedIndexFromIndex()
            => _instance.GetPedIndexFromIndex();

        public float GetPhysicsHeading()
            => _instance.GetPhysicsHeading();

        public float GetPitch()
            => _instance.GetPitch();

        public int GetPopulationType()
            => _instance.GetPopulationType();

        public void GetQuaternion(ref float x, ref float y, ref float z, ref float w)
            => _instance.GetQuaternion(ref x, ref y, ref z, ref w);

        public float GetRoll()
            => _instance.GetRoll();

        public uint GetRoomKeyFrom()
            => _instance.GetRoomKeyFrom();

        public Position3D GetRotation(int rotationOrder)
            => _instance.GetRotation(rotationOrder).ToPosition3D();

        public Position3D GetRotationVelocity()
            => _instance.GetRotationVelocity().ToPosition3D();

        public string GetScript(ref int script)
            => _instance.GetScript(ref script);

        public float GetSpeed()
            => _instance.GetSpeed();

        public Position3D GetSpeedVector(bool relative)
            => _instance.GetSpeedVector(relative).ToPosition3D();

        public float GetSubmergedLevel()
            => _instance.GetSubmergedLevel();

        public new int GetType()
            => _instance.GetType();

        public float GetUprightValue()
            => _instance.GetUprightValue();

        public int GetVehicleIndexFromIndex()
            => _instance.GetVehicleIndexFromIndex();

        public Position3D GetVelocity()
            => _instance.GetVelocity().ToPosition3D();

        public Position3D GetWorldPositionOfBone(int boneIndex)
            => _instance.GetWorldPositionOfBone(boneIndex).ToPosition3D();

        public bool HasAnimFinished(string animDict, string animName, int p3)
            => _instance.HasAnimFinished(animDict, animName, p3);

        public bool HasBeenDamagedByAnyObject()
            => _instance.HasBeenDamagedByAnyObject();

        public bool HasBeenDamagedByAnyPed()
            => _instance.HasBeenDamagedByAnyPed();

        public bool HasBeenDamagedByAnyVehicle()
            => _instance.HasBeenDamagedByAnyVehicle();

        public bool HasBeenDamagedByWeapon(uint weaponHash, int weaponType)
            => _instance.HasBeenDamagedByWeapon(weaponHash, weaponType);

        public bool HasCollidedWithAnything()
            => _instance.HasCollidedWithAnything();

        public bool HasCollisionLoadedAround()
            => _instance.HasCollisionLoadedAround();

        public bool IsAMissionEntity()
            => _instance.IsAMissionEntity();

        public bool IsAnObject()
            => _instance.IsAnObject();

        public bool IsAPed()
            => _instance.IsAPed();

        public bool IsAtCoord(float xPos, float yPos, float zPos, float xSize, float ySize, float zSize, bool p7, bool p8, int p9)
            => _instance.IsAtCoord(xPos, yPos, zPos, xSize, ySize, zSize, p7, p8, p9);

        public bool IsAtEntity(int entity2, float xSize, float ySize, float zSize, bool p5, bool p6, int p7)
            => _instance.IsAtEntity(entity2, xSize, ySize, zSize, p5, p6, p7);

        public bool IsAttached()
            => _instance.IsAttached();

        public bool IsAttachedToAnyObject()
            => _instance.IsAttachedToAnyObject();

        public bool IsAttachedToAnyPed()
            => _instance.IsAttachedToAnyPed();

        public bool IsAttachedToAnyVehicle()
            => _instance.IsAttachedToAnyVehicle();

        public bool IsAttachedToEntity(int to)
            => _instance.IsAttachedToEntity(to);

        public bool IsAVehicle()
            => _instance.IsAVehicle();

        public bool IsDead(int p1)
            => _instance.IsDead(p1);

        public bool IsFocus()
            => _instance.IsFocus();

        public bool IsInAir()
            => _instance.IsInAir();

        public bool IsInAngledArea(float originX, float originY, float originZ, float edgeX, float edgeY, float edgeZ, float angle, bool p8, bool p9, int p10)
            => _instance.IsInAngledArea(originX, originY, originZ, edgeX, edgeY, edgeZ, angle, p8, p9, p10);

        public bool IsInArea(float x1, float y1, float z1, float x2, float y2, float z2, bool p7, bool p8, int p9)
            => _instance.IsInArea(x1, y1, z1, x2, y2, z2, p7, p8, p9);

        public bool IsInWater()
            => _instance.IsInWater();

        public bool IsInZone(string zone)
            => _instance.IsInZone(zone);

        public bool IsOccluded()
            => _instance.IsOccluded();

        public bool IsOnFire()
            => _instance.IsOnFire();

        public bool IsOnScreen()
            => _instance.IsOnScreen();

        public bool IsStatic()
            => _instance.IsStatic();

        public bool IsTouchingEntity(int targetEntity)
            => _instance.IsTouchingEntity(targetEntity);

        public bool IsTouchingModel(uint modelHash)
            => _instance.IsTouchingModel(modelHash);

        public bool IsUpright(float angle)
            => _instance.IsUpright(angle);

        public bool IsUpsidedown()
            => _instance.IsUpsidedown();

        public bool IsVisible()
            => _instance.IsVisible();

        public bool IsVisibleToScript()
            => _instance.IsVisibleToScript();

        public bool IsWaitingForWorldCollision()
            => _instance.IsWaitingForWorldCollision();

        public void NetworkAddToSynchronisedScene(int netScene, string animDict, string animName, float speed, float speedMulitiplier, int flag)
            => _instance.NetworkAddToSynchronisedScene(netScene, animDict, animName, speed, speedMulitiplier, flag);

        public bool NetworkDoesExistWithNetworkId()
            => _instance.NetworkDoesExistWithNetworkId();

        public void NetworkFadeIn(bool state, int p2)
            => _instance.NetworkFadeIn(state, p2);

        public void NetworkFadeOut(bool normal, bool slow)
            => _instance.NetworkFadeOut(normal, slow);

        public bool NetworkGetIsLocal()
            => _instance.NetworkGetIsLocal();

        public bool NetworkGetIsNetworked()
            => _instance.NetworkGetIsNetworked();

        public int NetworkGetNetworkIdFrom()
            => _instance.NetworkGetNetworkIdFrom();

        public bool NetworkHasControlOf()
            => _instance.NetworkHasControlOf();

        public void NetworkRegisterAsNetworked()
            => _instance.NetworkRegisterAsNetworked();

        public bool NetworkRequestControlOf()
            => _instance.NetworkRequestControlOf();

        public void NetworkSetCanBlend(bool toggle)
            => _instance.NetworkSetCanBlend(toggle);

        public void NetworkSetVisibleToNetwork(bool toggle)
            => _instance.NetworkSetVisibleToNetwork(toggle);

        public void NetworkUnregisterNetworked()
            => _instance.NetworkUnregisterNetworked();

        public bool PlayAnim(string animName, string animDict, float p3, bool loop, bool stayInAnim, bool p6, float delta, int bitset)
            => _instance.PlayAnim(animName, animDict, p3, loop, stayInAnim, p6, delta, bitset);

        public bool PlaySynchronizedAnim(int syncedScene, string animation, string propName, float p4, float p5, int p6, float p7)
            => _instance.PlaySynchronizedAnim(syncedScene, animation, propName, p4, p5, p6, p7);

        public void ProcessAttachments()
            => _instance.ProcessAttachments();

        public void RemoveParticleFxFrom()
            => _instance.RemoveParticleFxFrom();

        public void SetAlpha(int alphaLevel, bool skin)
            => _instance.SetAlpha(alphaLevel, skin);

        public void SetAlwaysPrerender(bool toggle)
            => _instance.SetAlwaysPrerender(toggle);

        public void SetAnimCurrentTime(string animDictionary, string animName, float time)
            => _instance.SetAnimCurrentTime(animDictionary, animName, time);

        public void SetAnimSpeed(string animDictionary, string animName, float speedMultiplier)
            => _instance.SetAnimSpeed(animDictionary, animName, speedMultiplier);

        public void SetAsMissionEntity(bool p1, bool p2)
            => _instance.SetAsMissionEntity(p1, p2);

        public void SetCanBeDamaged(bool toggle)
            => _instance.SetCanBeDamaged(toggle);

        public void SetCanBeDamagedByRelationshipGroup(bool bCanBeDamaged, int relGroup)
            => _instance.SetCanBeDamagedByRelationshipGroup(bCanBeDamaged, relGroup);

        public void SetCanBeTargetedWithoutLos(bool toggle)
            => _instance.SetCanBeTargetedWithoutLos(toggle);

        public void SetCoords(float xPos, float yPos, float zPos, bool xAxis, bool yAxis, bool zAxis, bool clearArea)
            => _instance.SetCoords(xPos, yPos, zPos, xAxis, yAxis, zAxis, clearArea);

        public void SetCoords2(float xPos, float yPos, float zPos, bool xAxis, bool yAxis, bool zAxis, bool clearArea)
            => _instance.SetCoords2(xPos, yPos, zPos, xAxis, yAxis, zAxis, clearArea);

        public void SetCoordsNoOffset(float xPos, float yPos, float zPos, bool xAxis, bool yAxis, bool zAxis)
            => _instance.SetCoordsNoOffset(xPos, yPos, zPos, xAxis, yAxis, zAxis);

        public void SetDynamic(bool toggle)
            => _instance.SetDynamic(toggle);

        public void SetFocus()
            => _instance.SetFocus();

        public void SetGameplayHint(float xOffset, float yOffset, float zOffset, bool p4, int p5, int p6, int p7, int p8)
            => _instance.SetGameplayHint(xOffset, yOffset, zOffset, p4, p5, p6, p7, p8);

        public void SetHasGravity(bool toggle)
            => _instance.SetHasGravity(toggle);

        public void SetHeading(float heading)
            => _instance.SetHeading(heading);

        public void SetHealth(int health)
            => _instance.SetHealth(health);

        public void SetIconColor(int red, int green, int blue, int alpha)
            => _instance.SetIconColor(red, green, blue, alpha);

        public void SetIconVisibility(bool toggle)
            => _instance.SetIconVisibility(toggle);

        public void SetIsTargetPriority(bool p1, float p2)
            => _instance.SetIsTargetPriority(p1, p2);

        public void SetLights(bool toggle)
            => _instance.SetLights(toggle);

        public void SetLoadCollisionFlag(bool toggle, int p2)
            => _instance.SetLoadCollisionFlag(toggle, p2);

        public void SetLocallyInvisible()
            => _instance.SetLocallyInvisible();

        public void SetLocallyVisible()
            => _instance.SetLocallyVisible();

        public void SetLodDist(int value)
            => _instance.SetLodDist(value);

        public void SetMaxHealth(int value)
            => _instance.SetMaxHealth(value);

        public void SetMaxSpeed(float speed)
            => _instance.SetMaxSpeed(speed);

        public void SetMotionBlur(bool toggle)
            => _instance.SetMotionBlur(toggle);

        public void SetNoCollisionEntity(int entity2)
            => _instance.SetNoCollisionEntity(entity2, true);

        public void SetOnlyDamagedByPlayer(bool toggle)
            => _instance.SetOnlyDamagedByPlayer(toggle);

        public void SetOnlyDamagedByRelationshipGroup(bool p1, int p2)
            => _instance.SetOnlyDamagedByRelationshipGroup(p1, p2);

        public void SetProofs(bool bulletProof, bool fireProof, bool explosionProof, bool collisionProof, bool meleeProof, bool p6, bool p7, bool drownProof)
            => _instance.SetProofs(bulletProof, fireProof, explosionProof, collisionProof, meleeProof, p6, p7, drownProof);

        public void SetQuaternion(float x, float y, float z, float w)
            => _instance.SetQuaternion(x, y, z, w);

        public void SetRecordsCollisions(bool toggle)
            => _instance.SetRecordsCollisions(toggle);

        public void SetRenderScorched(bool toggle)
            => _instance.SetRenderScorched(toggle);

        public void SetRotation(float pitch, float roll, float yaw, int rotationOrder, bool p5)
            => _instance.SetRotation(pitch, roll, yaw, rotationOrder, p5);

        public void SetSomething(bool toggle)
            => _instance.SetSomething(toggle);

        public void SetTrafficlightOverride(int state)
            => _instance.SetTrafficlightOverride(state);

        public void SetVelocity(float x, float y, float z)
            => _instance.SetVelocity(x, y, z);

        public void SetVisibleInCutscene(bool p1, bool p2)
            => _instance.SetVisibleInCutscene(p1, p2);

        public int StartFire()
            => _instance.StartFire();

        public int StartShapeTestBound(int flags1, int flags2)
            => _instance.StartShapeTestBound(flags1, flags2);

        public int StartShapeTestBoundingBox(int flags1, int flags2)
            => _instance.StartShapeTestBoundingBox(flags1, flags2);

        public int StopAnim(string animation, string animGroup, float p3)
            => _instance.StopAnim(animation, animGroup, p3);

        public void StopFire()
            => _instance.StopFire();

        public bool StopSynchronizedAnim(float p1, bool p2)
            => _instance.StopSynchronizedAnim(p1, p2);
    }
}
