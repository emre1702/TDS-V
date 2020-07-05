using System.Drawing;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Player
{
    internal class Player : RAGE.Elements.Player, IPlayer
    {
        #region Public Constructors

        public Player(ushort id, ushort remoteId) : base(id, remoteId)
        {
        }

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
            get => base.Position.ToPosition3D();
            set => base.Position = value.ToVector3();
        }

        public Position3D Rotation
        {
            get => GetRotation(2);
            set => SetRotation(value.X, value.Y, value.Z, 2, true);
        }

        public new EntityType Type => (EntityType)base.Type;

        public new IVehicle Vehicle => base.Vehicle as IVehicle;

        #endregion Public Properties

        #region Public Methods

        public void AddAmmoTo(WeaponHash weaponHash, int ammo)
            => AddAmmoTo((uint)weaponHash, ammo);

        public void AddOwnedExplosion(float x, float y, float z, ExplosionType explosionType, float damageScale, bool isAudible, bool isInvisible, float cameraShake)
            => AddOwnedExplosion(x, y, z, (int)explosionType, damageScale, isAudible, isInvisible, cameraShake);

        public bool AreFlashingStarsAboutToDrop()
            => RAGE.Game.Player.ArePlayerFlashingStarsAboutToDrop();

        public bool AreStarsGreyedOut()
        => RAGE.Game.Player.ArePlayerStarsGreyedOut();

        public void AssistedMovementCloseRoute()
            => RAGE.Game.Player.AssistedMovementCloseRoute();

        public void AssistedMovementFlushRoute()
            => RAGE.Game.Player.AssistedMovementFlushRoute();

        public void AttachVirtualBound(float p0, float p1, float p2, float p3, float p4, float p5, float p6, float p7)
            => RAGE.Game.Player.PlayerAttachVirtualBound(p0, p1, p2, p3, p4, p5, p6, p7);

        public bool CanStartMission()
            => RAGE.Game.Player.CanPlayerStartMission();

        public void ChangePed(int ped, bool b2, bool b3)
            => RAGE.Game.Player.ChangePlayerPed(ped, b2, b3);

        public void ClearHasDamagedAtLeastOneNonAnimalPed()
            => RAGE.Game.Player.ClearPlayerHasDamagedAtLeastOneNonAnimalPed();

        public void ClearHasDamagedAtLeastOnePed()
            => RAGE.Game.Player.ClearPlayerHasDamagedAtLeastOnePed();

        public void ClearParachuteModelOverride()
            => RAGE.Game.Player.ClearPlayerParachuteModelOverride();

        public void ClearParachutePackModelOverride()
            => RAGE.Game.Player.ClearPlayerParachutePackModelOverride();

        public void ClearParachuteVariationOverride()
            => RAGE.Game.Player.ClearPlayerParachuteVariationOverride();

        public void ClearWantedLevel()
            => RAGE.Game.Player.ClearPlayerWantedLevel();

        public void DetachVirtualBound()
            => RAGE.Game.Player.PlayerDetachVirtualBound();

        public void DisableFiring(bool toggle)
            => RAGE.Game.Player.DisablePlayerFiring(toggle);

        public void DisableVehicleRewards()
            => RAGE.Game.Player.DisablePlayerVehicleRewards();

        public void DisplaySystemSigninUi(bool unk)
            => RAGE.Game.Player.DisplaySystemSigninUi(unk);

        public void EnableSpecialAbility(bool toggle)
            => RAGE.Game.Player.EnableSpecialAbility(toggle);

        public bool Equals(IEntity other)
        {
            return Id == other?.Id;
        }

        public void ExpandWorldLimits(float x, float y, float z)
            => RAGE.Game.Player.ExpandWorldLimits(x, y, z);

        public void ExplodeHead(WeaponHash weaponHash)
                    => ExplodeHead((uint)weaponHash);

        public void ExplodeProjectiles(WeaponHash weaponHash, bool p2)
            => ExplodeProjectiles((uint)weaponHash, p2);

        public void ForceCleanup(int cleanupFlags)
            => RAGE.Game.Player.ForceCleanup(cleanupFlags);

        public void ForceCleanupForAllThreadsWithThisName(string name, int cleanupFlags)
            => RAGE.Game.Player.ForceCleanupForAllThreadsWithThisName(name, cleanupFlags);

        public void ForceCleanupForThreadWithThisId(int id, int cleanupFlags)
            => RAGE.Game.Player.ForceCleanupForThreadWithThisId(id, cleanupFlags);

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

        public int GetCauseOfMostRecentForceCleanup()
            => RAGE.Game.Player.GetCauseOfMostRecentForceCleanup();

        public new Position3D GetCollisionNormalOfLastHitFor()
                    => base.GetCollisionNormalOfLastHitFor().ToPosition3D();

        public new Position3D GetCoords(bool alive)
            => base.GetCoords(alive).ToPosition3D();

        public float GetCurrentStealthNoise()
            => RAGE.Game.Player.GetPlayerCurrentStealthNoise();

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

        public int GetGroup()
            => RAGE.Game.Player.GetPlayerGroup();

        public bool GetHasReserveParachute()
            => RAGE.Game.Player.GetPlayerHasReserveParachute();

        public int? GetHeadBlendData()
        {
            int headBlendData = 0;
            if (!GetHeadBlendData(ref headBlendData))
                return null;
            return headBlendData;
        }

        public int GetIndex()
            => RAGE.Game.Player.GetPlayerIndex();

        public bool GetInvincible()
            => RAGE.Game.Player.GetPlayerInvincible();

        public bool GetIsFreeAimingAt(ref int entity)
            => RAGE.Game.Player.GetEntityPlayerIsFreeAimingAt(ref entity);

        public PedBone? GetLastDamageBone()
        {
            int outBone = 0;
            if (!GetLastDamageBone(ref outBone))
                return null;

            return (PedBone)outBone;
        }

        public int GetLastVehicle()
            => RAGE.Game.Player.GetPlayersLastVehicle();

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

        public int GetMaxArmour()
            => RAGE.Game.Player.GetPlayerMaxArmour();

        public int GetMaxWantedLevel()
            => RAGE.Game.Player.GetMaxWantedLevel();

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

        public string GetName()
            => RAGE.Game.Player.GetPlayerName();

        public new Position3D GetOffsetFromGivenWorldCoords(float posX, float posY, float posZ)
                    => base.GetOffsetFromGivenWorldCoords(posX, posY, posZ).ToPosition3D();

        public new Position3D GetOffsetFromInWorldCoords(float offsetX, float offsetY, float offsetZ)
            => base.GetOffsetFromInWorldCoords(offsetX, offsetY, offsetZ).ToPosition3D();

        public Position3D GetOffsetInWorldCoords(float offsetX, float offsetY, float offsetZ)
            => RAGE.Game.Entity.GetOffsetFromEntityInWorldCoords(Handle, offsetX, offsetY, offsetZ).ToPosition3D();

        public void GetParachutePackTintIndex(ref int tintIndex)
            => RAGE.Game.Player.GetPlayerParachutePackTintIndex(ref tintIndex);

        public void GetParachuteSmokeTrailColor(ref int r, ref int g, ref int b)
            => RAGE.Game.Player.GetPlayerParachuteSmokeTrailColor(ref r, ref g, ref b);

        public int GetParachuteTintIndex()
        {
            int tintIndex = 0;
            GetParachuteTintIndex(ref tintIndex);
            return tintIndex;
        }

        public int GetPed()
            => RAGE.Game.Player.GetPlayerPed();

        public int GetPedScriptIndex()
            => RAGE.Game.Player.GetPlayerPedScriptIndex();

        public int GetRagdollBoneIndex(PedBone bone)
            => GetRagdollBoneIndex((int)bone);

        public void GetReserveParachuteTintIndex(ref int index)
            => RAGE.Game.Player.GetPlayerReserveParachuteTintIndex(ref index);

        public void GetRgbColour(ref int r, ref int g, ref int b)
            => RAGE.Game.Player.GetPlayerRgbColour(ref r, ref g, ref b);

        public new Position3D GetRotation(int rotationOrder)
            => base.GetRotation(rotationOrder).ToPosition3D();

        public new Position3D GetRotationVelocity()
            => base.GetRotationVelocity().ToPosition3D();

        public new WeaponHash GetSelectedWeapon()
            => (WeaponHash)base.GetSelectedWeapon();

        public new Position3D GetSpeedVector(bool relative)
            => base.GetSpeedVector(relative).ToPosition3D();

        public float GetSprintStaminaRemaining()
            => RAGE.Game.Player.GetPlayerSprintStaminaRemaining();

        public float GetSprintTimeRemaining()
            => RAGE.Game.Player.GetPlayerSprintTimeRemaining();

        public bool GetTargetEntity(ref int entity)
            => RAGE.Game.Player.GetPlayerTargetEntity(ref entity);

        public int GetTeam()
            => RAGE.Game.Player.GetPlayerTeam();

        public int GetTimeSinceDroveAgainstTraffic()
            => RAGE.Game.Player.GetTimeSincePlayerDroveAgainstTraffic();

        public int GetTimeSinceDroveOnPavement()
            => RAGE.Game.Player.GetTimeSincePlayerDroveOnPavement();

        public int GetTimeSinceHitPed()
            => RAGE.Game.Player.GetTimeSincePlayerHitPed();

        public int GetTimeSinceHitVehicle()
            => RAGE.Game.Player.GetTimeSincePlayerHitVehicle();

        public int GetTimeSinceLastArrest()
            => RAGE.Game.Player.GetTimeSinceLastArrest();

        public int GetTimeSinceLastDeath()
            => RAGE.Game.Player.GetTimeSinceLastDeath();

        public float GetUnderwaterTimeRemaining()
            => RAGE.Game.Player.GetPlayerUnderwaterTimeRemaining();

        public new Position3D GetVelocity()
                                                                                                    => base.GetVelocity().ToPosition3D();

        public Position3D GetWantedCentrePosition()
            => RAGE.Game.Player.GetPlayerWantedCentrePosition().ToPosition3D();

        public int GetWantedLevel()
            => RAGE.Game.Player.GetPlayerWantedLevel();

        public float GetWantedLevelRadius()
            => RAGE.Game.Player.GetWantedLevelRadius();

        public int GetWantedLevelThreshold(int wantedLevel)
            => RAGE.Game.Player.GetWantedLevelThreshold(wantedLevel);

        public int GetWeaponTintIndex(WeaponHash weaponHash)
                                            => GetWeaponTintIndex((uint)weaponHash);

        public new Position3D GetWorldPositionOfBone(int boneIndex)
            => base.GetWorldPositionOfBone(boneIndex).ToPosition3D();

        public bool GiveAchievement(int achievement)
            => RAGE.Game.Player.GiveAchievementToPlayer(achievement);

        public void GiveDelayedWeaponTo(WeaponHash weaponHash, int time, bool equipNow)
                    => GiveDelayedWeaponTo((uint)weaponHash, time, equipNow);

        public void GiveRagdollControl(bool toggle)
            => RAGE.Game.Player.GivePlayerRagdollControl(toggle);

        public void GiveWeaponComponentTo(WeaponHash weaponHash, uint componentHash)
                    => GiveWeaponComponentTo((uint)weaponHash, componentHash);

        public void GiveWeaponTo(WeaponHash weaponHash, int ammoCount, bool isHidden, bool equipNow)
            => GiveWeaponTo((uint)weaponHash, ammoCount, isHidden, equipNow);

        public bool HasAchievementBeenPassed(int achievement)
            => RAGE.Game.Player.HasAchievementBeenPassed(achievement);

        public bool HasBeenDamagedByWeapon(WeaponHash weaponHash, int weaponType)
                    => HasBeenDamagedByWeapon((uint)weaponHash, weaponType);

        public bool HasBeenSpottedInStolenVehicle()
            => RAGE.Game.Player.HasPlayerBeenSpottedInStolenVehicle();

        public bool HasDamagedAtLeastOneNonAnimalPed()
            => RAGE.Game.Player.HasPlayerDamagedAtLeastOneNonAnimalPed();

        public bool HasDamagedAtLeastOnePed()
            => RAGE.Game.Player.HasPlayerDamagedAtLeastOnePed();

        public bool HasForceCleanupOccurred(int cleanupFlags)
            => RAGE.Game.Player.HasForceCleanupOccurred(cleanupFlags);

        public bool HasGotWeapon(WeaponHash weaponHash, bool p2)
                                            => HasGotWeapon((uint)weaponHash, p2);

        public bool HasGotWeaponComponent(WeaponHash weaponHash, uint componentHash)
            => HasGotWeaponComponent((uint)weaponHash, componentHash);

        public bool HasLeftTheWorld()
            => RAGE.Game.Player.HasPlayerLeftTheWorld();

        public bool HasTeleportFinished()
            => RAGE.Game.Player.HasPlayerTeleportFinished();

        public int IntToParticipantindex(int value)
            => RAGE.Game.Player.IntToParticipantindex(value);

        public int IntToPlayerindex(int value)
            => RAGE.Game.Player.IntToPlayerindex(value);

        public bool IsBeingArrested(bool atArresting)
            => RAGE.Game.Player.IsPlayerBeingArrested(atArresting);

        public bool IsCamControlDisabled()
            => RAGE.Game.Player.IsPlayerCamControlDisabled();

        public new bool IsClimbing()
            => RAGE.Game.Player.IsPlayerClimbing();

        public bool IsControlOn()
            => RAGE.Game.Player.IsPlayerControlOn();

        public bool IsDead()
            => RAGE.Game.Player.IsPlayerDead();

        public bool IsDeadOrDying()
                                                                                    => IsDeadOrDying(true);

        public bool IsFreeAiming()
            => RAGE.Game.Player.IsPlayerFreeAiming();

        public bool IsFreeAimingAtEntity(int entity)
            => RAGE.Game.Player.IsPlayerFreeAimingAtEntity(entity);

        public bool IsFreeForAmbientTask()
            => RAGE.Game.Player.IsPlayerFreeForAmbientTask();

        public bool IsHeadshotReady()
                                    => IsheadshotReady();

        public bool IsHeadshotValid()
            => IsheadshotValid();

        public bool IsLoggingInNp()
            => RAGE.Game.Player.IsPlayerLoggingInNp();

        public bool IsPlaying()
            => RAGE.Game.Player.IsPlayerPlaying();

        public bool IsPlayingAnim(string animDict, string animName)
                            => IsPlayingAnim(animDict, animName, 3);

        public bool IsPressingHorn()
            => RAGE.Game.Player.IsPlayerPressingHorn();

        public bool IsReadyForCutscene()
            => RAGE.Game.Player.IsPlayerReadyForCutscene();

        public bool IsRidingTrain()
            => RAGE.Game.Player.IsPlayerRidingTrain();

        public bool IsScriptControlOn()
            => RAGE.Game.Player.IsPlayerScriptControlOn();

        public bool IsSpecialAbilityActive()
            => RAGE.Game.Player.IsSpecialAbilityActive();

        public bool IsSpecialAbilityEnabled()
            => RAGE.Game.Player.IsSpecialAbilityEnabled();

        public bool IsSpecialAbilityMeterFull()
            => RAGE.Game.Player.IsSpecialAbilityMeterFull();

        public bool IsSpecialAbilityUnlocked(uint playerModel)
            => RAGE.Game.Player.IsSpecialAbilityUnlocked(playerModel);

        public bool IsSystemUiBeingDisplayed()
            => RAGE.Game.Player.IsSystemUiBeingDisplayed();

        public bool IsTargettingAnything()
            => RAGE.Game.Player.IsPlayerTargettingAnything();

        public bool IsTargettingEntity(int entity)
            => RAGE.Game.Player.IsPlayerTargettingEntity(entity);

        public bool IsTeleportActive()
            => RAGE.Game.Player.IsPlayerTeleportActive();

        public bool IsWantedLevelGreater(int wantedLevel)
            => RAGE.Game.Player.IsPlayerWantedLevelGreater(wantedLevel);

        public bool IsWeaponComponentActive(WeaponHash weaponHash, uint componentHash)
                                                                                                                    => IsWeaponComponentActive((uint)weaponHash, componentHash);

        public int NetworkPlayerIdToInt()
            => RAGE.Game.Player.NetworkPlayerIdToInt();

        public int PlayerId()
            => RAGE.Game.Player.PlayerId();

        public int PlayerPedId()
            => RAGE.Game.Player.PlayerPedId();

        public int RegisterHeadshot()
                                    => Registerheadshot();

        public new void RemoveHelmet(bool p2)
            => RAGE.Game.Player.RemovePlayerHelmet(p2);

        public void RemoveWeaponComponentFrom(WeaponHash weaponHash, uint componentHash)
                    => RemoveWeaponComponentFrom((uint)weaponHash, componentHash);

        public void RemoveWeaponFrom(WeaponHash weaponHash)
            => RemoveWeaponFrom((uint)weaponHash);

        public void ReportCrime(int crimeType, int wantedLvlThresh)
            => RAGE.Game.Player.ReportCrime(crimeType, wantedLvlThresh);

        public void ResetArrestState()
            => RAGE.Game.Player.ResetPlayerArrestState();

        public void ResetInputGait()
            => RAGE.Game.Player.ResetPlayerInputGait();

        public void ResetStamina()
            => RAGE.Game.Player.ResetPlayerStamina();

        public void ResetWantedLevelDifficulty()
            => RAGE.Game.Player.ResetWantedLevelDifficulty();

        public void RestoreStamina(float p1)
            => RAGE.Game.Player.RestorePlayerStamina(p1);

        public void SetAirDragMultiplierForVehicle(float multiplier)
            => RAGE.Game.Player.SetAirDragMultiplierForPlayersVehicle(multiplier);

        public void SetAllRandomPedsFlee(bool toggle)
            => RAGE.Game.Player.SetAllRandomPedsFlee(toggle);

        public void SetAllRandomPedsFleeThisFrame()
            => RAGE.Game.Player.SetAllRandomPedsFleeThisFrame();

        public void SetAmmo(WeaponHash weaponHash, int ammo, int p3)
                                                                                   => SetAmmo((uint)weaponHash, ammo, p3);

        public bool SetAmmoInClip(WeaponHash weaponHash, int ammo)
            => SetAmmoInClip((uint)weaponHash, ammo);

        public void SetAutoGiveParachuteWhenEnterPlane(bool toggle)
            => RAGE.Game.Player.SetAutoGiveParachuteWhenEnterPlane(toggle);

        public void SetCanAttackFriendly(bool toggle)
                    => SetCanAttackFriendly(toggle, false);

        public void SetCanBeHassledByGangs(bool toggle)
            => RAGE.Game.Player.SetPlayerCanBeHassledByGangs(toggle);

        public void SetCanDoDriveBy(bool toggle)
            => RAGE.Game.Player.SetPlayerCanDoDriveBy(toggle);

        public void SetCanLeaveParachuteSmokeTrail(bool enabled)
            => RAGE.Game.Player.SetPlayerCanLeaveParachuteSmokeTrail(enabled);

        public void SetCanUseCover(bool toggle)
            => RAGE.Game.Player.SetPlayerCanUseCover(toggle);

        public void SetClothLockCounter(int value)
            => RAGE.Game.Player.SetPlayerClothLockCounter(value);

        public void SetClothPackageIndex(int index)
            => RAGE.Game.Player.SetPlayerClothPackageIndex(index);

        public void SetClothPinFrames(bool toggle)
            => RAGE.Game.Player.SetPlayerClothPinFrames(toggle);

        public void SetControl(bool toggle, int possiblyFlags)
            => RAGE.Game.Player.SetPlayerControl(toggle, possiblyFlags);

        public bool SetCurrentVehicleWeapon(WeaponHash weaponHash)
                                                                            => SetCurrentVehicleWeapon((uint)weaponHash);

        public void SetCurrentWeapon(WeaponHash weaponHash, bool equipNow)
           => SetCurrentWeapon((uint)weaponHash, equipNow);

        public void SetDisableAmbientMeleeMove(bool toggle)
            => RAGE.Game.Player.SetDisableAmbientMeleeMove(toggle);

        public void SetDispatchCops(bool toggle)
            => RAGE.Game.Player.SetDispatchCopsForPlayer(toggle);

        public void SetDropsInventoryWeapon(WeaponHash weaponHash, float xOffset, float yOffset, float zOffset, int p5)
                            => SetDropsInventoryWeapon((uint)weaponHash, xOffset, yOffset, zOffset, p5);

        public void SetEveryoneIgnoreMe(bool toggle)
            => RAGE.Game.Player.SetEveryoneIgnorePlayer(toggle);

        public void SetForcedAim(bool toggle)
            => RAGE.Game.Player.SetPlayerForcedAim(toggle);

        public void SetForcedZoom(bool toggle)
            => RAGE.Game.Player.SetPlayerForcedZoom(toggle);

        public void SetForceSkipAimIntro(bool toggle)
            => RAGE.Game.Player.SetPlayerForceSkipAimIntro(toggle);

        public void SetHasReserveParachute()
            => RAGE.Game.Player.SetPlayerHasReserveParachute();

        public void SetHeadBlendPaletteColor(Color color, int type)
                                                    => RAGE.Game.Invoker.Invoke((ulong)NativeHash.SET_HEAD_BLEND_PALETTE_COLOR, Handle, (int)color.R, (int)color.G, (int)color.B, type);

        public void SetHeadBlendPaletteColor(ColorDto color, int type)
            => RAGE.Game.Invoker.Invoke((ulong)NativeHash.SET_HEAD_BLEND_PALETTE_COLOR, Handle, (int)color.R, (int)color.G, (int)color.B, type);

        public void SetHealthRechargeMultiplier(float regenRate)
            => RAGE.Game.Player.SetPlayerHealthRechargeMultiplier(regenRate);

        public void SetIgnoreLowPriorityShockingEvents(bool toggle)
            => RAGE.Game.Player.SetIgnoreLowPriorityShockingEvents(toggle);

        public void SetInfiniteAmmo(bool toggle, WeaponHash weaponHash)
                            => SetInfiniteAmmo(toggle, (uint)weaponHash);

        public void SetIntoVehicle(int vehicle, VehicleSeat seatIndex)
            => SetIntoVehicle(vehicle, (int)seatIndex - 1);

        public new void SetInvincible(bool toggle)
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            => RAGE.Game.Player.SetPlayerInvincible(toggle);

        public void SetLockon(bool toggle)
            => RAGE.Game.Player.SetPlayerLockon(toggle);

        public void SetLockonRangeOverride(float range)
            => RAGE.Game.Player.SetPlayerLockonRangeOverride(range);

        public void SetMaxArmour(int value)
            => RAGE.Game.Player.SetPlayerMaxArmour(value);

        public void SetMaxWantedLevel(int maxWantedLevel)
            => RAGE.Game.Player.SetMaxWantedLevel(maxWantedLevel);

        public void SetMayNotEnterAnyVehicle()
            => RAGE.Game.Player.SetPlayerMayNotEnterAnyVehicle();

        public void SetMayOnlyEnterThisVehicle(int vehicle)
            => RAGE.Game.Player.SetPlayerMayOnlyEnterThisVehicle(vehicle);

        public void SetMeleeWeaponDamageModifier(float modifier, int p2)
            => RAGE.Game.Player.SetPlayerMeleeWeaponDamageModifier(modifier, p2);

        public void SetMeleeWeaponDefenseModifier(float modifier)
            => RAGE.Game.Player.SetPlayerMeleeWeaponDefenseModifier(modifier);

        public void SetModel(uint model)
            => RAGE.Game.Player.SetPlayerModel(model);

        public void SetNoCollisionEntity(int entity2)
                                                                                            => SetNoCollisionEntity(entity2, true);

        public void SetNoiseMultiplier(float multiplier)
            => RAGE.Game.Player.SetPlayerNoiseMultiplier(multiplier);

        public void SetParachuteModelOverride(uint model)
            => RAGE.Game.Player.SetPlayerParachuteModelOverride(model);

        public void SetParachutePackModelOverride(uint model)
            => RAGE.Game.Player.SetPlayerParachutePackModelOverride(model);

        public void SetParachutePackTintIndex(int tintIndex)
            => RAGE.Game.Player.SetPlayerParachutePackTintIndex(tintIndex);

        public void SetParachuteSmokeTrailColor(int r, int g, int b)
            => RAGE.Game.Player.SetPlayerParachuteSmokeTrailColor(r, g, b);

        public new void SetParachuteTintIndex(int tintIndex)
            => RAGE.Game.Player.SetPlayerParachuteTintIndex(tintIndex);

        public void SetParachuteVariationOverride(int p1, int p2, int p3, bool p4)
            => RAGE.Game.Player.SetPlayerParachuteVariationOverride(p1, p2, p3, p4);

        public void SetPoliceIgnore(bool toggle)
            => RAGE.Game.Player.SetPoliceIgnorePlayer(toggle);

        public void SetPoliceRadarBlips(bool toggle)
            => RAGE.Game.Player.SetPoliceRadarBlips(toggle);

        public new void SetReserveParachuteTintIndex(int index)
            => RAGE.Game.Player.SetPlayerReserveParachuteTintIndex(index);

        public void SetResetFlagPreferRearSeats(int flags)
            => RAGE.Game.Player.SetPlayerResetFlagPreferRearSeats(flags);

        public void SetRunSprintMultiplier(float multiplier)
            => RAGE.Game.Player.SetRunSprintMultiplierForPlayer(multiplier);

        public void SetSimulateAiming(bool toggle)
            => RAGE.Game.Player.SetPlayerSimulateAiming(toggle);

        public void SetSneakingNoiseMultiplier(float multiplier)
            => RAGE.Game.Player.SetPlayerSneakingNoiseMultiplier(multiplier);

        public void SetSpecialAbilityMultiplier(float multiplier)
            => RAGE.Game.Player.SetSpecialAbilityMultiplier(multiplier);

        public void SetSprint(bool toggle)
            => RAGE.Game.Player.SetPlayerSprint(toggle);

        public void SetStealthPerceptionModifier(float value)
            => RAGE.Game.Player.SetPlayerStealthPerceptionModifier(value);

        public void SetSwimMultiplier(float multiplier)
            => RAGE.Game.Player.SetSwimMultiplierForPlayer(multiplier);

        public void SetTargetingMode(int targetMode)
            => RAGE.Game.Player.SetPlayerTargetingMode(targetMode);

        public void SetTeam(int team)
            => RAGE.Game.Player.SetPlayerTeam(team);

        public void SetVehicleDamageModifier(float damageAmount)
            => RAGE.Game.Player.SetPlayerVehicleDamageModifier(damageAmount);

        public void SetVehicleDefenseModifier(float modifier)
            => RAGE.Game.Player.SetPlayerVehicleDefenseModifier(modifier);

        public void SetVisible(bool toggle)
                                                                                                                                                                                            => SetVisible(toggle, false);

        public void SetWantedCentrePosition(Position3D position, bool p2, bool p3)
            => RAGE.Game.Player.SetPlayerWantedCentrePosition(position.ToVector3(), p2, p3);

        public void SetWantedLevel(int wantedLevel, bool disableNoMission)
            => RAGE.Game.Player.SetPlayerWantedLevel(wantedLevel, disableNoMission);

        public void SetWantedLevelDifficulty(float difficulty)
            => RAGE.Game.Player.SetWantedLevelDifficulty(difficulty);

        public void SetWantedLevelMultiplier(float multiplier)
            => RAGE.Game.Player.SetWantedLevelMultiplier(multiplier);

        public void SetWantedLevelNoDrop(int wantedLevel, bool p2)
            => RAGE.Game.Player.SetPlayerWantedLevelNoDrop(wantedLevel, p2);

        public void SetWantedLevelNow(bool p1)
            => RAGE.Game.Player.SetPlayerWantedLevelNow(p1);

        public void SetWeaponDamageModifier(float damageAmount)
            => RAGE.Game.Player.SetPlayerWeaponDamageModifier(damageAmount);

        public void SetWeaponDefenseModifier(float modifier)
            => RAGE.Game.Player.SetPlayerWeaponDefenseModifier(modifier);

        public void SetWeaponTintIndex(WeaponHash weaponHash, int tintIndex)
                                                                            => SetWeaponTintIndex((uint)weaponHash, tintIndex);

        public void SimulateInputGait(float amount, int gaitType, float speed, bool p4, bool p5)
            => RAGE.Game.Player.SimulatePlayerInputGait(amount, gaitType, speed, p4, p5);

        public void SpecialAbilityChargeAbsolute(int p1, bool p2)
            => RAGE.Game.Player.SpecialAbilityChargeAbsolute(p1, p2);

        public void SpecialAbilityChargeContinuous(int p2)
            => RAGE.Game.Player.SpecialAbilityChargeContinuous(p2);

        public void SpecialAbilityChargeLarge(bool p1, bool p2)
            => RAGE.Game.Player.SpecialAbilityChargeLarge(p1, p2);

        public void SpecialAbilityChargeMedium(bool p1, bool p2)
            => RAGE.Game.Player.SpecialAbilityChargeMedium(p1, p2);

        public void SpecialAbilityChargeNormalized(float normalizedValue, bool p2)
            => RAGE.Game.Player.SpecialAbilityChargeNormalized(normalizedValue, p2);

        public void SpecialAbilityChargeSmall(bool p1, bool p2)
            => RAGE.Game.Player.SpecialAbilityChargeSmall(p1, p2);

        public void SpecialAbilityDeactivate()
            => RAGE.Game.Player.SpecialAbilityDeactivate();

        public void SpecialAbilityDeactivateFast()
            => RAGE.Game.Player.SpecialAbilityDeactivateFast();

        public void SpecialAbilityDepleteMeter(bool p1)
            => RAGE.Game.Player.SpecialAbilityDepleteMeter(p1);

        public void SpecialAbilityFillMeter(bool p1)
            => RAGE.Game.Player.SpecialAbilityFillMeter(p1);

        public void SpecialAbilityLock(uint playerModel)
            => RAGE.Game.Player.SpecialAbilityLock(playerModel);

        public void SpecialAbilityReset()
            => RAGE.Game.Player.SpecialAbilityReset();

        public void SpecialAbilityUnlock(uint playerModel)
            => RAGE.Game.Player.SpecialAbilityUnlock(playerModel);

        public void StartFiringAmnesty(int duration)
            => RAGE.Game.Player.StartFiringAmnesty(duration);

        public void StartTeleport(float x, float y, float z, float heading, bool p5, bool p6, bool p7)
            => RAGE.Game.Player.StartPlayerTeleport(x, y, z, heading, p5, p6, p7);

        public void StopTeleport()
            => RAGE.Game.Player.StopPlayerTeleport();

        public void SwitchCrimeType(int p1)
            => RAGE.Game.Player.SwitchCrimeType(p1);

        public void TaskForceMotionState(MotionState state, bool p2)
            => TaskForceMotionState((uint)state, p2);

        public void TaskPlayAnim(string animDict, string animName, float speed, float speedMultiplier, int duration, int flat, int playbackRate, bool lockX, bool lockY, bool lockZ)
            => TaskPlayAnim(animDict, animName, speed, speedMultiplier, duration, flat, playbackRate, lockX, lockY, lockZ);

        #endregion Public Methods
    }
}
