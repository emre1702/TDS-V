using RAGE;
using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Ped;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Ped
{
    internal class PedBase : Entity.EntityBase, IPedBase
    {
        #region Private Fields

        private readonly RAGE.Elements.PedBase _instance;

        #endregion Private Fields

        #region Protected Constructors

        protected PedBase(RAGE.Elements.PedBase instance) : base(instance)
            => _instance = instance;

        #endregion Protected Constructors

        #region Public Properties

        public int Armor
        {
            get => _instance.GetArmour();
            set => _instance.SetArmour(value);
        }

        #endregion Public Properties

        #region Public Methods

        public void AddAmmoTo(WeaponHash weaponHash, int ammo)
            => _instance.AddAmmoTo((uint)weaponHash, ammo);

        public void AddArmourTo(int amount)
            => _instance.AddArmourTo(amount);

        public void AddOwnedExplosion(float x, float y, float z, ExplosionType explosionType, float damageScale, bool isAudible, bool isInvisible, float cameraShake)
            => _instance.AddOwnedExplosion(x, y, z, (int)explosionType, damageScale, isAudible, isInvisible, cameraShake);

        public void AddVehicleSubtaskAttack(int ped2)
            => _instance.AddVehicleSubtaskAttack(ped2);

        public void AddVehicleSubtaskAttackCoord(float x, float y, float z)
            => _instance.AddVehicleSubtaskAttackCoord(x, y, z);

        public void ApplyBlood(int boneIndex, float xRot, float yRot, float zRot, string woundType)
            => _instance.ApplyBlood(boneIndex, xRot, yRot, zRot, woundType);

        public void ApplyBloodByZone(int p1, float p2, float p3, ref int p4)
            => _instance.ApplyBloodByZone(p1, p2, p3, ref p4);

        public void ApplyBloodDamageByZone(int p1, float p2, float p3, int p4)
            => _instance.ApplyBloodDamageByZone(p1, p2, p3, p4);

        public void ApplyBloodSpecific(int p1, float p2, float p3, float p4, float p5, int p6, float p7, ref int p8)
            => _instance.ApplyBloodSpecific(p1, p2, p3, p4, p5, p6, p7, ref p8);

        public void ApplyDamageDecal(int p1, float p2, float p3, float p4, float p5, float p6, int p7, bool p8, string p9)
            => _instance.ApplyDamageDecal(p1, p2, p3, p4, p5, p6, p7, p8, p9);

        public void ApplyDamagePack(string damagePack, float damage, float mult)
            => _instance.ApplyDamagePack(damagePack, damage, mult);

        public void ApplyDamageTo(int damageAmount, bool p2)
            => _instance.ApplyDamageTo(damageAmount, p2);

        public void AttachPortablePickupTo(int p1)
            => _instance.AttachPortablePickupTo(p1);

        public bool CanInCombatSeeTarget(int target)
            => _instance.CanInCombatSeeTarget(target);

        public bool CanKnockOffVehicle()
            => _instance.CanKnockOffVehicle();

        public bool CanRagdoll()
            => _instance.CanRagdoll();

        public bool CanSpeak(string speechName, bool unk)
            => _instance.CanSpeak(speechName, unk);

        public void ClearAllProps()
            => _instance.ClearAllProps();

        public void ClearAlternateMovementAnim(int stance, float p2)
            => _instance.ClearAlternateMovementAnim(stance, p2);

        public void ClearAlternateWalkAnim(float p1)
            => _instance.ClearAlternateWalkAnim(p1);

        public void ClearBloodDamage()
            => _instance.ClearBloodDamage();

        public void ClearBloodDamageByZone(int p1)
            => _instance.ClearBloodDamageByZone(p1);

        public void ClearDamageDecalByZone(int p1, string p2)
            => _instance.ClearDamageDecalByZone(p1, p2);

        public void ClearDecorations()
            => _instance.ClearDecorations();

        public void ClearDriveByClipsetOverride()
            => _instance.ClearDriveByClipsetOverride();

        public void ClearDrivebyTaskUnderneathDrivingTask()
            => _instance.ClearDrivebyTaskUnderneathDrivingTask();

        public void ClearFacialDecorations()
            => _instance.ClearFacialDecorations();

        public void ClearFacialIdleAnimOverride()
            => _instance.ClearFacialIdleAnimOverride();

        public void ClearLastDamageBone()
            => _instance.ClearLastDamageBone();

        public new void ClearLastWeaponDamage()
            => _instance.ClearLastWeaponDamage();

        public void ClearProp(int propId)
            => _instance.ClearProp(propId);

        public void ClearSecondaryTask()
            => _instance.ClearSecondaryTask();

        public void ClearTasks()
            => _instance.ClearTasks();

        public void ClearTasksImmediately()
            => _instance.ClearTasksImmediately();

        public void ClearWetness()
            => _instance.ClearWetness();

        public int Clone(float heading, bool isNetwork, bool p3)
            => _instance.Clone(heading, isNetwork, p3);

        public void CloneToTarget(int targetPed)
            => _instance.CloneToTarget(targetPed);

        public bool ControlMountedWeapon()
            => _instance.ControlMountedWeapon();

        public int CreateMpGamerTag(string username, bool pointedClanTag, bool isRockstarClan, string clanTag, int p5)
            => _instance.CreateMpGamerTag(username, pointedClanTag, isRockstarClan, clanTag, p5);

        public void DetachPortablePickupFrom()
            => _instance.DetachPortablePickupFrom();

        public void DisablePainAudio(bool toggle)
            => _instance.DisablePainAudio(toggle);

        public bool DoesHaveAiBlip()
            => _instance.DoesHaveAiBlip();

        public void EnableTennisMode(bool toggle, bool p2)
            => _instance.EnableTennisMode(toggle, p2);

        public void ExplodeHead(WeaponHash weaponHash)
            => _instance.ExplodeHead((uint)weaponHash);

        public void ExplodeProjectiles(WeaponHash weaponHash, bool p2)
            => _instance.ExplodeProjectiles((uint)weaponHash, p2);

        public bool ForceMotionState(MotionState motionState, bool p2, bool p3, bool p4)
            => _instance.ForceMotionState((uint)motionState, p2, p3, p4);

        public void ForceToOpenParachute()
            => _instance.ForceToOpenParachute();

        public void FreezeCameraRotation()
            => _instance.FreezeCameraRotation();

        public int GetAccuracy()
            => _instance.GetAccuracy();

        public int GetAlertness()
            => _instance.GetAlertness();

        public int GetAmmoByType(int ammoType)
            => _instance.GetAmmoByType(ammoType);

        public int GetAmmoInClip(WeaponHash weaponHash)
        {
            int ammoInClip = 0;
            _instance.GetAmmoInClip((uint)weaponHash, ref ammoInClip);
            return ammoInClip;
        }

        public int GetAmmoInWeapon(WeaponHash weaponHash)
            => _instance.GetAmmoInWeapon((uint)weaponHash);

        public uint GetAmmoTypeFromWeapon(WeaponHash weaponHash)
            => _instance.GetAmmoTypeFromWeapon((uint)weaponHash);

        public uint GetAmmoTypeFromWeapon2(WeaponHash weaponHash)
            => _instance.GetAmmoTypeFromWeapon2((uint)weaponHash);

        public int GetArmour()
            => _instance.GetArmour();

        public uint GetBestWeapon(bool p1)
            => _instance.GetBestWeapon(p1);

        public Position3D GetBoneCoords(PedBone boneId, float offsetX, float offsetY, float offsetZ)
            => _instance.GetBoneCoords((int)boneId, offsetX, offsetY, offsetZ).ToPosition3D();

        public int GetBoneIndex(PedBone boneId)
            => _instance.GetBoneIndex((int)boneId);

        public uint GetCauseOfDeath()
            => _instance.GetCauseOfDeath();

        public float GetCombatFloat(int p1)
            => _instance.GetCombatFloat(p1);

        public int GetCombatMovement()
            => _instance.GetCombatMovement();

        public int GetCombatRange()
            => _instance.GetCombatRange();

        public bool GetConfigFlag(int flagId, bool p2)
            => _instance.GetConfigFlag(flagId, p2);

        public int? GetCurrentVehicleWeapon()
        {
            int weaponHashNumber = 0;
            if (!_instance.GetCurrentVehicleWeapon(ref weaponHashNumber))
                return null;
            return weaponHashNumber;
        }

        public WeaponHash? GetCurrentWeapon(bool p2)
        {
            int weaponHashNumber = 0;
            if (!_instance.GetCurrentWeapon(ref weaponHashNumber, p2))
                return null;
            return (WeaponHash)weaponHashNumber;
        }

        public int GetCurrentWeaponEntityIndex()
            => _instance.GetCurrentWeaponEntityIndex();

        public Position3D GetDeadPickupCoords(float p1, float p2)
            => _instance.GetDeadPickupCoords(p1, p2).ToPosition3D();

        public int GetDecorationsState()
            => _instance.GetDecorationsState();

        public Position3D GetDefensiveAreaPosition(bool p1)
            => _instance.GetDefensiveAreaPosition(p1).ToPosition3D();

        public float GetDesiredMoveBlendRatio()
            => _instance.GetDesiredMoveBlendRatio();

        public int GetDrawableVariation(int componentId)
            => _instance.GetDrawableVariation(componentId);

        public float GetEnveffScale()
            => _instance.GetEnveffScale();

        public Position3D GetExtractedDisplacement(bool worldSpace)
            => _instance.GetExtractedDisplacement(worldSpace).ToPosition3D();

        public int GetGroupIndex()
            => _instance.GetGroupIndex();

        public int? GetHeadBlendData()
        {
            int headBlendData = 0;
            if (!_instance.GetHeadBlendData(ref headBlendData))
                return null;
            return headBlendData;
        }

        public int GetHeadOverlayValue(int overlayID)
            => _instance.GetHeadOverlayValue(overlayID);

        public bool GetIsGadgetEquipped(uint gadgetHash)
            => _instance.GetIsGadgetEquipped(gadgetHash);

        public bool GetIsTaskActive(int taskNumber)
            => _instance.GetIsTaskActive(taskNumber);

        public int GetJackTarget()
            => _instance.GetJackTarget();

        public PedBone? GetLastDamageBone()
        {
            int outBone = 0;
            if (!_instance.GetLastDamageBone(ref outBone))
                return null;
            return (PedBone)outBone;
        }

        public Position3D GetLastWeaponImpactCoord()
        {
            Vector3 coords = new Vector3();
            if (!_instance.GetLastWeaponImpactCoord(coords))
                return null;
            return coords?.ToPosition3D();
        }

        public float GetLockonRangeOfCurrentWeapon()
            => _instance.GetLockonRangeOfCurrentWeapon();

        public int? GetMaxAmmo(WeaponHash weaponHash)
        {
            int ammo = 0;
            if (!_instance.GetMaxAmmo((uint)weaponHash, ref ammo))
                return null;
            return ammo;
        }

        public int GetMaxAmmoInClip(WeaponHash weaponHash, bool p2)
            => _instance.GetMaxAmmoInClip((uint)weaponHash, p2);

        public float GetMaxRangeOfCurrentWeapon()
            => _instance.GetMaxRangeOfCurrentWeapon();

        public int GetMeleeTargetFor()
            => _instance.GetMeleeTargetFor();

        public int GetMoney()
            => _instance.GetMoney();

        public int GetMount()
            => _instance.GetMount();

        public int GetNavmeshRouteDistanceRemaining(ref int p1, ref int p2)
            => _instance.GetNavmeshRouteDistanceRemaining(ref p1, ref p2);

        public int GetNavmeshRouteResult()
            => _instance.GetNavmeshRouteResult();

        public int GetNearbyPeds(ref int sizeAndPeds, int ignore)
            => _instance.GetNearbyPeds(ref sizeAndPeds, ignore);

        public int GetNearbyVehicles(ref int sizeAndVehs)
            => _instance.GetNearbyVehicles(ref sizeAndVehs);

        public int GetNumberOfDrawableVariations(int componentId)
            => _instance.GetNumberOfDrawableVariations(componentId);

        public int GetNumberOfPropDrawableVariations(int propId)
            => _instance.GetNumberOfPropDrawableVariations(propId);

        public int GetNumberOfPropTextureVariations(int propId, int drawableId)
            => _instance.GetNumberOfPropTextureVariations(propId, drawableId);

        public int GetNumberOfTextureVariations(int componentId, int drawableId)
            => _instance.GetNumberOfTextureVariations(componentId, drawableId);

        public int GetPaletteVariation(int componentId)
            => _instance.GetPaletteVariation(componentId);

        public int GetParachuteLandingType()
            => _instance.GetParachuteLandingType();

        public int GetParachuteState()
            => _instance.GetParachuteState();

        public int GetParachuteTintIndex()
        {
            int tintIndex = 0;
            _instance.GetParachuteTintIndex(ref tintIndex);
            return tintIndex;
        }

        public float GetPhoneGestureAnimCurrentTime()
            => _instance.GetPhoneGestureAnimCurrentTime();

        public float GetPhoneGestureAnimTotalTime()
            => _instance.GetPhoneGestureAnimTotalTime();

        public int GetPlayerIsFollowing()
            => _instance.GetPlayerIsFollowing();

        public int GetPropIndex(int componentId)
            => _instance.GetPropIndex(componentId);

        public int GetPropTextureIndex(int componentId)
            => _instance.GetPropTextureIndex(componentId);

        public int GetRagdollBoneIndex(PedBone bone)
            => _instance.GetRagdollBoneIndex((int)bone);

        public uint GetRelationshipGroupDefaultHash()
            => _instance.GetRelationshipGroupDefaultHash();

        public uint GetRelationshipGroupHash()
            => _instance.GetRelationshipGroupHash();

        public bool GetResetFlag(int flagId)
            => _instance.GetResetFlag(flagId);

        public int GetSeatIsTryingToEnter()
            => _instance.GetSeatIsTryingToEnter();

        public WeaponHash GetSelectedWeapon()
            => (WeaponHash)_instance.GetSelectedWeapon();

        public int GetSequenceProgress()
            => _instance.GetSequenceProgress();

        public int GetsJacker()
            => _instance.GetsJacker();

        public int GetSourceOfDeath()
            => _instance.GetSourceOfDeath();

        public bool GetStealthMovement()
            => _instance.GetStealthMovement();

        public int GetTextureVariation(int componentId)
            => _instance.GetTextureVariation(componentId);

        public int GetTimeOfDeath()
            => _instance.GetTimeOfDeath();

        public int GetVehicleIsEntering()
            => _instance.GetVehicleIsEntering();

        public int GetVehicleIsIn(bool lastVehicle)
            => _instance.GetVehicleIsIn(lastVehicle);

        public int GetVehicleIsTryingToEnter()
            => _instance.GetVehicleIsTryingToEnter();

        public int GetVehicleIsUsing()
            => _instance.GetVehicleIsUsing();

        public int GetWeaponObjectFrom(bool p1)
            => _instance.GetWeaponObjectFrom(p1);

        public int GetWeaponTintIndex(WeaponHash weaponHash)
            => _instance.GetWeaponTintIndex((uint)weaponHash);

        public uint GetWeapontypeInSlot(uint weaponSlot)
            => _instance.GetWeapontypeInSlot(weaponSlot);

        public void GiveDelayedWeaponTo(WeaponHash weaponHash, int time, bool equipNow)
            => _instance.GiveDelayedWeaponTo((uint)weaponHash, time, equipNow);

        public void GiveHelmet(bool cannotRemove, int helmetFlag, int textureIndex)
            => _instance.GiveHelmet(cannotRemove, helmetFlag, textureIndex);

        public void GiveNmMessage()
            => _instance.GiveNmMessage();

        public void GiveToPauseMenu(int p1)
            => _instance.GiveToPauseMenu(p1);

        public void GiveWeaponComponentTo(WeaponHash weaponHash, uint componentHash)
            => _instance.GiveWeaponComponentTo((uint)weaponHash, componentHash);

        public void GiveWeaponTo(WeaponHash weaponHash, int ammoCount, bool isHidden, bool equipNow)
            => _instance.GiveWeaponTo((uint)weaponHash, ammoCount, isHidden, equipNow);

        public bool HasBeenDamagedByWeapon(WeaponHash weaponHash, int weaponType)
            => _instance.HasBeenDamagedByWeapon((uint)weaponHash, weaponType);

        public bool HasGotWeapon(WeaponHash weaponHash, bool p2)
                    => _instance.HasGotWeapon((uint)weaponHash, p2);

        public bool HasGotWeaponComponent(WeaponHash weaponHash, uint componentHash)
            => _instance.HasGotWeaponComponent((uint)weaponHash, componentHash);

        public bool HasHeadBlendFinished()
            => _instance.HasHeadBlendFinished();

        public bool HasUseScenarioTask()
            => _instance.HasUseScenarioTask();

        public void HideBloodDamageByZone(int p1, bool p2)
            => _instance.HideBloodDamageByZone(p1, p2);

        public void HideWeaponForScriptedCutscene(bool toggle)
            => _instance.HideWeaponForScriptedCutscene(toggle);

        public bool IsActiveInScenario()
            => _instance.IsActiveInScenario();

        public bool IsAimingFromCover()
            => _instance.IsAimingFromCover();

        public bool IsAmbientSpeechDisabled()
            => _instance.IsAmbientSpeechDisabled();

        public bool IsAnySpeechPlaying()
            => _instance.IsAnySpeechPlaying();

        public bool IsAPlayer()
            => _instance.IsAPlayer();

        public bool IsArmed(int p1)
            => _instance.IsArmed(p1);

        public bool IsBeingArrested()
            => _instance.IsBeingArrested();

        public bool IsBeingJacked()
            => _instance.IsBeingJacked();

        public bool IsBeingStealthKilled()
            => _instance.IsBeingStealthKilled();

        public bool IsBeingStunned(int p1)
            => _instance.IsBeingStunned(p1);

        public bool IsBlushColorValid()
            => _instance.IsBlushColorValid();

        public bool IsClimbing()
            => _instance.IsClimbing();

        public bool IsComponentVariationValid(int componentId, int drawableId, int textureId)
                    => _instance.IsComponentVariationValid(componentId, drawableId, textureId);

        public bool IsConversationDead()
            => _instance.IsConversationDead();

        public bool IsCuffed()
            => _instance.IsCuffed();

        public bool IsCurrentWeaponSilenced()
            => _instance.IsCurrentWeaponSilenced();

        public bool IsDeadOrDying()
            => _instance.IsDeadOrDying(true);

        public bool IsDiving()
            => _instance.IsDiving();

        public bool IsDoingDriveby()
            => _instance.IsDoingDriveby();

        public bool IsDrivebyTaskUnderneathDrivingTask()
            => _instance.IsDrivebyTaskUnderneathDrivingTask();

        public bool IsDucking()
            => _instance.IsDucking();

        public bool IsEvasiveDiving(ref int evadingEntity)
            => _instance.IsEvasiveDiving(ref evadingEntity);

        public bool IsFacingPed(int otherPed, float angle)
            => _instance.IsFacingPed(otherPed, angle);

        public bool IsFalling()
            => _instance.IsFalling();

        public bool IsFatallyInjured()
            => _instance.IsFatallyInjured();

        public bool IsFlashLightOn()
            => _instance.IsFlashLightOn();

        public bool IsFleeing()
            => _instance.IsFleeing();

        public bool IsGettingIntoAVehicle()
            => _instance.IsGettingIntoAVehicle();

        public bool IsGettingUp()
            => _instance.IsGettingUp();

        public bool IsGoingIntoCover()
            => _instance.IsGoingIntoCover();

        public bool IsGroupMember(int groupId)
            => _instance.IsGroupMember(groupId);

        public bool IsHairColorValid()
            => _instance.IsHairColorValid();

        public bool IsHangingOnToVehicle()
            => _instance.IsHangingOnToVehicle();

        public bool IsHeadshotReady()
            => _instance.IsheadshotReady();

        public bool IsHeadshotValid()
            => _instance.IsheadshotValid();

        public bool IsHeadtrackingEntity(int entity)
            => _instance.IsHeadtrackingEntity(entity);

        public bool IsHeadtrackingPed(int ped2)
            => _instance.IsHeadtrackingPed(ped2);

        public bool IsHuman()
            => _instance.IsHuman();

        public bool IsHurt()
            => _instance.IsHurt();

        public bool IsInAnyBoat()
            => _instance.IsInAnyBoat();

        public bool IsInAnyHeli()
            => _instance.IsInAnyHeli();

        public bool IsInAnyPlane()
            => _instance.IsInAnyPlane();

        public bool IsInAnyPoliceVehicle()
            => _instance.IsInAnyPoliceVehicle();

        public bool IsInAnySub()
            => _instance.IsInAnySub();

        public bool IsInAnyTaxi()
            => _instance.IsInAnyTaxi();

        public bool IsInAnyTrain()
            => _instance.IsInAnyTrain();

        public bool IsInAnyVehicle(bool atGetIn)
            => _instance.IsInAnyVehicle(atGetIn);

        public bool IsInCombat(int target)
            => _instance.IsInCombat(target);

        public bool IsInCover(bool p1)
            => _instance.IsInCover(p1);

        public bool IsInCoverFacingLeft()
            => _instance.IsInCoverFacingLeft();

        public bool IsInCurrentConversation()
            => _instance.IsInCurrentConversation();

        public bool IsInFlyingVehicle()
            => _instance.IsInFlyingVehicle();

        public bool IsInGroup()
            => _instance.IsInGroup();

        public bool IsInjured()
            => _instance.IsInjured();

        public bool IsInMeleeCombat()
            => _instance.IsInMeleeCombat();

        public bool IsInModel(uint modelHash)
            => _instance.IsInModel(modelHash);

        public bool IsInParachuteFreeFall()
            => _instance.IsInParachuteFreeFall();

        public bool IsInVehicle(int vehicle, bool atGetIn)
            => _instance.IsInVehicle(vehicle, atGetIn);

        public bool IsInWrithe()
            => _instance.IsInWrithe();

        public bool IsJacking()
            => _instance.IsJacking();

        public bool IsJumping()
            => _instance.IsJumping();

        public bool IsJumpingOutOfVehicle()
            => _instance.IsJumpingOutOfVehicle();

        public bool IsLipstickColorValid()
            => _instance.IsLipstickColorValid();

        public bool IsMale()
            => _instance.IsMale();

        public bool IsModel(uint modelHash)
            => _instance.IsModel(modelHash);

        public bool IsMountedWeaponTaskUnderneathDrivingTask()
            => _instance.IsMountedWeaponTaskUnderneathDrivingTask();

        public bool IsMoveBlendRatioRunning()
            => _instance.IsMoveBlendRatioRunning();

        public bool IsMoveBlendRatioSprinting()
            => _instance.IsMoveBlendRatioSprinting();

        public bool IsMoveBlendRatioStill()
            => _instance.IsMoveBlendRatioStill();

        public bool IsMoveBlendRatioWalking()
            => _instance.IsMoveBlendRatioWalking();

        public bool IsOnAnyBike()
            => _instance.IsOnAnyBike();

        public bool IsOnFoot()
            => _instance.IsOnFoot();

        public bool IsOnMount()
            => _instance.IsOnMount();

        public bool IsOnSpecificVehicle(int vehicle)
            => _instance.IsOnSpecificVehicle(vehicle);

        public bool IsOnVehicle()
            => _instance.IsOnVehicle();

        public bool IsPerformingStealthKill()
            => _instance.IsPerformingStealthKill();

        public bool IsPlantingBomb()
            => _instance.IsPlantingBomb();

        public bool IsPlayingPhoneGestureAnim()
            => _instance.IsPlayingPhoneGestureAnim();

        public bool IsProne()
            => _instance.IsProne();

        public bool IsPropValid(int componentId, int drawableId, int TextureId)
            => _instance.IsPropValid(componentId, drawableId, TextureId);

        public bool IsRagdoll()
            => _instance.IsRagdoll();

        public bool IsReloading()
            => _instance.IsReloading();

        public bool IsRespondingToEvent(int eventId)
            => _instance.IsRespondingToEvent(eventId);

        public bool IsRingtonePlaying()
            => _instance.IsRingtonePlaying();

        public bool IsRunning()
            => _instance.IsRunning();

        public bool IsRunningArrestTask()
            => _instance.IsRunningArrestTask();

        public bool IsRunningMobilePhoneTask()
            => _instance.IsRunningMobilePhoneTask();

        public bool IsRunningRagdollTask()
            => _instance.IsRunningRagdollTask();

        public bool IsScriptedScenarioUsingConditionalAnim(string animDict, string anim)
            => _instance.IsScriptedScenarioUsingConditionalAnim(animDict, anim);

        public bool IsShooting()
            => _instance.IsShooting();

        public bool IsShootingInArea(float x1, float y1, float z1, float x2, float y2, float z2, bool p7, bool p8)
            => _instance.IsShootingInArea(x1, y1, z1, x2, y2, z2, p7, p8);

        public bool IsSittingInAnyVehicle()
            => _instance.IsSittingInAnyVehicle();

        public bool IsSittingInVehicle(int vehicle)
            => _instance.IsSittingInVehicle(vehicle);

        public bool IsSprinting()
            => _instance.IsSprinting();

        public bool IsStandingInCover()
            => _instance.IsStandingInCover();

        public bool IsStill()
            => _instance.IsStill();

        public bool IsStopped()
            => _instance.IsStopped();

        public bool IsStrafing()
            => _instance.IsStrafing();

        public bool IsSwimming()
            => _instance.IsSwimming();

        public bool IsSwimmingUnderWater()
            => _instance.IsSwimmingUnderWater();

        public bool IsTennisMode()
            => _instance.IsTennisMode();

        public bool IsTracked()
            => _instance.IsTracked();

        public bool IsTrackedVisible()
            => _instance.IsTrackedVisible();

        public bool IsTryingToEnterALockedVehicle()
            => _instance.IsTryingToEnterALockedVehicle();

        public bool IsUsingActionMode()
            => _instance.IsUsingActionMode();

        public bool IsUsingAnyScenario()
            => _instance.IsUsingAnyScenario();

        public bool IsUsingScenario(string scenario)
            => _instance.IsUsingScenario(scenario);

        public bool IsVaulting()
            => _instance.IsVaulting();

        public bool IsWalking()
            => _instance.IsWalking();

        public bool IsWeaponComponentActive(WeaponHash weaponHash, uint componentHash)
            => _instance.IsWeaponComponentActive((uint)weaponHash, componentHash);

        public bool IsWeaponReadyToShoot()
            => _instance.IsWeaponReadyToShoot();

        public bool IsWearingHelmet()
            => _instance.IsWearingHelmet();

        public void KnockOffProp(bool p1, bool p2, bool p3, bool p4)
            => _instance.KnockOffProp(p1, p2, p3, p4);

        public void KnockOffVehicle()
            => _instance.KnockOffVehicle();

        public bool MakeReload()
            => _instance.MakeReload();

        public void NetworkAddToSynchronisedScene(int netScene, string animDict, string animnName, float speed, float speedMultiplier, int duration, int flag, float playbackRate, int p9)
            => _instance.NetworkAddToSynchronisedScene(netScene, animDict, animnName, speed, speedMultiplier, duration, flag, playbackRate, p9);

        public int NetworkGetPlayerIndexFrom()
            => _instance.NetworkGetPlayerIndexFrom();

        public void PlayAmbientSpeech1(string speechName, string speechParam, int p3)
            => _instance.PlayAmbientSpeech1(speechName, speechParam, p3);

        public void PlayAmbientSpeech2(string speechName, string speechParam, int p3)
            => _instance.PlayAmbientSpeech2(speechName, speechParam, p3);

        public void PlayAnimOnRunningScenario(string animDict, string animName)
            => _instance.PlayAnimOnRunningScenario(animDict, animName);

        public void PlayFacialAnim(string animName, string animDict)
            => _instance.PlayFacialAnim(animName, animDict);

        public void PlayPain(int painID, int p2, int p3)
            => _instance.PlayPain(painID, p2, p3);

        public void PlayStreamFrom()
            => _instance.PlayStreamFrom();

        public void RegisterHatedTargetsAround(float radius)
            => _instance.RegisterHatedTargetsAround(radius);

        public int RegisterHeadshot()
            => _instance.Registerheadshot();

        public void RegisterTarget(int target)
            => _instance.RegisterTarget(target);

        public void RemoveAllWeapons(bool p1)
            => _instance.RemoveAllWeapons(p1);

        public void RemoveDefensiveArea(bool toggle)
            => _instance.RemoveDefensiveArea(toggle);

        public void RemoveFromGroup()
            => _instance.RemoveFromGroup();

        public void RemoveHelmet(bool instantly)
            => _instance.RemoveHelmet(instantly);

        public void RemovePreferredCoverSet()
            => _instance.RemovePreferredCoverSet();

        public void RemoveWeaponComponentFrom(WeaponHash weaponHash, uint componentHash)
            => _instance.RemoveWeaponComponentFrom((uint)weaponHash, componentHash);

        public void RemoveWeaponFrom(WeaponHash weaponHash)
            => _instance.RemoveWeaponFrom((uint)weaponHash);

        public void ResetInVehicleContext()
            => _instance.ResetInVehicleContext();

        public void ResetLastVehicle()
            => _instance.ResetLastVehicle();

        public void ResetMovementClipset(float clipSetSwitchTime)
            => _instance.ResetMovementClipset(clipSetSwitchTime);

        public void ResetRagdollBlockingFlags(int flags)
            => _instance.ResetRagdollBlockingFlags(flags);

        public void ResetRagdollTimer()
            => _instance.ResetRagdollTimer();

        public void ResetStrafeClipset()
            => _instance.ResetStrafeClipset();

        public void ResetVisibleDamage()
            => _instance.ResetVisibleDamage();

        public void ResetWeaponMovementClipset()
            => _instance.ResetWeaponMovementClipset();

        public void Resurrect()
            => _instance.Resurrect();

        public void ReviveInjured()
            => _instance.ReviveInjured();

        public void SetAccuracy(int accuracy)
            => _instance.SetAccuracy(accuracy);

        public void SetAiBlipMaxDistance(float distance)
            => _instance.SetAiBlipMaxDistance(distance);

        public void SetAlertness(int value)
            => _instance.SetAlertness(value);

        public void SetAllowedToDuck(bool toggle)
            => _instance.SetAllowedToDuck(toggle);

        public void SetAllowVehiclesOverride(bool toggle)
            => _instance.SetAllowVehiclesOverride(toggle);

        public void SetAlternateMovementAnim(int stance, string animDictionary, string animationName, float p4, bool p5)
            => _instance.SetAlternateMovementAnim(stance, animDictionary, animationName, p4, p5);

        public void SetAlternateWalkAnim(string animDict, string animName, float p3, bool p4)
            => _instance.SetAlternateWalkAnim(animDict, animName, p3, p4);

        public void SetAmbientVoiceName(string name)
            => _instance.SetAmbientVoiceName(name);

        public void SetAmmo(WeaponHash weaponHash, int ammo, int p3)
            => _instance.SetAmmo((uint)weaponHash, ammo, p3);

        public void SetAmmoByType(int ammoType, int ammo)
            => _instance.SetAmmoByType(ammoType, ammo);

        public bool SetAmmoInClip(WeaponHash weaponHash, int ammo)
            => _instance.SetAmmoInClip((uint)weaponHash, ammo);

        public void SetAmmoToDrop(int p1)
            => _instance.SetAmmoToDrop(p1);

        public void SetAngledDefensiveArea(float p1, float p2, float p3, float p4, float p5, float p6, float p7, bool p8, bool p9)
            => _instance.SetAngledDefensiveArea(p1, p2, p3, p4, p5, p6, p7, p8, p9);

        public void SetArmour(int amount)
            => _instance.SetArmour(amount);

        public void SetAsCop(bool toggle)
            => _instance.SetAsCop(toggle);

        public void SetAsEnemy(bool toggle)
            => _instance.SetAsEnemy(toggle);

        public void SetAsGroupLeader(int groupId)
            => _instance.SetAsGroupLeader(groupId);

        public void SetAsGroupMember(int groupId)
            => _instance.SetAsGroupMember(groupId);

        public void SetBlendFromParents(int p1, int p2, float p3, float p4)
            => _instance.SetBlendFromParents(p1, p2, p3, p4);

        public void SetBlockingOfNonTemporaryEvents(bool toggle)
            => _instance.SetBlockingOfNonTemporaryEvents(toggle);

        public void SetBoundsOrientation(float p1, float p2, float p3, float p4, float p5)
            => _instance.SetBoundsOrientation(p1, p2, p3, p4, p5);

        public void SetCanArmIk(bool toggle)
            => _instance.SetCanArmIk(toggle);

        public void SetCanAttackFriendly(bool toggle)
            => _instance.SetCanAttackFriendly(toggle, false);

        public void SetCanBeDraggedOut(bool toggle)
            => _instance.SetCanBeDraggedOut(toggle);

        public void SetCanBeKnockedOffVehicle(int state)
            => _instance.SetCanBeKnockedOffVehicle(state);

        public void SetCanBeShotInVehicle(bool toggle)
            => _instance.SetCanBeShotInVehicle(toggle);

        public void SetCanBeTargetedWhenInjured(bool toggle)
            => _instance.SetCanBeTargetedWhenInjured(toggle);

        public void SetCanBeTargetted(bool toggle)
            => _instance.SetCanBeTargetted(toggle);

        public void SetCanBeTargettedByPlayer(int player, bool toggle)
            => _instance.SetCanBeTargettedByPlayer(player, toggle);

        public void SetCanBeTargettedByTeam(int team, bool toggle)
            => _instance.SetCanBeTargettedByTeam(team, toggle);

        public void SetCanCowerInCover(bool toggle)
            => _instance.SetCanCowerInCover(toggle);

        public void SetCanEvasiveDive(bool toggle)
            => _instance.SetCanEvasiveDive(toggle);

        public void SetCanHeadIk(bool toggle)
            => _instance.SetCanHeadIk(toggle);

        public void SetCanLegIk(bool toggle)
            => _instance.SetCanLegIk(toggle);

        public void SetCanPeekInCover(bool toggle)
            => _instance.SetCanPeekInCover(toggle);

        public void SetCanPlayAmbientAnims(bool toggle)
            => _instance.SetCanPlayAmbientAnims(toggle);

        public void SetCanPlayAmbientBaseAnims(bool toggle)
            => _instance.SetCanPlayAmbientBaseAnims(toggle);

        public void SetCanPlayGestureAnims(bool toggle)
            => _instance.SetCanPlayGestureAnims(toggle);

        public void SetCanPlayVisemeAnims(bool toggle, bool p2)
            => _instance.SetCanPlayVisemeAnims(toggle, p2);

        public void SetCanRagdoll(bool toggle)
            => _instance.SetCanRagdoll(toggle);

        public void SetCanRagdollFromPlayerImpact(bool toggle)
            => _instance.SetCanRagdollFromPlayerImpact(toggle);

        public void SetCanSmashGlass(bool p1, bool p2)
            => _instance.SetCanSmashGlass(p1, p2);

        public void SetCanSwitchWeapon(bool toggle)
            => _instance.SetCanSwitchWeapon(toggle);

        public void SetCanTeleportToGroupLeader(int groupHandle, bool toggle)
            => _instance.SetCanTeleportToGroupLeader(groupHandle, toggle);

        public void SetCanTorsoIk(bool toggle)
            => _instance.SetCanTorsoIk(toggle);

        public void SetCanUseAutoConversationLookat(bool toggle)
            => _instance.SetCanUseAutoConversationLookat(toggle);

        public void SetCapsule(float value)
            => _instance.SetCapsule(value);

        public void SetChanceOfFiringBlanks(float xBias, float yBias)
            => _instance.SetChanceOfFiringBlanks(xBias, yBias);

        public void SetClothProne(int p1)
            => _instance.SetClothProne(p1);

        public void SetCombatAbility(int p1)
            => _instance.SetCombatAbility(p1);

        public void SetCombatAttributes(int attributeIndex, bool enabled)
            => _instance.SetCombatAttributes(attributeIndex, enabled);

        public void SetCombatFloat(int combatType, float p2)
            => _instance.SetCombatFloat(combatType, p2);

        public void SetCombatMovement(int combatMovement)
            => _instance.SetCombatMovement(combatMovement);

        public void SetCombatRange(int p1)
            => _instance.SetCombatRange(p1);

        public void SetComponentVariation(int componentId, int drawableId, int textureId, int paletteId)
            => _instance.SetComponentVariation(componentId, drawableId, textureId, paletteId);

        public void SetConfigFlag(int flagId, bool value)
            => _instance.SetConfigFlag(flagId, value);

        public void SetCoordsKeepVehicle(float posX, float posY, float posZ)
            => _instance.SetCoordsKeepVehicle(posX, posY, posZ);

        public void SetCoordsNoGang(float posX, float posY, float posZ)
            => _instance.SetCoordsNoGang(posX, posY, posZ);

        public void SetCowerHash(string p1)
            => _instance.SetCowerHash(p1);

        public bool SetCurrentVehicleWeapon(WeaponHash weaponHash)
            => _instance.SetCurrentVehicleWeapon((uint)weaponHash);

        public void SetCurrentWeapon(WeaponHash weaponHash, bool equipNow)
            => _instance.SetCurrentWeapon((uint)weaponHash, equipNow);

        public void SetCurrentWeaponVisible(bool visible, bool deselectWeapon, bool p3, bool p4)
            => _instance.SetCurrentWeaponVisible(visible, deselectWeapon, p3, p4);

        public void SetDecisionMaker(uint name)
            => _instance.SetDecisionMaker(name);

        public void SetDecoration(uint collection, uint overlay)
            => _instance.SetDecoration(collection, overlay);

        public void SetDefaultComponentVariation()
            => _instance.SetDefaultComponentVariation();

        public void SetDefensiveAreaAttachedToPed(int attachPed, float p2, float p3, float p4, float p5, float p6, float p7, float p8, bool p9, bool p10)
            => _instance.SetDefensiveAreaAttachedToPed(attachPed, p2, p3, p4, p5, p6, p7, p8, p9, p10);

        public void SetDefensiveAreaDirection(float p1, float p2, float p3, bool p4)
            => _instance.SetDefensiveAreaDirection(p1, p2, p3, p4);

        public void SetDefensiveSphereAttachedToPed(int target, float xOffset, float yOffset, float zOffset, float radius, bool p6)
            => _instance.SetDefensiveSphereAttachedToPed(target, xOffset, yOffset, zOffset, radius, p6);

        public void SetDesiredHeading(float heading)
            => _instance.SetDesiredHeading(heading);

        public void SetDesiredMoveBlendRatio(float p1)
            => _instance.SetDesiredMoveBlendRatio(p1);

        public void SetDiesInSinkingVehicle(bool toggle)
            => _instance.SetDiesInSinkingVehicle(toggle);

        public void SetDiesInstantlyInWater(bool toggle)
            => _instance.SetDiesInstantlyInWater(toggle);

        public void SetDiesInVehicle(bool toggle)
            => _instance.SetDiesInVehicle(toggle);

        public void SetDiesInWater(bool toggle)
            => _instance.SetDiesInWater(toggle);

        public void SetDiesWhenInjured(bool toggle)
            => _instance.SetDiesWhenInjured(toggle);

        public void SetDriveByClipsetOverride(string clipset)
            => _instance.SetDriveByClipsetOverride(clipset);

        public void SetDriveTaskDrivingStyle(int drivingStyle)
            => _instance.SetDriveTaskDrivingStyle(drivingStyle);

        public void SetDropsInventoryWeapon(WeaponHash weaponHash, float xOffset, float yOffset, float zOffset, int p5)
            => _instance.SetDropsInventoryWeapon((uint)weaponHash, xOffset, yOffset, zOffset, p5);

        public void SetDropsWeapon()
            => _instance.SetDropsWeapon();

        public void SetDropsWeaponsWhenDead(bool toggle)
            => _instance.SetDropsWeaponsWhenDead(toggle);

        public void SetDucking(bool toggle)
            => _instance.SetDucking(toggle);

        public void SetEnableBoundAnkles(bool toggle)
            => _instance.SetEnableBoundAnkles(toggle);

        public void SetEnableEnveffScale(bool toggle)
            => _instance.SetEnableEnveffScale(toggle);

        public void SetEnableHandcuffs(bool toggle)
            => _instance.SetEnableHandcuffs(toggle);

        public void SetEnableScuba(bool toggle)
            => _instance.SetEnableScuba(toggle);

        public void SetEnableWeaponBlocking(bool toggle)
            => _instance.SetEnableWeaponBlocking(toggle);

        public void SetEnemyAiBlip(bool showViewCones)
            => _instance.SetEnemyAiBlip(showViewCones);

        public void SetEnveffScale(float value)
            => _instance.SetEnveffScale(value);

        public void SetEyeColor(int index)
            => _instance.SetEyeColor(index);

        public void SetFaceFeature(int index, float scale)
            => _instance.SetFaceFeature(index, scale);

        public void SetFacialDecoration(uint collection, uint overlay)
            => _instance.SetFacialDecoration(collection, overlay);

        public void SetFacialIdleAnimOverride(string animName, string animDict)
            => _instance.SetFacialIdleAnimOverride(animName, animDict);

        public void SetFiringPattern(uint patternHash)
            => _instance.SetFiringPattern(patternHash);

        public void SetFleeAttributes(int attributes, bool p2)
            => _instance.SetFleeAttributes(attributes, p2);

        public void SetGadget(uint gadgetHash, bool p2)
            => _instance.SetGadget(gadgetHash, p2);

        public void SetGeneratesDeadBodyEvents(bool toggle)
            => _instance.SetGeneratesDeadBodyEvents(toggle);

        public void SetGestureGroup(string animGroupGesture)
            => _instance.SetGestureGroup(animGroupGesture);

        public void SetGetOutUpsideDownVehicle(bool toggle)
            => _instance.SetGetOutUpsideDownVehicle(toggle);

        public void SetGravity(bool toggle)
            => _instance.SetGravity(toggle);

        public void SetGroupMemberPassengerIndex(int index)
            => _instance.SetGroupMemberPassengerIndex(index);

        public void SetHairColor(int colorID, int highlightColorID)
            => _instance.SetHairColor(colorID, highlightColorID);

        public void SetHeadBlendData(int shapeFirstID, int shapeSecondID, int shapeThirdID, int skinFirstID, int skinSecondID, int skinThirdID, float shapeMix, float skinMix, float thirdMix, bool isParent)
            => _instance.SetHeadBlendData(shapeFirstID, shapeSecondID, shapeThirdID, skinFirstID, skinSecondID, skinThirdID, shapeMix, skinMix, thirdMix, isParent);

        public void SetHeadBlendPaletteColor(Color color, int type)
            => RAGE.Game.Invoker.Invoke((ulong)NativeHash.SET_HEAD_BLEND_PALETTE_COLOR, _instance.Handle, (int)color.R, (int)color.G, (int)color.B, type);

        public void SetHeadBlendPaletteColor(ColorDto color, int type)
            => RAGE.Game.Invoker.Invoke((ulong)NativeHash.SET_HEAD_BLEND_PALETTE_COLOR, _instance.Handle, (int)color.R, (int)color.G, (int)color.B, type);

        public void SetHeadOverlay(int overlayID, int index, float opacity)
            => _instance.SetHeadOverlay(overlayID, index, opacity);

        public void SetHeadOverlayColor(int overlayID, int colorType, int colorID, int secondColorID)
            => _instance.SetHeadOverlayColor(overlayID, colorType, colorID, secondColorID);

        public void SetHearingRange(float value)
            => _instance.SetHearingRange(value);

        public void SetHelmet(bool canWearHelmet)
            => _instance.SetHelmet(canWearHelmet);

        public void SetHelmetFlag(int helmetFlag)
            => _instance.SetHelmetFlag(helmetFlag);

        public void SetHelmetPropIndex(int propIndex, int p2)
            => _instance.SetHelmetPropIndex(propIndex, p2);

        public void SetHelmetTextureIndex(int textureIndex)
            => _instance.SetHelmetTextureIndex(textureIndex);

        public void SetHighFallTask(int p1, int p2, int p3)
            => _instance.SetHighFallTask(p1, p2, p3);

        public void SetIdRange(float value)
            => _instance.SetIdRange(value);

        public void SetIkTarget(int p1, int targetPed, int boneLookAt, float x, float y, float z, int p7, int duration, int duration1)
            => _instance.SetIkTarget(p1, targetPed, boneLookAt, x, y, z, p7, duration, duration1);

        public void SetInfiniteAmmo(bool toggle, WeaponHash weaponHash)
            => _instance.SetInfiniteAmmo(toggle, (uint)weaponHash);

        public void SetInfiniteAmmoClip(bool toggle)
            => _instance.SetInfiniteAmmoClip(toggle);

        public void SetIntoVehicle(int vehicle, VehicleSeat seatIndex)
            => _instance.SetIntoVehicle(vehicle, (int)seatIndex - 1);

        public void SetInVehicleContext(uint context)
            => _instance.SetInVehicleContext(context);

        public void SetIsDrunk(bool toggle)
            => _instance.SetIsDrunk(toggle);

        public void SetKeepTask(bool toggle)
            => _instance.SetKeepTask(toggle);

        public void SetLegIkMode(int mode)
            => _instance.SetLegIkMode(mode);

        public void SetLodMultiplier(float multiplier)
            => _instance.SetLodMultiplier(multiplier);

        public void SetMaxMoveBlendRatio(float value)
            => _instance.SetMaxMoveBlendRatio(value);

        public void SetMaxTimeInWater(float value)
            => _instance.SetMaxTimeInWater(value);

        public void SetMaxTimeUnderwater(float value)
            => _instance.SetMaxTimeUnderwater(value);

        public void SetMinGroundTimeForStungun(int ms)
            => _instance.SetMinGroundTimeForStungun(ms);

        public void SetMinMoveBlendRatio(float value)
            => _instance.SetMinMoveBlendRatio(value);

        public void SetModelIsSuppressed(bool toggle)
            => _instance.SetModelIsSuppressed(toggle);

        public void SetMoney(int amount)
            => _instance.SetMoney(amount);

        public void SetMoveAnimsBlendOut()
            => _instance.SetMoveAnimsBlendOut();

        public void SetMovementClipset(string clipSet, float clipSetSwitchTime)
            => _instance.SetMovementClipset(clipSet, clipSetSwitchTime);

        public void SetMoveRateOverride(float value)
            => _instance.SetMoveRateOverride(value);

        public void SetMute()
            => _instance.SetMute();

        public void SetNameDebug(string name)
            => _instance.SetNameDebug(name);

        public void SetNeverLeavesGroup(bool toggle)
            => _instance.SetNeverLeavesGroup(toggle);

        public void SetParachuteTaskTarget(float x, float y, float z)
            => _instance.SetParachuteTaskTarget(x, y, z);

        public void SetParachuteTaskThrust(float thrust)
            => _instance.SetParachuteTaskThrust(thrust);

        public void SetParachuteTintIndex(int tintIndex)
            => _instance.SetParachuteTintIndex(tintIndex);

        public void SetPathAvoidFire(bool avoidFire)
            => _instance.SetPathAvoidFire(avoidFire);

        public void SetPathCanDropFromHeight(bool Toggle)
            => _instance.SetPathCanDropFromHeight(Toggle);

        public void SetPathCanUseClimbovers(bool Toggle)
            => _instance.SetPathCanUseClimbovers(Toggle);

        public void SetPathCanUseLadders(bool Toggle)
            => _instance.SetPathCanUseLadders(Toggle);

        public void SetPathMayEnterWater(bool mayEnterWater)
            => _instance.SetPathMayEnterWater(mayEnterWater);

        public void SetPathPreferToAvoidWater(bool avoidWater)
            => _instance.SetPathPreferToAvoidWater(avoidWater);

        public void SetPathsBackToOriginal(int p1, int p2, int p3, int p4, int p5, int p6)
            => _instance.SetPathsBackToOriginal(p1, p2, p3, p4, p5, p6);

        public int SetPinnedDown(bool pinned, int i)
            => _instance.SetPinnedDown(pinned, i);

        public void SetPlaysHeadOnHornAnimWhenDiesInVehicle(bool toggle)
            => _instance.SetPlaysHeadOnHornAnimWhenDiesInVehicle(toggle);

        public void SetPopulationBudget()
            => _instance.SetPopulationBudget();

        public void SetPreferredCoverSet(int itemSet)
            => _instance.SetPreferredCoverSet(itemSet);

        public void SetPrimaryLookat(int lookAt)
            => _instance.SetPrimaryLookat(lookAt);

        public void SetPropIndex(int componentId, int drawableId, int TextureId, bool attach)
            => _instance.SetPropIndex(componentId, drawableId, TextureId, attach);

        public void SetRagdollBlockingFlags(int flags)
            => _instance.SetRagdollBlockingFlags(flags);

        public void SetRagdollForceFall()
            => _instance.SetRagdollForceFall();

        public void SetRagdollOnCollision(bool toggle)
            => _instance.SetRagdollOnCollision(toggle);

        public void SetRandomComponentVariation(bool p1)
            => _instance.SetRandomComponentVariation(p1);

        public void SetRandomProps()
            => _instance.SetRandomProps();

        public void SetRelationshipGroupDefaultHash(uint hash)
            => _instance.SetRelationshipGroupDefaultHash(hash);

        public void SetRelationshipGroupHash(uint hash)
            => _instance.SetRelationshipGroupHash(hash);

        public void SetReserveParachuteTintIndex(int p1)
            => _instance.SetReserveParachuteTintIndex(p1);

        public void SetResetFlag(int flagId, bool doReset)
            => _instance.SetResetFlag(flagId, doReset);

        public void SetScream()
            => _instance.SetScream();

        public void SetScriptedAnimSeatOffset(float p1)
            => _instance.SetScriptedAnimSeatOffset(p1);

        public void SetSeeingRange(float value)
            => _instance.SetSeeingRange(value);

        public void SetShootRate(int shootRate)
            => _instance.SetShootRate(shootRate);

        public void SetShootsAtCoord(float x, float y, float z, bool toggle)
            => _instance.SetShootsAtCoord(x, y, z, toggle);

        public void SetSphereDefensiveArea(float x, float y, float z, float radius, bool p5, bool p6)
            => _instance.SetSphereDefensiveArea(x, y, z, radius, p5, p6);

        public void SetStayInVehicleWhenJacked(bool toggle)
            => _instance.SetStayInVehicleWhenJacked(toggle);

        public void SetStealthMovement(bool p1, string action)
            => _instance.SetStealthMovement(p1, action);

        public void SetSteersAroundObjects(bool toggle)
            => _instance.SetSteersAroundObjects(toggle);

        public void SetSteersAroundPeds(bool toggle)
            => _instance.SetSteersAroundPeds(toggle);

        public void SetSteersAroundVehicles(bool toggle)
            => _instance.SetSteersAroundVehicles(toggle);

        public void SetStrafeClipset(string clipSet)
            => _instance.SetStrafeClipset(clipSet);

        public void SetSuffersCriticalHits(bool toggle)
            => _instance.SetSuffersCriticalHits(toggle);

        public void SetSweat(float sweat)
            => _instance.SetSweat(sweat);

        public void SetTalk()
            => _instance.SetTalk();

        public void SetTargetLossResponse(int responseType)
            => _instance.SetTargetLossResponse(responseType);

        public void SetTargettableVehicleDestroy(int vehicleComponent, int destroyType)
            => _instance.SetTargettableVehicleDestroy(vehicleComponent, destroyType);

        public void SetTaskVehicleChaseBehaviorFlag(int flag, bool set)
            => _instance.SetTaskVehicleChaseBehaviorFlag(flag, set);

        public void SetTaskVehicleChaseIdealPursuitDistance(float distance)
            => _instance.SetTaskVehicleChaseIdealPursuitDistance(distance);

        public void SetToInformRespectedFriends(float radius, int maxFriends)
            => _instance.SetToInformRespectedFriends(radius, maxFriends);

        public void SetToLoadCover(bool toggle)
            => _instance.SetToLoadCover(toggle);

        public bool SetToRagdoll(int time1, int time2, int ragdollType, bool p4, bool p5, bool p6)
            => _instance.SetToRagdoll(time1, time2, ragdollType, p4, p5, p6);

        public bool SetToRagdollWithFall(int time, int p2, int ragdollType, float x, float y, float z, float p7, float p8, float p9, float p10, float p11, float p12, float p13)
            => _instance.SetToRagdollWithFall(time, p2, ragdollType, x, y, z, p7, p8, p9, p10, p11, p12, p13);

        public void SetUsingActionMode(bool p1, int p2, string action)
            => _instance.SetUsingActionMode(p1, p2, action);

        public void SetVisualFieldCenterAngle(float angle)
            => _instance.SetVisualFieldCenterAngle(angle);

        public void SetVisualFieldMaxAngle(float value)
            => _instance.SetVisualFieldMaxAngle(value);

        public void SetVisualFieldMaxElevationAngle(float angle)
            => _instance.SetVisualFieldMaxElevationAngle(angle);

        public void SetVisualFieldMinAngle(float value)
            => _instance.SetVisualFieldMinAngle(value);

        public void SetVisualFieldMinElevationAngle(float angle)
            => _instance.SetVisualFieldMinElevationAngle(angle);

        public void SetVisualFieldPeripheralRange(float range)
            => _instance.SetVisualFieldPeripheralRange(range);

        public int SetWaypointRouteOffset(int p1, int p2, int p3)
            => _instance.SetWaypointRouteOffset(p1, p2, p3);

        public void SetWeaponAnimationOverride(uint animStyle)
            => _instance.SetWeaponAnimationOverride(animStyle);

        public void SetWeaponMovementClipset(string clipSet)
            => _instance.SetWeaponMovementClipset(clipSet);

        public void SetWeaponTintIndex(WeaponHash weaponHash, int tintIndex)
            => _instance.SetWeaponTintIndex((uint)weaponHash, tintIndex);

        public void SetWetnessEnabledThisFrame()
            => _instance.SetWetnessEnabledThisFrame();

        public void SetWetnessHeight(float height)
            => _instance.SetWetnessHeight(height);

        public bool SkipNextReloading()
            => _instance.SkipNextReloading();

        public void StopAnimPlayback(int p1, bool p2)
            => _instance.StopAnimPlayback(p1, p2);

        public void StopAnimTask(string animDictionary, string animationName, float p3)
            => _instance.StopAnimTask(animDictionary, animationName, p3);

        public void StopCurrentPlayingAmbientSpeech()
            => _instance.StopCurrentPlayingAmbientSpeech();

        public void StopRingtone()
            => _instance.StopRingtone();

        public void StopSpeaking(bool shaking)
            => _instance.StopSpeaking(shaking);

        public void StopWeaponFiringWhenDropped()
            => _instance.StopWeaponFiringWhenDropped();

        public void SwitchOutPlayer(int flags, int unknown)
            => _instance.SwitchOutPlayer(flags, unknown);

        public void TaskAchieveHeading(float heading, int timeout)
            => _instance.TaskAchieveHeading(heading, timeout);

        public void TaskAimGunAtCoord(float x, float y, float z, int time, bool p5, bool p6)
            => _instance.TaskAimGunAtCoord(x, y, z, time, p5, p6);

        public void TaskAimGunAtEntity(int entity, int duration, bool p3)
            => _instance.TaskAimGunAtEntity(entity, duration, p3);

        public void TaskAimGunScripted(uint scriptTask, bool p2, bool p3)
            => _instance.TaskAimGunScripted(scriptTask, p2, p3);

        public void TaskArrest(int target)
            => _instance.TaskArrest(target);

        public void TaskChatTo(int target, int p2, float p3, float p4, float p5, float p6, float p7)
            => _instance.TaskChatTo(target, p2, p3, p4, p5, p6, p7);

        public void TaskClearLookAt()
            => _instance.TaskClearLookAt();

        public void TaskClimb(bool unused)
            => _instance.TaskClimb(unused);

        public void TaskClimbLadder(int p1)
            => _instance.TaskClimbLadder(p1);

        public void TaskCombat(int targetPed, int p2, int p3)
            => _instance.TaskCombat(targetPed, p2, p3);

        public void TaskCombatHatedTargetsAround(float radius, int p2)
            => _instance.TaskCombatHatedTargetsAround(radius, p2);

        public void TaskCombatHatedTargetsInArea(float x, float y, float z, float radius, int p5)
            => _instance.TaskCombatHatedTargetsInArea(x, y, z, radius, p5);

        public void TaskCower(int duration)
            => _instance.TaskCower(duration);

        public void TaskEnterVehicle(int vehicle, int timeout, int seat, float speed, int p5, int p6)
            => _instance.TaskEnterVehicle(vehicle, timeout, seat, speed, p5, p6);

        public void TaskFollowNavMeshToCoord(float x, float y, float z, float speed, int timeout, float stoppingRange, bool persistFollowing, float unk)
            => _instance.TaskFollowNavMeshToCoord(x, y, z, speed, timeout, stoppingRange, persistFollowing, unk);

        public void TaskFollowNavMeshToCoordAdvanced(float x, float y, float z, float speed, int timeout, float unkFloat, int unkInt, float unkX, float unkY, float unkZ, float unk_40000f)
            => _instance.TaskFollowNavMeshToCoordAdvanced(x, y, z, speed, timeout, unkFloat, unkInt, unkX, unkY, unkZ, unk_40000f);

        public void TaskFollowPointRoute(float speed, int unknown)
            => _instance.TaskFollowPointRoute(speed, unknown);

        public void TaskFollowToOffsetOfEntity(int entity, float offsetX, float offsetY, float offsetZ, float movementSpeed, int timeout, float stoppingRange, bool persistFollowing)
            => _instance.TaskFollowToOffsetOfEntity(entity, offsetX, offsetY, offsetZ, movementSpeed, timeout, stoppingRange, persistFollowing);

        public void TaskForceMotionState(MotionState state, bool p2)
            => _instance.TaskForceMotionState((uint)state, p2);

        public void TaskGetOffBoat(int boat)
            => _instance.TaskGetOffBoat(boat);

        public void TaskGoStraightToCoord(float x, float y, float z, float speed, int timeout, float targetHeading, float distanceToSlide)
            => _instance.TaskGoStraightToCoord(x, y, z, speed, timeout, targetHeading, distanceToSlide);

        public void TaskGoTo(int target, int duration, float distance, float speed, float p5, int p6)
            => _instance.TaskGoTo(target, duration, distance, speed, p5, p6);

        public void TaskGoToCoordAnyMeans(float x, float y, float z, float speed, int p5, bool p6, int walkingStyle, float p8)
            => _instance.TaskGoToCoordAnyMeans(x, y, z, speed, p5, p6, walkingStyle, p8);

        public void TaskGoToCoordAnyMeansExtraParams(float x, float y, float z, float speed, int p5, bool p6, int walkingStyle, float p8, int p9, int p10, int p11, int p12)
            => _instance.TaskGoToCoordAnyMeansExtraParams(x, y, z, speed, p5, p6, walkingStyle, p8, p9, p10, p11, p12);

        public void TaskGoToCoordAnyMeansExtraParamsWithCruiseSpeed(float x, float y, float z, float speed, int p5, bool p6, int walkingStyle, float p8, int p9, int p10, int p11, int p12, int p13)
            => _instance.TaskGoToCoordAnyMeansExtraParamsWithCruiseSpeed(x, y, z, speed, p5, p6, walkingStyle, p8, p9, p10, p11, p12, p13);

        public void TaskGoToCoordWhileAimingAtCoord(float x, float y, float z, float aimAtX, float aimAtY, float aimAtZ, float moveSpeed, bool p8, float p9, float p10, bool p11, int flags, bool p13, uint firingPattern)
            => _instance.TaskGoToCoordWhileAimingAtCoord(x, y, z, aimAtX, aimAtY, aimAtZ, moveSpeed, p8, p9, p10, p11, flags, p13, firingPattern);

        public void TaskGotoEntityAiming(int target, float distanceToStopAt, float startAimingDist)
            => _instance.TaskGotoEntityAiming(target, distanceToStopAt, startAimingDist);

        public void TaskGotoEntityOffset(int p1, int p2, float x, float y, float z, int duration)
            => _instance.TaskGotoEntityOffset(p1, p2, x, y, z, duration);

        public void TaskGoToEntityWhileAimingAtEntity(int entityToWalkTo, int entityToAimAt, float speed, bool shootatEntity, float p5, float p6, bool p7, bool p8, uint firingPattern)
            => _instance.TaskGoToEntityWhileAimingAtEntity(entityToWalkTo, entityToAimAt, speed, shootatEntity, p5, p6, p7, p8, firingPattern);

        public void TaskHandsUp(int duration, int facingPed, int p3, bool p4)
            => _instance.TaskHandsUp(duration, facingPed, p3, p4);

        public void TaskJump(bool unused, int p2, int p3)
            => _instance.TaskJump(unused, p2, p3);

        public void TaskLeaveAnyVehicle(int p1, int p2)
            => _instance.TaskLeaveAnyVehicle(p1, p2);

        public void TaskLeaveVehicle(int vehicle, int flags)
            => _instance.TaskLeaveVehicle(vehicle, flags);

        public void TaskLookAtCoord(float x, float y, float z, float duration, int p5, int p6)
            => _instance.TaskLookAtCoord(x, y, z, duration, p5, p6);

        public void TaskLookAtEntity(int lookAt, int duration, int unknown1, int unknown2)
            => _instance.TaskLookAtEntity(lookAt, duration, unknown1, unknown2);

        public void TaskMoveNetwork(string task, float multiplier, bool p3, string animDict, int flags)
            => _instance.TaskMoveNetwork(task, multiplier, p3, animDict, flags);

        public void TaskMoveNetworkAdvanced(string p1, float p2, float p3, float p4, float p5, float p6, float p7, int p8, float p9, bool p10, string animDict, int flags)
            => _instance.TaskMoveNetworkAdvanced(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, animDict, flags);

        public void TaskOpenVehicleDoor(int vehicle, int timeOut, int doorIndex, float speed)
            => _instance.TaskOpenVehicleDoor(vehicle, timeOut, doorIndex, speed);

        public void TaskParachute(bool p1, int p2)
            => _instance.TaskParachute(p1, p2);

        public void TaskParachuteToTarget(float x, float y, float z)
            => _instance.TaskParachuteToTarget(x, y, z);

        public void TaskPatrol(string p1, int p2, bool p3, bool p4)
            => _instance.TaskPatrol(p1, p2, p3, p4);

        public void TaskPause(int ms)
            => _instance.TaskPause(ms);

        public void TaskPerformSequence(int taskSequence)
            => _instance.TaskPerformSequence(taskSequence);

        public void TaskPlantBomb(float x, float y, float z, float heading)
            => _instance.TaskPlantBomb(x, y, z, heading);

        public void TaskPlayAnim(string animDict, string animName, float speed, float speedMultiplier, int duration, int flat, int playbackRate, bool lockX, bool lockY, bool lockZ)
            => _instance.TaskPlayAnim(animDict, animName, speed, speedMultiplier, duration, flat, playbackRate, lockX, lockY, lockZ);

        public void TaskPlayAnimAdvanced(string animDict, string animName, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, float speed, float speedMultiplier, int duration, int flag, float animTime, int p14, int p15)
            => _instance.TaskPlayAnimAdvanced(animDict, animName, posX, posY, posZ, rotX, rotY, rotZ, speed, speedMultiplier, duration, flag, animTime, p14, p15);

        public void TaskPlayPhoneGestureAnimation(string animDict, string animation, string boneMaskType, float p4, float p5, bool p6, bool p7)
            => _instance.TaskPlayPhoneGestureAnimation(animDict, animation, boneMaskType, p4, p5, p6, p7);

        public void TaskPutDirectlyIntoCover(float x, float y, float z, int timeout, bool p5, float p6, bool p7, bool p8, int p9, bool p10)
            => _instance.TaskPutDirectlyIntoCover(x, y, z, timeout, p5, p6, p7, p8, p9, p10);

        public void TaskPutDirectlyIntoMelee(int meleeTarget, float p2, float p3, float p4, bool p5)
            => _instance.TaskPutDirectlyIntoMelee(meleeTarget, p2, p3, p4, p5);

        public void TaskRappelFromHeli(int unused)
            => _instance.TaskRappelFromHeli(unused);

        public void TaskReactAndFlee(int fleeTarget)
            => _instance.TaskReactAndFlee(fleeTarget);

        public void TaskReloadWeapon(bool unused)
            => _instance.TaskReloadWeapon(unused);

        public void TaskScriptedAnimation(ref int p1, ref int p2, ref int p3, float p4, float p5)
            => _instance.TaskScriptedAnimation(ref p1, ref p2, ref p3, p4, p5);

        public void TaskSeekCoverFrom(int target, int duration, bool p3)
            => _instance.TaskSeekCoverFrom(target, duration, p3);

        public void TaskSeekCoverFromPos(float x, float y, float z, int duration, bool p5)
            => _instance.TaskSeekCoverFromPos(x, y, z, duration, p5);

        public void TaskSeekCoverToCoords(float x1, float y1, float z1, float x2, float y2, float z2, int p7, bool p8)
            => _instance.TaskSeekCoverToCoords(x1, y1, z1, x2, y2, z2, p7, p8);

        public void TaskSetBlockingOfNonTemporaryEvents(bool toggle)
            => _instance.TaskSetBlockingOfNonTemporaryEvents(toggle);

        public void TaskShockingEventReact(int eventHandle)
            => _instance.TaskShockingEventReact(eventHandle);

        public void TaskShootAt(int target, int duration, uint firingPattern)
            => _instance.TaskShootAt(target, duration, firingPattern);

        public void TaskShootAtCoord(float x, float y, float z, int duration, uint firingPattern)
            => _instance.TaskShootAtCoord(x, y, z, duration, firingPattern);

        public void TaskShuffleToNextVehicleSeat(int vehicle, int p2)
            => _instance.TaskShuffleToNextVehicleSeat(vehicle, p2);

        public void TaskSkyDive(int p1)
            => _instance.TaskSkyDive(p1);

        public void TaskSlideToCoord(float x, float y, float z, float heading, float p5)
            => _instance.TaskSlideToCoord(x, y, z, heading, p5);

        public void TaskSlideToCoordHdgRate(float x, float y, float z, float heading, float p5, float p6)
            => _instance.TaskSlideToCoordHdgRate(x, y, z, heading, p5, p6);

        public void TaskSmartFlee(int fleeTarget, float distance, int fleeTime, bool p4, bool p5)
            => _instance.TaskSmartFlee(fleeTarget, distance, fleeTime, p4, p5);

        public void TaskSmartFleeCoord(float x, float y, float z, float distance, int time, bool p6, bool p7)
            => _instance.TaskSmartFleeCoord(x, y, z, distance, time, p6, p7);

        public void TaskStandGuard(float x, float y, float z, float heading, string scenarioName)
            => _instance.TaskStandGuard(x, y, z, heading, scenarioName);

        public void TaskStandStill(int time)
            => _instance.TaskStandStill(time);

        public void TaskStartScenarioAtPosition(string scenarioName, float x, float y, float z, float heading, int duration, bool sittingScenario, bool teleport)
            => _instance.TaskStartScenarioAtPosition(scenarioName, x, y, z, heading, duration, sittingScenario, teleport);

        public void TaskStartScenarioInPlace(string scenarioName, int unkDelay, bool playEnterAnim)
            => _instance.TaskStartScenarioInPlace(scenarioName, unkDelay, playEnterAnim);

        public void TaskStayInCover()
            => _instance.TaskStayInCover();

        public void TaskStopPhoneGestureAnimation(int p1)
            => _instance.TaskStopPhoneGestureAnimation(p1);

        public void TaskSwapWeapon(bool p1)
            => _instance.TaskSwapWeapon(p1);

        public void TaskSweepAimEntity(string anim, string p2, string p3, string p4, int p5, int vehicle, float p7, float p8)
            => _instance.TaskSweepAimEntity(anim, p2, p3, p4, p5, vehicle, p7, p8);

        public void TaskSynchronizedScene(int scene, string animDictionary, string animationName, float speed, float speedMultiplier, int duration, int flag, float playbackRate, int p9)
            => _instance.TaskSynchronizedScene(scene, animDictionary, animationName, speed, speedMultiplier, duration, flag, playbackRate, p9);

        public void TaskThrowProjectile(float x, float y, float z, int p4, int p5)
            => _instance.TaskThrowProjectile(x, y, z, p4, p5);

        public void TaskTurnToFaceCoord(float x, float y, float z, int duration)
            => _instance.TaskTurnToFaceCoord(x, y, z, duration);

        public void TaskTurnToFaceEntity(int entity, int duration)
            => _instance.TaskTurnToFaceEntity(entity, duration);

        public void TaskUseMobilePhone(int p1, int p2)
            => _instance.TaskUseMobilePhone(p1, p2);

        public void TaskUseMobilePhoneTimed(int duration)
            => _instance.TaskUseMobilePhoneTimed(duration);

        public void TaskUseNearestScenarioToCoord(float x, float y, float z, float distance, int duration)
            => _instance.TaskUseNearestScenarioToCoord(x, y, z, distance, duration);

        public void TaskUseNearestScenarioToCoordWarp(float x, float y, float z, float radius, int p5)
            => _instance.TaskUseNearestScenarioToCoordWarp(x, y, z, radius, p5);

        public void TaskVehicleAimAt(int target)
            => _instance.TaskVehicleAimAt(target);

        public void TaskVehicleAimAtCoord(float x, float y, float z)
            => _instance.TaskVehicleAimAtCoord(x, y, z);

        public void TaskVehicleDriveToCoord(int vehicle, float x, float y, float z, float speed, int p6, uint vehicleModel, int drivingMode, float stopRange, float p10)
            => _instance.TaskVehicleDriveToCoord(vehicle, x, y, z, speed, p6, vehicleModel, drivingMode, stopRange, p10);

        public void TaskVehicleDriveToCoordLongrange(int vehicle, float x, float y, float z, float speed, int driveMode, float stopRange)
            => _instance.TaskVehicleDriveToCoordLongrange(vehicle, x, y, z, speed, driveMode, stopRange);

        public void TaskVehicleDriveWander(int vehicle, float speed, int drivingStyle)
            => _instance.TaskVehicleDriveWander(vehicle, speed, drivingStyle);

        public void TaskVehicleEscort(int vehicle, int targetVehicle, int mode, float speed, int drivingStyle, float minDistance, int p7, float noRoadsDistance)
            => _instance.TaskVehicleEscort(vehicle, targetVehicle, mode, speed, drivingStyle, minDistance, p7, noRoadsDistance);

        public void TaskVehicleFollowWaypointRecording(int vehicle, string WPRecording, int p3, int p4, int p5, int p6, float p7, bool p8, float p9)
            => _instance.TaskVehicleFollowWaypointRecording(vehicle, WPRecording, p3, p4, p5, p6, p7, p8, p9);

        public void TaskVehicleGotoNavmesh(int vehicle, float x, float y, float z, float speed, int behaviorFlag, float stoppingRange)
            => _instance.TaskVehicleGotoNavmesh(vehicle, x, y, z, speed, behaviorFlag, stoppingRange);

        public void TaskVehicleMissionCoorsTarget(int vehicle, float x, float y, float z, int p5, int p6, int p7, float p8, float p9, bool p10)
            => _instance.TaskVehicleMissionCoorsTarget(vehicle, x, y, z, p5, p6, p7, p8, p9, p10);

        public void TaskVehicleMissionTarget(int vehicle, int pedTarget, int mode, float maxSpeed, int drivingStyle, float minDistance, float p7, bool p8)
            => _instance.TaskVehicleMissionTarget(vehicle, pedTarget, mode, maxSpeed, drivingStyle, minDistance, p7, p8);

        public void TaskVehiclePark(int vehicle, float x, float y, float z, float heading, int mode, float radius, bool keepEngineOn)
            => _instance.TaskVehiclePark(vehicle, x, y, z, heading, mode, radius, keepEngineOn);

        public void TaskVehicleShootAt(int target, float p2)
            => _instance.TaskVehicleShootAt(target, p2);

        public void TaskVehicleShootAtCoord(float x, float y, float z, float p4)
            => _instance.TaskVehicleShootAtCoord(x, y, z, p4);

        public void TaskWanderInArea(float x, float y, float z, float radius, float minimalLength, float timeBetweenWalks)
            => _instance.TaskWanderInArea(x, y, z, radius, minimalLength, timeBetweenWalks);

        public void TaskWanderStandard(float p1, int p2)
            => _instance.TaskWanderStandard(p1, p2);

        public void TaskWarpIntoVehicle(int vehicle, int seat)
            => _instance.TaskWarpIntoVehicle(vehicle, seat);

        public void TaskWrithe(int target, int time, int p3, int p4, int p5)
            => _instance.TaskWrithe(target, time, p3, p4, p5);

        public int ToNet()
            => _instance.ToNet();

        public void Uncuff()
            => _instance.Uncuff();

        public void UpdateHeadBlendData(float shapeMix, float skinMix, float thirdMix)
            => _instance.UpdateHeadBlendData(shapeMix, skinMix, thirdMix);

        public void UpdateTaskHandsUpDuration(int duration)
            => _instance.UpdateTaskHandsUpDuration(duration);

        public void UpdateTaskSweepAimEntity(int entity)
            => _instance.UpdateTaskSweepAimEntity(entity);

        public bool WasKilledByStealth()
            => _instance.WasKilledByStealth();

        public bool WasKilledByTakedown()
            => _instance.WasKilledByTakedown();

        public bool WasSkeletonUpdated()
            => _instance.WasSkeletonUpdated();

        #endregion Public Methods
    }
}
