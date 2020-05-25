﻿using System;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Entity
{
    public interface IEntityBase : IEntity
    {
        #region Public Properties

        int Alpha { get; set; }

        float Heading { get; set; }

        int Health { get; set; }

        Position3D Rotation { get; set; }

        #endregion Public Properties

        #region Public Methods

        void ActivatePhysics();

        int AddBlipFor();

        int AddIcon(string icon);

        void ApplyForceTo(int forceFlags, float x, float y, float z, float offX, float offY, float offZ, int p8, bool isLocal, bool p10, bool isMassRel, bool p12, bool p13);

        void ApplyForceToCenterOfMass(int forceType, float x, float y, float z, bool p5, bool isRel, bool highForce, bool p8);

        void AttachTvAudioTo();

        bool CellCamIsCharVisibleNoFaceCheck();

        void ClearLastDamageEntity();

        void ClearLastWeaponDamage();

        void ClearRoomFor();

        bool DecorExistOn(string propertyName);

        bool DecorGetBool(string propertyName);

        float DecorGetFloat(string propertyName);

        int DecorGetInt(string propertyName);

        bool DecorRemove(string propertyName);

        bool DecorSetBool(string propertyName, bool value);

        bool DecorSetFloat(string propertyName, float value);

        bool DecorSetInt(string propertyName, int value);

        bool DecorSetTime(string propertyName, int timestamp);

        void Detach(bool p1, bool collision);

        bool DoesBelongToThisScript(bool p1);

        bool DoesExist();

        bool DoesHaveDrawable();

        bool DoesHavePhysics();

        void ForceAiAndAnimationUpdate();

        void ForceRoomFor(int interiorID, uint roomHashKey);

        void FreezePosition(bool toggle);

        int GetAlpha();

        float GetAnimCurrentTime(string animDict, string animName);

        float GetAnimTotalTime(string animDict, string animName);

        int GetAttachedTo();

        int GetBlipFrom();

        int GetBoneIndexByName(string boneName);

        bool GetCollisionDisabled();

        Position3D GetCollisionNormalOfLastHitFor();

        Position3D GetCoords(bool alive);

        Position3D GetForwardVector();

        float GetForwardX();

        float GetForwardY();

        uint GetHashNameForComponent(int componentId, int drawableVariant, int textureVariant);

        uint GetHashNameForProp(int componentId, int propIndex, int propTextureIndex);

        float GetHeading();

        int GetHealth();

        float GetHeight(float X, float Y, float Z, bool atTop, bool inWorldCoords);

        float GetHeightAboveGround();

        int GetInteriorFrom();

        uint GetKeyForInRoom();

        uint GetLastMaterialHitBy();

        int GetLodDist();

        void GetMatrix(Position3D rightVector, Position3D forwardVector, Position3D upVector, Position3D position);

        int GetMaxHealth();

        uint GetModel();

        void GetModelDimensions(Position3D a, Position3D b);

        int GetNearestPlayerTo();

        int GetNearestPlayerToOnTeam(int team);

        int GetObjectIndexFromIndex();

        Position3D GetOffsetFromGivenWorldCoords(float posX, float posY, float posZ);

        Position3D GetOffsetFromInWorldCoords(float offsetX, float offsetY, float offsetZ);

        Position3D GetOffsetInWorldCoords(float offsetX, float offsetY, float offsetZ);

        int GetPedIndexFromIndex();

        float GetPhysicsHeading();

        float GetPitch();

        int GetPopulationType();

        void GetQuaternion(ref float x, ref float y, ref float z, ref float w);

        float GetRoll();

        uint GetRoomKeyFrom();

        Position3D GetRotation(int rotationOrder);

        Position3D GetRotationVelocity();

        string GetScript(ref int script);

        float GetSpeed();

        Position3D GetSpeedVector(bool relative);

        float GetSubmergedLevel();

        int GetType();

        float GetUprightValue();

        int GetVehicleIndexFromIndex();

        Position3D GetVelocity();

        Position3D GetWorldPositionOfBone(int boneIndex);

        bool HasAnimEventFired(uint actionHash);

        bool HasAnimFinished(string animDict, string animName, int p3);

        bool HasBeenDamagedByAnyObject();

        bool HasBeenDamagedByAnyPed();

        bool HasBeenDamagedByAnyVehicle();

        bool HasBeenDamagedByWeapon(uint weaponHash, int weaponType);

        bool HasCollidedWithAnything();

        bool HasCollisionLoadedAround();

        bool IsAMissionEntity();

        bool IsAnObject();

        bool IsAPed();

        bool IsAtCoord(float xPos, float yPos, float zPos, float xSize, float ySize, float zSize, bool p7, bool p8, int p9);

        bool IsAtEntity(int entity2, float xSize, float ySize, float zSize, bool p5, bool p6, int p7);

        bool IsAttached();

        bool IsAttachedToAnyObject();

        bool IsAttachedToAnyPed();

        bool IsAttachedToAnyVehicle();

        bool IsAttachedToEntity(int to);

        bool IsAVehicle();

        bool IsDead(int p1);

        bool IsFocus();

        bool IsInAir();

        bool IsInAngledArea(float originX, float originY, float originZ, float edgeX, float edgeY, float edgeZ, float angle, bool p8, bool p9, int p10);

        bool IsInArea(float x1, float y1, float z1, float x2, float y2, float z2, bool p7, bool p8, int p9);

        bool IsInWater();

        bool IsInZone(string zone);

        bool IsOccluded();

        bool IsOnFire();

        bool IsOnScreen();

        bool IsPlayingAnim(string animDict, string animName);

        bool IsStatic();

        bool IsTouchingEntity(int targetEntity);

        bool IsTouchingModel(uint modelHash);

        bool IsUpright(float angle);

        bool IsUpsidedown();

        bool IsVisible();

        bool IsVisibleToScript();

        bool IsWaitingForWorldCollision();

        void NetworkAddToSynchronisedScene(int netScene, string animDict, string animName, float speed, float speedMulitiplier, int flag);

        bool NetworkDoesExistWithNetworkId();

        void NetworkFadeIn(bool state, int p2);

        void NetworkFadeOut(bool normal, bool slow);

        bool NetworkGetIsLocal();

        bool NetworkGetIsNetworked();

        int NetworkGetNetworkIdFrom();

        bool NetworkHasControlOf();

        void NetworkRegisterAsNetworked();

        bool NetworkRequestControlOf();

        void NetworkSetCanBlend(bool toggle);

        void NetworkSetVisibleToNetwork(bool toggle);

        void NetworkUnregisterNetworked();

        bool PlayAnim(string animName, string animDict, float p3, bool loop, bool stayInAnim, bool p6, float delta, int bitset);

        bool PlaySynchronizedAnim(int syncedScene, string animation, string propName, float p4, float p5, int p6, float p7);

        void ProcessAttachments();

        void RemoveParticleFxFrom();

        void ResetAlpha();

        void SetAlpha(int alphaLevel, bool skin);

        void SetAlwaysPrerender(bool toggle);

        void SetAnimCurrentTime(string animDictionary, string animName, float time);

        void SetAnimSpeed(string animDictionary, string animName, float speedMultiplier);

        void SetAsMissionEntity(bool p1, bool p2);

        void SetCanBeDamaged(bool toggle);

        void SetCanBeDamagedByRelationshipGroup(bool bCanBeDamaged, int relGroup);

        void SetCanBeTargetedWithoutLos(bool toggle);

        void SetCollision(bool toggle, bool keepPhysics);

        void SetCoords(float xPos, float yPos, float zPos, bool xAxis, bool yAxis, bool zAxis, bool clearArea);

        void SetCoords2(float xPos, float yPos, float zPos, bool xAxis, bool yAxis, bool zAxis, bool clearArea);

        void SetCoordsNoOffset(float xPos, float yPos, float zPos, bool xAxis, bool yAxis, bool zAxis);

        void SetDynamic(bool toggle);

        void SetFocus();

        void SetGameplayHint(float xOffset, float yOffset, float zOffset, bool p4, int p5, int p6, int p7, int p8);

        void SetHasGravity(bool toggle);

        void SetHeading(float heading);

        void SetHealth(int health);

        void SetIconColor(int red, int green, int blue, int alpha);

        void SetIconVisibility(bool toggle);

        void SetInvincible(bool toggle);

        void SetIsTargetPriority(bool p1, float p2);

        void SetLights(bool toggle);

        void SetLoadCollisionFlag(bool toggle, int p2);

        void SetLocallyInvisible();

        void SetLocallyVisible();

        void SetLodDist(int value);

        void SetMaxHealth(int value);

        void SetMaxSpeed(float speed);

        void SetMotionBlur(bool toggle);

        void SetNoCollisionEntity(int entity2);

        void SetOnlyDamagedByPlayer(bool toggle);

        void SetOnlyDamagedByRelationshipGroup(bool p1, int p2);

        void SetProofs(bool bulletProof, bool fireProof, bool explosionProof, bool collisionProof, bool meleeProof, bool p6, bool p7, bool drownProof);

        void SetQuaternion(float x, float y, float z, float w);

        void SetRecordsCollisions(bool toggle);

        void SetRenderScorched(bool toggle);

        void SetRotation(float pitch, float roll, float yaw, int rotationOrder, bool p5);

        void SetSomething(bool toggle);

        void SetTrafficlightOverride(int state);

        void SetVelocity(float x, float y, float z);

        void SetVisible(bool toggle);

        void SetVisibleInCutscene(bool p1, bool p2);

        int StartFire();

        int StartShapeTestBound(int flags1, int flags2);

        int StartShapeTestBoundingBox(int flags1, int flags2);

        int StopAnim(string animation, string animGroup, float p3);

        void StopFire();

        bool StopSynchronizedAnim(float p1, bool p2);

        #endregion Public Methods
    }
}
