using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Ped
{
    public interface IPedBase : IEntityBase
    {
        int Armor { get; set; }

        //
        // Summary:
        //     see RAGE.Game.Weapon.AddAmmoToPed(System.Int32,System.UInt32,System.Int32)
        void AddAmmoTo(uint weaponHash, int ammo);
        //
        // Summary:
        //     see RAGE.Game.Ped.AddArmourToPed(System.Int32,System.Int32)
        void AddArmourTo(int amount);
        //
        // Summary:
        //     see RAGE.Game.Fire.AddOwnedExplosion(System.Int32,System.Single,System.Single,System.Single,System.Int32,System.Single,System.Boolean,System.Boolean,System.Single)
        void AddOwnedExplosion(float x, float y, float z, int explosionType, float damageScale, bool isAudible, bool isInvisible, float cameraShake);
        //
        // Summary:
        //     see RAGE.Game.Ai.AddVehicleSubtaskAttackPed(System.Int32,System.Int32)
        void AddVehicleSubtaskAttack(int ped2);
        //
        // Summary:
        //     see RAGE.Game.Ai.AddVehicleSubtaskAttackCoord(System.Int32,System.Single,System.Single,System.Single)
        void AddVehicleSubtaskAttackCoord(float x, float y, float z);
        //
        // Summary:
        //     see RAGE.Game.Ped.ApplyPedBlood(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.String)
        void ApplyBlood(int boneIndex, float xRot, float yRot, float zRot, string woundType);
        //
        // Summary:
        //     see RAGE.Game.Ped.ApplyPedBloodByZone(System.Int32,System.Int32,System.Single,System.Single,System.Int32@)
        void ApplyBloodByZone(int p1, float p2, float p3, ref int p4);
        //
        // Summary:
        //     see RAGE.Game.Ped.ApplyPedBloodDamageByZone(System.Int32,System.Int32,System.Single,System.Single,System.Int32)
        void ApplyBloodDamageByZone(int p1, float p2, float p3, int p4);
        //
        // Summary:
        //     see RAGE.Game.Ped.ApplyPedBloodSpecific(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Single,System.Int32@)
        void ApplyBloodSpecific(int p1, float p2, float p3, float p4, float p5, int p6, float p7, ref int p8);
        //
        // Summary:
        //     see RAGE.Game.Ped.ApplyPedDamageDecal(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Boolean,System.String)
        void ApplyDamageDecal(int p1, float p2, float p3, float p4, float p5, float p6, int p7, bool p8, string p9);
        //
        // Summary:
        //     see RAGE.Game.Ped.ApplyPedDamagePack(System.Int32,System.String,System.Single,System.Single)
        void ApplyDamagePack(string damagePack, float damage, float mult);
        //
        // Summary:
        //     see RAGE.Game.Ped.ApplyDamageToPed(System.Int32,System.Int32,System.Boolean)
        void ApplyDamageTo(int damageAmount, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Object.AttachPortablePickupToPed(System.Int32,System.Int32)
        void AttachPortablePickupTo(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.CanPedInCombatSeeTarget(System.Int32,System.Int32)
        bool CanInCombatSeeTarget(int target);
        //
        // Summary:
        //     see RAGE.Game.Ped.CanKnockPedOffVehicle(System.Int32)
        bool CanKnockOffVehicle();
        //
        // Summary:
        //     see RAGE.Game.Ped.CanPedRagdoll(System.Int32)
        bool CanRagdoll();
        //
        // Summary:
        //     see RAGE.Game.Audio.CanPedSpeak(System.Int32,System.String,System.Boolean)
        bool CanSpeak(string speechName, bool unk);
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearAllPedProps(System.Int32)
        void ClearAllProps();
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedAlternateMovementAnim(System.Int32,System.Int32,System.Single)
        void ClearAlternateMovementAnim(int stance, float p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedAlternateWalkAnim(System.Int32,System.Single)
        void ClearAlternateWalkAnim(float p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedBloodDamage(System.Int32)
        void ClearBloodDamage();
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedBloodDamageByZone(System.Int32,System.Int32)
        void ClearBloodDamageByZone(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedDamageDecalByZone(System.Int32,System.Int32,System.String)
        void ClearDamageDecalByZone(int p1, string p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedDecorations(System.Int32)
        void ClearDecorations();
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedDriveByClipsetOverride(System.Int32)
        void ClearDriveByClipsetOverride();
        //
        // Summary:
        //     see RAGE.Game.Ai.ClearDrivebyTaskUnderneathDrivingTask(System.Int32)
        void ClearDrivebyTaskUnderneathDrivingTask();
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedFacialDecorations(System.Int32)
        void ClearFacialDecorations();
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearFacialIdleAnimOverride(System.Int32)
        void ClearFacialIdleAnimOverride();
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedLastDamageBone(System.Int32)
        void ClearLastDamageBone();
        //
        // Summary:
        //     see RAGE.Game.Weapon.ClearPedLastWeaponDamage(System.Int32)
        new void ClearLastWeaponDamage();
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedProp(System.Int32,System.Int32)
        void ClearProp(int propId);
        //
        // Summary:
        //     see RAGE.Game.Ai.ClearPedSecondaryTask(System.Int32)
        void ClearSecondaryTask();
        //
        // Summary:
        //     see RAGE.Game.Ai.ClearPedTasks(System.Int32)
        void ClearTasks();
        //
        // Summary:
        //     see RAGE.Game.Ai.ClearPedTasksImmediately(System.Int32)
        void ClearTasksImmediately();
        //
        // Summary:
        //     see RAGE.Game.Ped.ClearPedWetness(System.Int32)
        void ClearWetness();
        //
        // Summary:
        //     see RAGE.Game.Ped.ClonePed(System.Int32,System.Single,System.Boolean,System.Boolean)
        int Clone(float heading, bool isNetwork, bool p3);
        //
        // Summary:
        //     see RAGE.Game.Ped.ClonePedToTarget(System.Int32,System.Int32)
        void CloneToTarget(int targetPed);
        //
        // Summary:
        //     see RAGE.Game.Ai.ControlMountedWeapon(System.Int32)
        bool ControlMountedWeapon();
        //
        // Summary:
        //     see RAGE.Game.Ui.CreateMpGamerTag(System.Int32,System.String,System.Boolean,System.Boolean,System.String,System.Int32)
        int CreateMpGamerTag(string username, bool pointedClanTag, bool isRockstarClan, string clanTag, int p5);
        //
        // Summary:
        //     see RAGE.Game.Object.DetachPortablePickupFromPed(System.Int32)
        void DetachPortablePickupFrom();
        //
        // Summary:
        //     see RAGE.Game.Audio.DisablePedPainAudio(System.Int32,System.Boolean)
        void DisablePainAudio(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ui.DoesPedHaveAiBlip(System.Int32)
        bool DoesHaveAiBlip();
        //
        // Summary:
        //     see RAGE.Game.Misc.EnableTennisMode(System.Int32,System.Boolean,System.Boolean)
        void EnableTennisMode(bool toggle, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.ExplodePedHead(System.Int32,System.UInt32)
        void ExplodeHead(WeaponHash weaponHash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.ExplodeProjectiles(System.Int32,System.UInt32,System.Boolean)
        void ExplodeProjectiles(uint weaponHash, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.ForcePedMotionState(System.Int32,System.UInt32,System.Boolean,System.Boolean,System.Boolean)
        bool ForceMotionState(uint motionStateHash, bool p2, bool p3, bool p4);
        //
        // Summary:
        //     see RAGE.Game.Ped.ForcePedToOpenParachute(System.Int32)
        void ForceToOpenParachute();
        //
        // Summary:
        //     see RAGE.Game.Ped.FreezePedCameraRotation(System.Int32)
        void FreezeCameraRotation();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedAccuracy(System.Int32)
        int GetAccuracy();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedAlertness(System.Int32)
        int GetAlertness();
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetPedAmmoByType(System.Int32,System.Int32)
        int GetAmmoByType(int ammoType);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetAmmoInClip(System.Int32,System.UInt32,System.Int32@)
        int GetAmmoInClip(WeaponHash weaponHash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetAmmoInPedWeapon(System.Int32,System.UInt32)
        int GetAmmoInWeapon(WeaponHash weaponhash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetPedAmmoTypeFromWeapon(System.Int32,System.UInt32)
        uint GetAmmoTypeFromWeapon(uint weaponHash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetPedAmmoTypeFromWeapon2(System.Int32,System.UInt32)
        uint GetAmmoTypeFromWeapon2(uint weaponHash);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedArmour(System.Int32)
        int GetArmour();
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetBestPedWeapon(System.Int32,System.Boolean)
        uint GetBestWeapon(bool p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedBoneCoords(System.Int32,System.Int32,System.Single,System.Single,System.Single)
        Position3D GetBoneCoords(int boneId, float offsetX, float offsetY, float offsetZ);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedBoneIndex(System.Int32,System.Int32)
        int GetBoneIndex(int boneId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedCauseOfDeath(System.Int32)
        uint GetCauseOfDeath();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetCombatFloat(System.Int32,System.Int32)
        float GetCombatFloat(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedCombatMovement(System.Int32)
        int GetCombatMovement();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedCombatRange(System.Int32)
        int GetCombatRange();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedConfigFlag(System.Int32,System.Int32,System.Boolean)
        bool GetConfigFlag(int flagId, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetCurrentPedVehicleWeapon(System.Int32,System.Int32@)
        bool GetCurrentVehicleWeapon(ref int weaponHash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetCurrentPedWeapon(System.Int32,System.Int32@,System.Boolean)
        bool GetCurrentWeapon(ref int weaponHash, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetCurrentPedWeaponEntityIndex(System.Int32)
        int GetCurrentWeaponEntityIndex();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetDeadPedPickupCoords(System.Int32,System.Single,System.Single)
        Position3D GetDeadPickupCoords(float p1, float p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedDecorationsState(System.Int32)
        int GetDecorationsState();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedDefensiveAreaPosition(System.Int32,System.Boolean)
        Position3D GetDefensiveAreaPosition(bool p1);
        //
        // Summary:
        //     see RAGE.Game.Ai.GetPedDesiredMoveBlendRatio(System.Int32)
        float GetDesiredMoveBlendRatio();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedDrawableVariation(System.Int32,System.Int32)
        int GetDrawableVariation(int componentId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedEnveffScale(System.Int32)
        float GetEnveffScale();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedExtractedDisplacement(System.Int32,System.Boolean)
        Position3D GetExtractedDisplacement(bool worldSpace);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedGroupIndex(System.Int32)
        int GetGroupIndex();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedHeadBlendData(System.Int32,System.Int32@)
        bool GetHeadBlendData(ref int headBlendData);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedHeadOverlayValue(System.Int32,System.Int32)
        int GetHeadOverlayValue(int overlayID);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetIsPedGadgetEquipped(System.Int32,System.UInt32)
        bool GetIsGadgetEquipped(uint gadgetHash);
        //
        // Summary:
        //     see RAGE.Game.Ai.GetIsTaskActive(System.Int32,System.Int32)
        bool GetIsTaskActive(int taskNumber);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetJackTarget(System.Int32)
        int GetJackTarget();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedLastDamageBone(System.Int32,System.Int32@)
        bool GetLastDamageBone(ref int outBone);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetPedLastWeaponImpactCoord(System.Int32,RAGE.Vector3)
        bool GetLastWeaponImpactCoord(Position3D coords);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetLockonRangeOfCurrentPedWeapon(System.Int32)
        float GetLockonRangeOfCurrentWeapon();
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetMaxAmmo(System.Int32,System.UInt32,System.Int32@)
        bool GetMaxAmmo(uint weaponHash, ref int ammo);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetMaxAmmoInClip(System.Int32,System.UInt32,System.Boolean)
        int GetMaxAmmoInClip(uint weaponHash, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedMaxHealth(System.Int32)
        new int GetMaxHealth();
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetMaxRangeOfCurrentPedWeapon(System.Int32)
        float GetMaxRangeOfCurrentWeapon();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetMeleeTargetForPed(System.Int32)
        int GetMeleeTargetFor();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedMoney(System.Int32)
        int GetMoney();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetMount(System.Int32)
        int GetMount();
        //
        // Summary:
        //     see RAGE.Game.Ai.GetNavmeshRouteDistanceRemaining(System.Int32,System.Int32@,System.Int32@)
        int GetNavmeshRouteDistanceRemaining(ref int p1, ref int p2);
        //
        // Summary:
        //     see RAGE.Game.Ai.GetNavmeshRouteResult(System.Int32)
        int GetNavmeshRouteResult();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedNearbyPeds(System.Int32,System.Int32@,System.Int32)
        int GetNearbyPeds(ref int sizeAndPeds, int ignore);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedNearbyVehicles(System.Int32,System.Int32@)
        int GetNearbyVehicles(ref int sizeAndVehs);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetNumberOfPedDrawableVariations(System.Int32,System.Int32)
        int GetNumberOfDrawableVariations(int componentId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetNumberOfPedPropDrawableVariations(System.Int32,System.Int32)
        int GetNumberOfPropDrawableVariations(int propId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetNumberOfPedPropTextureVariations(System.Int32,System.Int32,System.Int32)
        int GetNumberOfPropTextureVariations(int propId, int drawableId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetNumberOfPedTextureVariations(System.Int32,System.Int32,System.Int32)
        int GetNumberOfTextureVariations(int componentId, int drawableId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedPaletteVariation(System.Int32,System.Int32)
        int GetPaletteVariation(int componentId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedParachuteLandingType(System.Int32)
        int GetParachuteLandingType();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedParachuteState(System.Int32)
        int GetParachuteState();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedParachuteTintIndex(System.Int32,System.Int32@)
        void GetParachuteTintIndex(ref int outTintIndex);
        //
        // Summary:
        //     see RAGE.Game.Ai.GetPhoneGestureAnimCurrentTime(System.Int32)
        float GetPhoneGestureAnimCurrentTime();
        //
        // Summary:
        //     see RAGE.Game.Ai.GetPhoneGestureAnimTotalTime(System.Int32)
        float GetPhoneGestureAnimTotalTime();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPlayerPedIsFollowing(System.Int32)
        int GetPlayerIsFollowing();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedPropIndex(System.Int32,System.Int32)
        int GetPropIndex(int componentId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedPropTextureIndex(System.Int32,System.Int32)
        int GetPropTextureIndex(int componentId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedRagdollBoneIndex(System.Int32,System.Int32)
        int GetRagdollBoneIndex(int bone);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedRelationshipGroupDefaultHash(System.Int32)
        uint GetRelationshipGroupDefaultHash();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedRelationshipGroupHash(System.Int32)
        uint GetRelationshipGroupHash();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedResetFlag(System.Int32,System.Int32)
        bool GetResetFlag(int flagId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetSeatPedIsTryingToEnter(System.Int32)
        int GetSeatIsTryingToEnter();
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetSelectedPedWeapon(System.Int32)
        WeaponHash GetSelectedWeapon();
        //
        // Summary:
        //     see RAGE.Game.Ai.GetSequenceProgress(System.Int32)
        int GetSequenceProgress();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedsJacker(System.Int32)
        int GetsJacker();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedSourceOfDeath(System.Int32)
        int GetSourceOfDeath();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedStealthMovement(System.Int32)
        bool GetStealthMovement();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedTextureVariation(System.Int32,System.Int32)
        int GetTextureVariation(int componentId);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedTimeOfDeath(System.Int32)
        int GetTimeOfDeath();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetPedType(System.Int32)
        new int GetType();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetVehiclePedIsEntering(System.Int32)
        int GetVehicleIsEntering();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetVehiclePedIsIn(System.Int32,System.Boolean)
        int GetVehicleIsIn(bool lastVehicle);
        //
        // Summary:
        //     see RAGE.Game.Ped.GetVehiclePedIsTryingToEnter(System.Int32)
        int GetVehicleIsTryingToEnter();
        //
        // Summary:
        //     see RAGE.Game.Ped.GetVehiclePedIsUsing(System.Int32)
        int GetVehicleIsUsing();
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetWeaponObjectFromPed(System.Int32,System.Boolean)
        int GetWeaponObjectFrom(bool p1);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetPedWeaponTintIndex(System.Int32,System.UInt32)
        int GetWeaponTintIndex(uint weaponHash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GetPedWeapontypeInSlot(System.Int32,System.UInt32)
        uint GetWeapontypeInSlot(uint weaponSlot);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GiveDelayedWeaponToPed(System.Int32,System.UInt32,System.Int32,System.Boolean)
        void GiveDelayedWeaponTo(uint weaponHash, int time, bool equipNow);
        //
        // Summary:
        //     see RAGE.Game.Ped.GivePedHelmet(System.Int32,System.Boolean,System.Int32,System.Int32)
        void GiveHelmet(bool cannotRemove, int helmetFlag, int textureIndex);
        //
        // Summary:
        //     see RAGE.Game.Ped.GivePedNmMessage(System.Int32)
        void GiveNmMessage();
        //
        // Summary:
        //     see RAGE.Game.Ui.GivePedToPauseMenu(System.Int32,System.Int32)
        void GiveToPauseMenu(int p1);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GiveWeaponComponentToPed(System.Int32,System.UInt32,System.UInt32)
        void GiveWeaponComponentTo(uint weaponHash, uint componentHash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.GiveWeaponToPed(System.Int32,System.UInt32,System.Int32,System.Boolean,System.Boolean)
        void GiveWeaponTo(uint weaponHash, int ammoCount, bool isHidden, bool equipNow);
        //
        // Summary:
        //     see RAGE.Game.Weapon.HasPedBeenDamagedByWeapon(System.Int32,System.UInt32,System.Int32)
        new bool HasBeenDamagedByWeapon(uint weaponHash, int weaponType);
        //
        // Summary:
        //     see RAGE.Game.Weapon.HasPedGotWeapon(System.Int32,System.UInt32,System.Boolean)
        bool HasGotWeapon(uint weaponHash, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Weapon.HasPedGotWeaponComponent(System.Int32,System.UInt32,System.UInt32)
        bool HasGotWeaponComponent(uint weaponHash, uint componentHash);
        //
        // Summary:
        //     see RAGE.Game.Ped.HasPedHeadBlendFinished(System.Int32)
        bool HasHeadBlendFinished();
        //
        // Summary:
        //     see RAGE.Game.Ai.PedHasUseScenarioTask(System.Int32)
        bool HasUseScenarioTask();
        //
        // Summary:
        //     see RAGE.Game.Ped.HidePedBloodDamageByZone(System.Int32,System.Int32,System.Boolean)
        void HideBloodDamageByZone(int p1, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Weapon.HidePedWeaponForScriptedCutscene(System.Int32,System.Boolean)
        void HideWeaponForScriptedCutscene(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedActiveInScenario(System.Int32)
        bool IsActiveInScenario();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedAimingFromCover(System.Int32)
        bool IsAimingFromCover();
        //
        // Summary:
        //     see RAGE.Game.Audio.IsAmbientSpeechDisabled(System.Int32)
        bool IsAmbientSpeechDisabled();
        //
        // Summary:
        //     see RAGE.Game.Audio.IsAnySpeechPlaying(System.Int32)
        bool IsAnySpeechPlaying();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedAPlayer(System.Int32)
        bool IsAPlayer();
        //
        // Summary:
        //     see RAGE.Game.Weapon.IsPedArmed(System.Int32,System.Int32)
        bool IsArmed(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedBeingArrested(System.Int32)
        bool IsBeingArrested();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedBeingJacked(System.Int32)
        bool IsBeingJacked();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedBeingStealthKilled(System.Int32)
        bool IsBeingStealthKilled();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedBeingStunned(System.Int32,System.Int32)
        bool IsBeingStunned(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedBlushColorValid(System.Int32)
        bool IsBlushColorValid();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedClimbing(System.Int32)
        bool IsClimbing();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedComponentVariationValid(System.Int32,System.Int32,System.Int32,System.Int32)
        bool IsComponentVariationValid(int componentId, int drawableId, int textureId);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsConversationPedDead(System.Int32)
        bool IsConversationDead();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedCuffed(System.Int32)
        bool IsCuffed();
        //
        // Summary:
        //     see RAGE.Game.Weapon.IsPedCurrentWeaponSilenced(System.Int32)
        bool IsCurrentWeaponSilenced();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedDeadOrDying(System.Int32,System.Boolean)
        bool IsDeadOrDying();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedDiving(System.Int32)
        bool IsDiving();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedDoingDriveby(System.Int32)
        bool IsDoingDriveby();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsDrivebyTaskUnderneathDrivingTask(System.Int32)
        bool IsDrivebyTaskUnderneathDrivingTask();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedDucking(System.Int32)
        bool IsDucking();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedEvasiveDiving(System.Int32,System.Int32@)
        bool IsEvasiveDiving(ref int evadingEntity);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedFacingPed(System.Int32,System.Int32,System.Single)
        bool IsFacingPed(int otherPed, float angle);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedFalling(System.Int32)
        bool IsFalling();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedFatallyInjured(System.Int32)
        bool IsFatallyInjured();
        //
        // Summary:
        //     see RAGE.Game.Weapon.IsFlashLightOn(System.Int32)
        bool IsFlashLightOn();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedFleeing(System.Int32)
        bool IsFleeing();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedGettingIntoAVehicle(System.Int32)
        bool IsGettingIntoAVehicle();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedGettingUp(System.Int32)
        bool IsGettingUp();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedGoingIntoCover(System.Int32)
        bool IsGoingIntoCover();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedGroupMember(System.Int32,System.Int32)
        bool IsGroupMember(int groupId);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedHairColorValid(System.Int32)
        bool IsHairColorValid();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedHangingOnToVehicle(System.Int32)
        bool IsHangingOnToVehicle();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedheadshotReady(System.Int32)
        bool IsheadshotReady();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedheadshotValid(System.Int32)
        bool IsheadshotValid();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedHeadtrackingEntity(System.Int32,System.Int32)
        bool IsHeadtrackingEntity(int entity);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedHeadtrackingPed(System.Int32,System.Int32)
        bool IsHeadtrackingPed(int ped2);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedHuman(System.Int32)
        bool IsHuman();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedHurt(System.Int32)
        bool IsHurt();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInAnyBoat(System.Int32)
        bool IsInAnyBoat();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInAnyHeli(System.Int32)
        bool IsInAnyHeli();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInAnyPlane(System.Int32)
        bool IsInAnyPlane();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInAnyPoliceVehicle(System.Int32)
        bool IsInAnyPoliceVehicle();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInAnySub(System.Int32)
        bool IsInAnySub();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInAnyTaxi(System.Int32)
        bool IsInAnyTaxi();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInAnyTrain(System.Int32)
        bool IsInAnyTrain();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInAnyVehicle(System.Int32,System.Boolean)
        bool IsInAnyVehicle(bool atGetIn);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInCombat(System.Int32,System.Int32)
        bool IsInCombat(int target);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInCover(System.Int32,System.Boolean)
        bool IsInCover(bool p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInCoverFacingLeft(System.Int32)
        bool IsInCoverFacingLeft();
        //
        // Summary:
        //     see RAGE.Game.Audio.IsPedInCurrentConversation(System.Int32)
        bool IsInCurrentConversation();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInFlyingVehicle(System.Int32)
        bool IsInFlyingVehicle();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInGroup(System.Int32)
        bool IsInGroup();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInjured(System.Int32)
        bool IsInjured();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInMeleeCombat(System.Int32)
        bool IsInMeleeCombat();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInModel(System.Int32,System.UInt32)
        bool IsInModel(uint modelHash);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInParachuteFreeFall(System.Int32)
        bool IsInParachuteFreeFall();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedInVehicle(System.Int32,System.Int32,System.Boolean)
        bool IsInVehicle(int vehicle, bool atGetIn);
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedInWrithe(System.Int32)
        bool IsInWrithe();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedJacking(System.Int32)
        bool IsJacking();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedJumping(System.Int32)
        bool IsJumping();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedJumpingOutOfVehicle(System.Int32)
        bool IsJumpingOutOfVehicle();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedLipstickColorValid(System.Int32)
        bool IsLipstickColorValid();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedMale(System.Int32)
        bool IsMale();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedModel(System.Int32,System.UInt32)
        bool IsModel(uint modelHash);
        //
        // Summary:
        //     see RAGE.Game.Ai.IsMountedWeaponTaskUnderneathDrivingTask(System.Int32)
        bool IsMountedWeaponTaskUnderneathDrivingTask();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsMoveBlendRatioRunning(System.Int32)
        bool IsMoveBlendRatioRunning();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsMoveBlendRatioSprinting(System.Int32)
        bool IsMoveBlendRatioSprinting();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsMoveBlendRatioStill(System.Int32)
        bool IsMoveBlendRatioStill();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsMoveBlendRatioWalking(System.Int32)
        bool IsMoveBlendRatioWalking();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedOnAnyBike(System.Int32)
        bool IsOnAnyBike();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedOnFoot(System.Int32)
        bool IsOnFoot();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedOnMount(System.Int32)
        bool IsOnMount();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedOnSpecificVehicle(System.Int32,System.Int32)
        bool IsOnSpecificVehicle(int vehicle);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedOnVehicle(System.Int32)
        bool IsOnVehicle();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedPerformingStealthKill(System.Int32)
        bool IsPerformingStealthKill();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedPlantingBomb(System.Int32)
        bool IsPlantingBomb();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPlayingPhoneGestureAnim(System.Int32)
        bool IsPlayingPhoneGestureAnim();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedProne(System.Int32)
        bool IsProne();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedPropValid(System.Int32,System.Int32,System.Int32,System.Int32)
        bool IsPropValid(int componentId, int drawableId, int TextureId);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedRagdoll(System.Int32)
        bool IsRagdoll();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedReloading(System.Int32)
        bool IsReloading();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedRespondingToEvent(System.Int32,System.Int32)
        bool IsRespondingToEvent(int eventId);
        //
        // Summary:
        //     see RAGE.Game.Audio.IsPedRingtonePlaying(System.Int32)
        bool IsRingtonePlaying();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedRunning(System.Int32)
        bool IsRunning();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedRunningArrestTask(System.Int32)
        bool IsRunningArrestTask();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedRunningMobilePhoneTask(System.Int32)
        bool IsRunningMobilePhoneTask();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedRunningRagdollTask(System.Int32)
        bool IsRunningRagdollTask();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsScriptedScenarioPedUsingConditionalAnim(System.Int32,System.String,System.String)
        bool IsScriptedScenarioUsingConditionalAnim(string animDict, string anim);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedShooting(System.Int32)
        bool IsShooting();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedShootingInArea(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Boolean,System.Boolean)
        bool IsShootingInArea(float x1, float y1, float z1, float x2, float y2, float z2, bool p7, bool p8);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedSittingInAnyVehicle(System.Int32)
        bool IsSittingInAnyVehicle();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedSittingInVehicle(System.Int32,System.Int32)
        bool IsSittingInVehicle(int vehicle);
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedSprinting(System.Int32)
        bool IsSprinting();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedStandingInCover(System.Int32)
        bool IsStandingInCover();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedStill(System.Int32)
        bool IsStill();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedStopped(System.Int32)
        bool IsStopped();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedStrafing(System.Int32)
        bool IsStrafing();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedSwimming(System.Int32)
        bool IsSwimming();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedSwimmingUnderWater(System.Int32)
        bool IsSwimmingUnderWater();
        //
        // Summary:
        //     see RAGE.Game.Misc.IsTennisMode(System.Int32)
        bool IsTennisMode();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedTracked(System.Int32)
        bool IsTracked();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsTrackedPedVisible(System.Int32)
        bool IsTrackedVisible();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedTryingToEnterALockedVehicle(System.Int32)
        bool IsTryingToEnterALockedVehicle();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedUsingActionMode(System.Int32)
        bool IsUsingActionMode();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedUsingAnyScenario(System.Int32)
        bool IsUsingAnyScenario();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedUsingScenario(System.Int32,System.String)
        bool IsUsingScenario(string scenario);
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedVaulting(System.Int32)
        bool IsVaulting();
        //
        // Summary:
        //     see RAGE.Game.Ai.IsPedWalking(System.Int32)
        bool IsWalking();
        //
        // Summary:
        //     see RAGE.Game.Weapon.IsPedWeaponComponentActive(System.Int32,System.UInt32,System.UInt32)
        bool IsWeaponComponentActive(uint weaponHash, uint componentHash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.IsPedWeaponReadyToShoot(System.Int32)
        bool IsWeaponReadyToShoot();
        //
        // Summary:
        //     see RAGE.Game.Ped.IsPedWearingHelmet(System.Int32)
        bool IsWearingHelmet();
        //
        // Summary:
        //     see RAGE.Game.Ped.KnockOffPedProp(System.Int32,System.Boolean,System.Boolean,System.Boolean,System.Boolean)
        void KnockOffProp(bool p1, bool p2, bool p3, bool p4);
        //
        // Summary:
        //     see RAGE.Game.Ped.KnockPedOffVehicle(System.Int32)
        void KnockOffVehicle();
        //
        // Summary:
        //     see RAGE.Game.Weapon.MakePedReload(System.Int32)
        bool MakeReload();
        //
        // Summary:
        //     see RAGE.Game.Network.NetworkAddPedToSynchronisedScene(System.Int32,System.Int32,System.String,System.String,System.Single,System.Single,System.Int32,System.Int32,System.Single,System.Int32)
        void NetworkAddToSynchronisedScene(int netScene, string animDict, string animnName, float speed, float speedMultiplier, int duration, int flag, float playbackRate, int p9);
        //
        // Summary:
        //     see RAGE.Game.Network.NetworkGetPlayerIndexFromPed(System.Int32)
        int NetworkGetPlayerIndexFrom();
        //
        // Summary:
        //     see RAGE.Game.Audio.PlayAmbientSpeech1(System.Int32,System.String,System.String,System.Int32)
        void PlayAmbientSpeech1(string speechName, string speechParam, int p3);
        //
        // Summary:
        //     see RAGE.Game.Audio.PlayAmbientSpeech2(System.Int32,System.String,System.String,System.Int32)
        void PlayAmbientSpeech2(string speechName, string speechParam, int p3);
        //
        // Summary:
        //     see RAGE.Game.Ai.PlayAnimOnRunningScenario(System.Int32,System.String,System.String)
        void PlayAnimOnRunningScenario(string animDict, string animName);
        //
        // Summary:
        //     see RAGE.Game.Ped.PlayFacialAnim(System.Int32,System.String,System.String)
        void PlayFacialAnim(string animName, string animDict);
        //
        // Summary:
        //     see RAGE.Game.Audio.PlayPain(System.Int32,System.Int32,System.Int32,System.Int32)
        void PlayPain(int painID, int p1, int p3);
        //
        // Summary:
        //     see RAGE.Game.Audio.PlayStreamFromPed(System.Int32)
        void PlayStreamFrom();
        //
        // Summary:
        //     see RAGE.Game.Ped.RegisterHatedTargetsAroundPed(System.Int32,System.Single)
        void RegisterHatedTargetsAround(float radius);
        //
        // Summary:
        //     see RAGE.Game.Ped.RegisterPedheadshot(System.Int32)
        int Registerheadshot();
        //
        // Summary:
        //     see RAGE.Game.Ped.RegisterTarget(System.Int32,System.Int32)
        void RegisterTarget(int target);
        //
        // Summary:
        //     see RAGE.Game.Weapon.RemoveAllPedWeapons(System.Int32,System.Boolean)
        void RemoveAllWeapons(bool p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.RemovePedDefensiveArea(System.Int32,System.Boolean)
        void RemoveDefensiveArea(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.RemovePedFromGroup(System.Int32)
        void RemoveFromGroup();
        //
        // Summary:
        //     see RAGE.Game.Ped.RemovePedHelmet(System.Int32,System.Boolean)
        void RemoveHelmet(bool instantly);
        //
        // Summary:
        //     see RAGE.Game.Ped.RemovePedPreferredCoverSet(System.Int32)
        void RemovePreferredCoverSet();
        //
        // Summary:
        //     see RAGE.Game.Weapon.RemoveWeaponComponentFromPed(System.Int32,System.UInt32,System.UInt32)
        void RemoveWeaponComponentFrom(uint weaponHash, uint componentHash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.RemoveWeaponFromPed(System.Int32,System.UInt32)
        void RemoveWeaponFrom(uint weaponHash);
        //
        // Summary:
        //     see RAGE.Game.Ped.ResetPedInVehicleContext(System.Int32)
        void ResetInVehicleContext();
        //
        // Summary:
        //     see RAGE.Game.Ped.ResetPedLastVehicle(System.Int32)
        void ResetLastVehicle();
        //
        // Summary:
        //     see RAGE.Game.Ped.ResetPedMovementClipset(System.Int32,System.Single)
        void ResetMovementClipset(float clipSetSwitchTime);
        void ResetStrafeClipset();
        //
        // Summary:
        //     see RAGE.Game.Ped.ResetPedRagdollBlockingFlags(System.Int32,System.Int32)
        void ResetRagdollBlockingFlags(int flags);
        //
        // Summary:
        //     see RAGE.Game.Ped.ResetPedRagdollTimer(System.Int32)
        void ResetRagdollTimer();
        //
        // Summary:
        //     see RAGE.Game.Ped.ResetPedVisibleDamage(System.Int32)
        void ResetVisibleDamage();
        //
        // Summary:
        //     see RAGE.Game.Ped.ResetPedWeaponMovementClipset(System.Int32)
        void ResetWeaponMovementClipset();
        //
        // Summary:
        //     see RAGE.Game.Ped.ResurrectPed(System.Int32)
        void Resurrect();
        //
        // Summary:
        //     see RAGE.Game.Ped.ReviveInjuredPed(System.Int32)
        void ReviveInjured();
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAccuracy(System.Int32,System.Int32)
        void SetAccuracy(int accuracy);
        //
        // Summary:
        //     see RAGE.Game.Ui.SetAiBlipMaxDistance(System.Int32,System.Single)
        void SetAiBlipMaxDistance(float distance);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAlertness(System.Int32,System.Int32)
        void SetAlertness(int value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAllowedToDuck(System.Int32,System.Boolean)
        void SetAllowedToDuck(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAllowVehiclesOverride(System.Int32,System.Boolean)
        void SetAllowVehiclesOverride(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAlternateMovementAnim(System.Int32,System.Int32,System.String,System.String,System.Single,System.Boolean)
        void SetAlternateMovementAnim(int stance, string animDictionary, string animationName, float p4, bool p5);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAlternateWalkAnim(System.Int32,System.String,System.String,System.Single,System.Boolean)
        void SetAlternateWalkAnim(string animDict, string animName, float p3, bool p4);
        //
        // Summary:
        //     see RAGE.Game.Audio.SetAmbientVoiceName(System.Int32,System.String)
        void SetAmbientVoiceName(string name);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedAmmo(System.Int32,System.UInt32,System.Int32,System.Int32)
        void SetAmmo(uint weaponHash, int ammo, int p3);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedAmmoByType(System.Int32,System.Int32,System.Int32)
        void SetAmmoByType(int ammoType, int ammo);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetAmmoInClip(System.Int32,System.UInt32,System.Int32)
        bool SetAmmoInClip(uint weaponHash, int ammo);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedAmmoToDrop(System.Int32,System.Int32)
        void SetAmmoToDrop(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAngledDefensiveArea(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Boolean,System.Boolean)
        void SetAngledDefensiveArea(float p1, float p2, float p3, float p4, float p5, float p6, float p7, bool p8, bool p9);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedArmour(System.Int32,System.Int32)
        void SetArmour(int amount);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAsCop(System.Int32,System.Boolean)
        void SetAsCop(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAsEnemy(System.Int32,System.Boolean)
        void SetAsEnemy(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAsGroupLeader(System.Int32,System.Int32)
        void SetAsGroupLeader(int groupId);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedAsGroupMember(System.Int32,System.Int32)
        void SetAsGroupMember(int groupId);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedBlendFromParents(System.Int32,System.Int32,System.Int32,System.Single,System.Single)
        void SetBlendFromParents(int p1, int p2, float p3, float p4);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetBlockingOfNonTemporaryEvents(System.Int32,System.Boolean)
        void SetBlockingOfNonTemporaryEvents(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedBoundsOrientation(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single)
        void SetBoundsOrientation(float p1, float p2, float p3, float p4, float p5);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanArmIk(System.Int32,System.Boolean)
        void SetCanArmIk(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetCanAttackFriendly(System.Int32,System.Boolean,System.Boolean)
        void SetCanAttackFriendly(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanBeDraggedOut(System.Int32,System.Boolean)
        void SetCanBeDraggedOut(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanBeKnockedOffVehicle(System.Int32,System.Int32)
        void SetCanBeKnockedOffVehicle(int state);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanBeShotInVehicle(System.Int32,System.Boolean)
        void SetCanBeShotInVehicle(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanBeTargetedWhenInjured(System.Int32,System.Boolean)
        void SetCanBeTargetedWhenInjured(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanBeTargetedWithoutLos(System.Int32,System.Boolean)
        new void SetCanBeTargetedWithoutLos(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanBeTargetted(System.Int32,System.Boolean)
        void SetCanBeTargetted(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanBeTargettedByPlayer(System.Int32,System.Int32,System.Boolean)
        void SetCanBeTargettedByPlayer(int player, bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanBeTargettedByTeam(System.Int32,System.Int32,System.Boolean)
        void SetCanBeTargettedByTeam(int team, bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanCowerInCover(System.Int32,System.Boolean)
        void SetCanCowerInCover(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanEvasiveDive(System.Int32,System.Boolean)
        void SetCanEvasiveDive(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanHeadIk(System.Int32,System.Boolean)
        void SetCanHeadIk(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanLegIk(System.Int32,System.Boolean)
        void SetCanLegIk(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanPeekInCover(System.Int32,System.Boolean)
        void SetCanPeekInCover(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanPlayAmbientAnims(System.Int32,System.Boolean)
        void SetCanPlayAmbientAnims(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanPlayAmbientBaseAnims(System.Int32,System.Boolean)
        void SetCanPlayAmbientBaseAnims(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanPlayGestureAnims(System.Int32,System.Boolean)
        void SetCanPlayGestureAnims(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanPlayVisemeAnims(System.Int32,System.Boolean,System.Boolean)
        void SetCanPlayVisemeAnims(bool toggle, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanRagdoll(System.Int32,System.Boolean)
        void SetCanRagdoll(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanRagdollFromPlayerImpact(System.Int32,System.Boolean)
        void SetCanRagdollFromPlayerImpact(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanSmashGlass(System.Int32,System.Boolean,System.Boolean)
        void SetCanSmashGlass(bool p1, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanSwitchWeapon(System.Int32,System.Boolean)
        void SetCanSwitchWeapon(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanTeleportToGroupLeader(System.Int32,System.Int32,System.Boolean)
        void SetCanTeleportToGroupLeader(int groupHandle, bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanTorsoIk(System.Int32,System.Boolean)
        void SetCanTorsoIk(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCanUseAutoConversationLookat(System.Int32,System.Boolean)
        void SetCanUseAutoConversationLookat(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCapsule(System.Int32,System.Single)
        void SetCapsule(float value);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedChanceOfFiringBlanks(System.Int32,System.Single,System.Single)
        void SetChanceOfFiringBlanks(float xBias, float yBias);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedClothProne(System.Int32,System.Int32)
        void SetClothProne(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCombatAbility(System.Int32,System.Int32)
        void SetCombatAbility(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCombatAttributes(System.Int32,System.Int32,System.Boolean)
        void SetCombatAttributes(int attributeIndex, bool enabled);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetCombatFloat(System.Int32,System.Int32,System.Single)
        void SetCombatFloat(int combatType, float p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCombatMovement(System.Int32,System.Int32)
        void SetCombatMovement(int combatMovement);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCombatRange(System.Int32,System.Int32)
        void SetCombatRange(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedComponentVariation(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)
        void SetComponentVariation(int componentId, int drawableId, int textureId, int paletteId);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedConfigFlag(System.Int32,System.Int32,System.Boolean)
        void SetConfigFlag(int flagId, bool value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCoordsKeepVehicle(System.Int32,System.Single,System.Single,System.Single)
        void SetCoordsKeepVehicle(float posX, float posY, float posZ);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCoordsNoGang(System.Int32,System.Single,System.Single,System.Single)
        void SetCoordsNoGang(float posX, float posY, float posZ);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedCowerHash(System.Int32,System.String)
        void SetCowerHash(string p1);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetCurrentPedVehicleWeapon(System.Int32,System.UInt32)
        bool SetCurrentVehicleWeapon(uint weaponHash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetCurrentPedWeapon(System.Int32,System.UInt32,System.Boolean)
        void SetCurrentWeapon(uint weaponHash, bool equipNow);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedCurrentWeaponVisible(System.Int32,System.Boolean,System.Boolean,System.Boolean,System.Boolean)
        void SetCurrentWeaponVisible(bool visible, bool deselectWeapon, bool p3, bool p4);
        //
        // Summary:
        //     see RAGE.Game.Event.SetDecisionMaker(System.Int32,System.UInt32)
        void SetDecisionMaker(uint name);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDecoration(System.Int32,System.UInt32,System.UInt32)
        void SetDecoration(uint collection, uint overlay);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDefaultComponentVariation(System.Int32)
        void SetDefaultComponentVariation();
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDefensiveAreaAttachedToPed(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Boolean,System.Boolean)
        void SetDefensiveAreaAttachedToPed(int attachPed, float p2, float p3, float p4, float p5, float p6, float p7, float p8, bool p9, bool p10);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDefensiveAreaDirection(System.Int32,System.Single,System.Single,System.Single,System.Boolean)
        void SetDefensiveAreaDirection(float p1, float p2, float p3, bool p4);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDefensiveSphereAttachedToPed(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Boolean)
        void SetDefensiveSphereAttachedToPed(int target, float xOffset, float yOffset, float zOffset, float radius, bool p6);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDesiredHeading(System.Int32,System.Single)
        void SetDesiredHeading(float heading);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetPedDesiredMoveBlendRatio(System.Int32,System.Single)
        void SetDesiredMoveBlendRatio(float p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDiesInSinkingVehicle(System.Int32,System.Boolean)
        void SetDiesInSinkingVehicle(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDiesInstantlyInWater(System.Int32,System.Boolean)
        void SetDiesInstantlyInWater(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDiesInVehicle(System.Int32,System.Boolean)
        void SetDiesInVehicle(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDiesInWater(System.Int32,System.Boolean)
        void SetDiesInWater(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDiesWhenInjured(System.Int32,System.Boolean)
        void SetDiesWhenInjured(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDriveByClipsetOverride(System.Int32,System.String)
        void SetDriveByClipsetOverride(string clipset);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetDriveTaskDrivingStyle(System.Int32,System.Int32)
        void SetDriveTaskDrivingStyle(int drivingStyle);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedDropsInventoryWeapon(System.Int32,System.UInt32,System.Single,System.Single,System.Single,System.Int32)
        void SetDropsInventoryWeapon(uint weaponHash, float xOffset, float yOffset, float zOffset, int p5);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedDropsWeapon(System.Int32)
        void SetDropsWeapon();
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedDropsWeaponsWhenDead(System.Int32,System.Boolean)
        void SetDropsWeaponsWhenDead(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedDucking(System.Int32,System.Boolean)
        void SetDucking(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetEnableBoundAnkles(System.Int32,System.Boolean)
        void SetEnableBoundAnkles(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetEnablePedEnveffScale(System.Int32,System.Boolean)
        void SetEnableEnveffScale(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetEnableHandcuffs(System.Int32,System.Boolean)
        void SetEnableHandcuffs(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetEnableScuba(System.Int32,System.Boolean)
        void SetEnableScuba(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedEnableWeaponBlocking(System.Int32,System.Boolean)
        void SetEnableWeaponBlocking(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ui.SetPedEnemyAiBlip(System.Int32,System.Boolean)
        void SetEnemyAiBlip(bool showViewCones);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedEnveffScale(System.Int32,System.Single)
        void SetEnveffScale(float value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedEyeColor(System.Int32,System.Int32)
        void SetEyeColor(int index);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedFaceFeature(System.Int32,System.Int32,System.Single)
        void SetFaceFeature(int index, float scale);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedFacialDecoration(System.Int32,System.UInt32,System.UInt32)
        void SetFacialDecoration(uint collection, uint overlay);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetFacialIdleAnimOverride(System.Int32,System.String,System.String)
        void SetFacialIdleAnimOverride(string animName, string animDict);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedFiringPattern(System.Int32,System.UInt32)
        void SetFiringPattern(uint patternHash);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedFleeAttributes(System.Int32,System.Int32,System.Boolean)
        void SetFleeAttributes(int attributes, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedGadget(System.Int32,System.UInt32,System.Boolean)
        void SetGadget(uint gadgetHash, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedGeneratesDeadBodyEvents(System.Int32,System.Boolean)
        void SetGeneratesDeadBodyEvents(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedGestureGroup(System.Int32,System.String)
        void SetGestureGroup(string animGroupGesture);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedGetOutUpsideDownVehicle(System.Int32,System.Boolean)
        void SetGetOutUpsideDownVehicle(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedGravity(System.Int32,System.Boolean)
        void SetGravity(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedGroupMemberPassengerIndex(System.Int32,System.Int32)
        void SetGroupMemberPassengerIndex(int index);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedHairColor(System.Int32,System.Int32,System.Int32)
        void SetHairColor(int colorID, int highlightColorID);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedHeadBlendData(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Boolean)
        void SetHeadBlendData(int shapeFirstID, int shapeSecondID, int shapeThirdID, int skinFirstID, int skinSecondID, int skinThirdID, float shapeMix, float skinMix, float thirdMix, bool isParent);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedHeadOverlay(System.Int32,System.Int32,System.Int32,System.Single)
        void SetHeadOverlay(int overlayID, int index, float opacity);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedHeadOverlayColor(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)
        void SetHeadOverlayColor(int overlayID, int colorType, int colorID, int secondColorID);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedHearingRange(System.Int32,System.Single)
        void SetHearingRange(float value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedHelmet(System.Int32,System.Boolean)
        void SetHelmet(bool canWearHelmet);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedHelmetFlag(System.Int32,System.Int32)
        void SetHelmetFlag(int helmetFlag);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedHelmetPropIndex(System.Int32,System.Int32,System.Int32)
        void SetHelmetPropIndex(int propIndex, int p2);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedHelmetTextureIndex(System.Int32,System.Int32)
        void SetHelmetTextureIndex(int textureIndex);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetHighFallTask(System.Int32,System.Int32,System.Int32,System.Int32)
        void SetHighFallTask(int p1, int p2, int p3);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedIdRange(System.Int32,System.Single)
        void SetIdRange(float value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetIkTarget(System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Int32,System.Int32,System.Int32)
        void SetIkTarget(int p1, int targetPed, int boneLookAt, float x, float y, float z, int p7, int duration, int duration1);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedInfiniteAmmo(System.Int32,System.Boolean,System.UInt32)
        void SetInfiniteAmmo(bool toggle, uint weaponHash);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedInfiniteAmmoClip(System.Int32,System.Boolean)
        void SetInfiniteAmmoClip(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedIntoVehicle(System.Int32,System.Int32,System.Int32)
        void SetIntoVehicle(int vehicle, int seatIndex);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedInVehicleContext(System.Int32,System.UInt32)
        void SetInVehicleContext(uint context);
        //
        // Summary:
        //     see RAGE.Game.Audio.SetPedIsDrunk(System.Int32,System.Boolean)
        void SetIsDrunk(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedKeepTask(System.Int32,System.Boolean)
        void SetKeepTask(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedLegIkMode(System.Int32,System.Int32)
        void SetLegIkMode(int mode);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedLodMultiplier(System.Int32,System.Single)
        void SetLodMultiplier(float multiplier);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMaxHealth(System.Int32,System.Int32)
        new void SetMaxHealth(int value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMaxMoveBlendRatio(System.Int32,System.Single)
        void SetMaxMoveBlendRatio(float value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMaxTimeInWater(System.Int32,System.Single)
        void SetMaxTimeInWater(float value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMaxTimeUnderwater(System.Int32,System.Single)
        void SetMaxTimeUnderwater(float value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMinGroundTimeForStungun(System.Int32,System.Int32)
        void SetMinGroundTimeForStungun(int ms);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMinMoveBlendRatio(System.Int32,System.Single)
        void SetMinMoveBlendRatio(float value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedModelIsSuppressed(System.Int32,System.Boolean)
        void SetModelIsSuppressed(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMoney(System.Int32,System.Int32)
        void SetMoney(int amount);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMotionBlur(System.Int32,System.Boolean)
        new void SetMotionBlur(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMoveAnimsBlendOut(System.Int32)
        void SetMoveAnimsBlendOut();
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMovementClipset(System.Int32,System.String,System.Single)
        void SetMovementClipset(string clipSet, float clipSetSwitchTime);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedMoveRateOverride(System.Int32,System.Single)
        void SetMoveRateOverride(float value);
        //
        // Summary:
        //     see RAGE.Game.Audio.SetPedMute(System.Int32)
        void SetMute();
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedNameDebug(System.Int32,System.String)
        void SetNameDebug(string name);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedNeverLeavesGroup(System.Int32,System.Boolean)
        void SetNeverLeavesGroup(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetParachuteTaskTarget(System.Int32,System.Single,System.Single,System.Single)
        void SetParachuteTaskTarget(float x, float y, float z);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetParachuteTaskThrust(System.Int32,System.Single)
        void SetParachuteTaskThrust(float thrust);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedParachuteTintIndex(System.Int32,System.Int32)
        void SetParachuteTintIndex(int tintIndex);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetPedPathAvoidFire(System.Int32,System.Boolean)
        void SetPathAvoidFire(bool avoidFire);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetPedPathCanDropFromHeight(System.Int32,System.Boolean)
        void SetPathCanDropFromHeight(bool Toggle);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetPedPathCanUseClimbovers(System.Int32,System.Boolean)
        void SetPathCanUseClimbovers(bool Toggle);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetPedPathCanUseLadders(System.Int32,System.Boolean)
        void SetPathCanUseLadders(bool Toggle);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetPedPathMayEnterWater(System.Int32,System.Boolean)
        void SetPathMayEnterWater(bool mayEnterWater);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetPedPathPreferToAvoidWater(System.Int32,System.Boolean)
        void SetPathPreferToAvoidWater(bool avoidWater);
        //
        // Summary:
        //     see RAGE.Game.Pathfind.SetPedPathsBackToOriginal(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)
        void SetPathsBackToOriginal(int p1, int p2, int p3, int p4, int p5, int p6);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedPinnedDown(System.Int32,System.Boolean,System.Int32)
        int SetPinnedDown(bool pinned, int i);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedPlaysHeadOnHornAnimWhenDiesInVehicle(System.Int32,System.Boolean)
        void SetPlaysHeadOnHornAnimWhenDiesInVehicle(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Streaming.SetPedPopulationBudget(System.Int32)
        void SetPopulationBudget();
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedPreferredCoverSet(System.Int32,System.Int32)
        void SetPreferredCoverSet(int itemSet);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedPrimaryLookat(System.Int32,System.Int32)
        void SetPrimaryLookat(int lookAt);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedPropIndex(System.Int32,System.Int32,System.Int32,System.Int32,System.Boolean)
        void SetPropIndex(int componentId, int drawableId, int TextureId, bool attach);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedRagdollBlockingFlags(System.Int32,System.Int32)
        void SetRagdollBlockingFlags(int flags);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedRagdollForceFall(System.Int32)
        void SetRagdollForceFall();
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedRagdollOnCollision(System.Int32,System.Boolean)
        void SetRagdollOnCollision(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedRandomComponentVariation(System.Int32,System.Boolean)
        void SetRandomComponentVariation(bool p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedRandomProps(System.Int32)
        void SetRandomProps();
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedRelationshipGroupDefaultHash(System.Int32,System.UInt32)
        void SetRelationshipGroupDefaultHash(uint hash);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedRelationshipGroupHash(System.Int32,System.UInt32)
        void SetRelationshipGroupHash(uint hash);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedReserveParachuteTintIndex(System.Int32,System.Int32)
        void SetReserveParachuteTintIndex(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedResetFlag(System.Int32,System.Int32,System.Boolean)
        void SetResetFlag(int flagId, bool doReset);
        //
        // Summary:
        //     see RAGE.Game.Audio.SetPedScream(System.Int32)
        void SetScream();
        //
        // Summary:
        //     see RAGE.Game.Ped.SetScriptedAnimSeatOffset(System.Int32,System.Single)
        void SetScriptedAnimSeatOffset(float p1);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedSeeingRange(System.Int32,System.Single)
        void SetSeeingRange(float value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedShootRate(System.Int32,System.Int32)
        void SetShootRate(int shootRate);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedShootsAtCoord(System.Int32,System.Single,System.Single,System.Single,System.Boolean)
        void SetShootsAtCoord(float x, float y, float z, bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedSphereDefensiveArea(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Boolean,System.Boolean)
        void SetSphereDefensiveArea(float x, float y, float z, float radius, bool p5, bool p6);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedStayInVehicleWhenJacked(System.Int32,System.Boolean)
        void SetStayInVehicleWhenJacked(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedStealthMovement(System.Int32,System.Boolean,System.String)
        void SetStealthMovement(bool p1, string action);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedSteersAroundObjects(System.Int32,System.Boolean)
        void SetSteersAroundObjects(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedSteersAroundPeds(System.Int32,System.Boolean)
        void SetSteersAroundPeds(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedSteersAroundVehicles(System.Int32,System.Boolean)
        void SetSteersAroundVehicles(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedStrafeClipset(System.Int32,System.String)
        void SetStrafeClipset(string clipSet);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedSuffersCriticalHits(System.Int32,System.Boolean)
        void SetSuffersCriticalHits(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedSweat(System.Int32,System.Single)
        void SetSweat(float sweat);
        //
        // Summary:
        //     see RAGE.Game.Audio.SetPedTalk(System.Int32)
        void SetTalk();
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedTargetLossResponse(System.Int32,System.Int32)
        void SetTargetLossResponse(int responseType);
        //
        // Summary:
        //     see RAGE.Game.Vehicle.SetPedTargettableVehicleDestroy(System.Int32,System.Int32,System.Int32)
        void SetTargettableVehicleDestroy(int vehicleComponent, int destroyType);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetTaskVehicleChaseBehaviorFlag(System.Int32,System.Int32,System.Boolean)
        void SetTaskVehicleChaseBehaviorFlag(int flag, bool set);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetTaskVehicleChaseIdealPursuitDistance(System.Int32,System.Single)
        void SetTaskVehicleChaseIdealPursuitDistance(float distance);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedToInformRespectedFriends(System.Int32,System.Single,System.Int32)
        void SetToInformRespectedFriends(float radius, int maxFriends);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedToLoadCover(System.Int32,System.Boolean)
        void SetToLoadCover(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedToRagdoll(System.Int32,System.Int32,System.Int32,System.Int32,System.Boolean,System.Boolean,System.Boolean)
        bool SetToRagdoll(int time1, int time2, int ragdollType, bool p4, bool p5, bool p6);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedToRagdollWithFall(System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single)
        bool SetToRagdollWithFall(int time, int p2, int ragdollType, float x, float y, float z, float p7, float p8, float p9, float p10, float p11, float p12, float p13);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedUsingActionMode(System.Int32,System.Boolean,System.Int32,System.String)
        void SetUsingActionMode(bool p1, int p2, string action);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedVisualFieldCenterAngle(System.Int32,System.Single)
        void SetVisualFieldCenterAngle(float angle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedVisualFieldMaxAngle(System.Int32,System.Single)
        void SetVisualFieldMaxAngle(float value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedVisualFieldMaxElevationAngle(System.Int32,System.Single)
        void SetVisualFieldMaxElevationAngle(float angle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedVisualFieldMinAngle(System.Int32,System.Single)
        void SetVisualFieldMinAngle(float value);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedVisualFieldMinElevationAngle(System.Int32,System.Single)
        void SetVisualFieldMinElevationAngle(float angle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedVisualFieldPeripheralRange(System.Int32,System.Single)
        void SetVisualFieldPeripheralRange(float range);
        //
        // Summary:
        //     see RAGE.Game.Ai.SetPedWaypointRouteOffset(System.Int32,System.Int32,System.Int32,System.Int32)
        int SetWaypointRouteOffset(int p1, int p2, int p3);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetWeaponAnimationOverride(System.Int32,System.UInt32)
        void SetWeaponAnimationOverride(uint animStyle);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedWeaponMovementClipset(System.Int32,System.String)
        void SetWeaponMovementClipset(string clipSet);
        //
        // Summary:
        //     see RAGE.Game.Weapon.SetPedWeaponTintIndex(System.Int32,System.UInt32,System.Int32)
        void SetWeaponTintIndex(uint weaponHash, int tintIndex);
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedWetnessEnabledThisFrame(System.Int32)
        void SetWetnessEnabledThisFrame();
        //
        // Summary:
        //     see RAGE.Game.Ped.SetPedWetnessHeight(System.Int32,System.Single)
        void SetWetnessHeight(float height);
        //
        // Summary:
        //     see RAGE.Game.Weapon.PedSkipNextReloading(System.Int32)
        bool SkipNextReloading();
        //
        // Summary:
        //     see RAGE.Game.Ai.StopAnimPlayback(System.Int32,System.Int32,System.Boolean)
        void StopAnimPlayback(int p1, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Ai.StopAnimTask(System.Int32,System.String,System.String,System.Single)
        void StopAnimTask(string animDictionary, string animationName, float p3);
        //
        // Summary:
        //     see RAGE.Game.Audio.StopCurrentPlayingAmbientSpeech(System.Int32)
        void StopCurrentPlayingAmbientSpeech();
        //
        // Summary:
        //     see RAGE.Game.Audio.StopPedRingtone(System.Int32)
        void StopRingtone();
        //
        // Summary:
        //     see RAGE.Game.Audio.StopPedSpeaking(System.Int32,System.Boolean)
        void StopSpeaking(bool shaking);
        //
        // Summary:
        //     see RAGE.Game.Ped.StopPedWeaponFiringWhenDropped(System.Int32)
        void StopWeaponFiringWhenDropped();
        //
        // Summary:
        //     see RAGE.Game.Streaming.SwitchOutPlayer(System.Int32,System.Int32,System.Int32)
        void SwitchOutPlayer(int flags, int unknown);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskAchieveHeading(System.Int32,System.Single,System.Int32)
        void TaskAchieveHeading(float heading, int timeout);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskAimGunAtCoord(System.Int32,System.Single,System.Single,System.Single,System.Int32,System.Boolean,System.Boolean)
        void TaskAimGunAtCoord(float x, float y, float z, int time, bool p5, bool p6);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskAimGunAtEntity(System.Int32,System.Int32,System.Int32,System.Boolean)
        void TaskAimGunAtEntity(int entity, int duration, bool p3);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskAimGunScripted(System.Int32,System.UInt32,System.Boolean,System.Boolean)
        void TaskAimGunScripted(uint scriptTask, bool p2, bool p3);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskArrestPed(System.Int32,System.Int32)
        void TaskArrest(int target);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskChatToPed(System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single)
        void TaskChatTo(int target, int p2, float p3, float p4, float p5, float p6, float p7);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskClearLookAt(System.Int32)
        void TaskClearLookAt();
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskClimb(System.Int32,System.Boolean)
        void TaskClimb(bool unused);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskClimbLadder(System.Int32,System.Int32)
        void TaskClimbLadder(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskCombatPed(System.Int32,System.Int32,System.Int32,System.Int32)
        void TaskCombat(int targetPed, int p2, int p3);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskCombatHatedTargetsAroundPed(System.Int32,System.Single,System.Int32)
        void TaskCombatHatedTargetsAround(float radius, int p2);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskCombatHatedTargetsInArea(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32)
        void TaskCombatHatedTargetsInArea(float x, float y, float z, float radius, int p5);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskCower(System.Int32,System.Int32)
        void TaskCower(int duration);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskEnterVehicle(System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Int32,System.Int32)
        void TaskEnterVehicle(int vehicle, int timeout, int seat, float speed, int p5, int p6);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskFollowNavMeshToCoord(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Single,System.Boolean,System.Single)
        void TaskFollowNavMeshToCoord(float x, float y, float z, float speed, int timeout, float stoppingRange, bool persistFollowing, float unk);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskFollowNavMeshToCoordAdvanced(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Single,System.Int32,System.Single,System.Single,System.Single,System.Single)
        void TaskFollowNavMeshToCoordAdvanced(float x, float y, float z, float speed, int timeout, float unkFloat, int unkInt, float unkX, float unkY, float unkZ, float unk_40000f);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskFollowPointRoute(System.Int32,System.Single,System.Int32)
        void TaskFollowPointRoute(float speed, int unknown);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskFollowToOffsetOfEntity(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Single,System.Boolean)
        void TaskFollowToOffsetOfEntity(int entity, float offsetX, float offsetY, float offsetZ, float movementSpeed, int timeout, float stoppingRange, bool persistFollowing);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskForceMotionState(System.Int32,System.UInt32,System.Boolean)
        void TaskForceMotionState(uint state, bool p2);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskGetOffBoat(System.Int32,System.Int32)
        void TaskGetOffBoat(int boat);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskGoStraightToCoord(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Single,System.Single)
        void TaskGoStraightToCoord(float x, float y, float z, float speed, int timeout, float targetHeading, float distanceToSlide);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskGoToEntity(System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Int32)
        void TaskGoTo(int target, int duration, float distance, float speed, float p5, int p6);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskGoToCoordAnyMeans(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Boolean,System.Int32,System.Single)
        void TaskGoToCoordAnyMeans(float x, float y, float z, float speed, int p5, bool p6, int walkingStyle, float p8);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskGoToCoordAnyMeansExtraParams(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Boolean,System.Int32,System.Single,System.Int32,System.Int32,System.Int32,System.Int32)
        void TaskGoToCoordAnyMeansExtraParams(float x, float y, float z, float speed, int p5, bool p6, int walkingStyle, float p8, int p9, int p10, int p11, int p12);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskGoToCoordAnyMeansExtraParamsWithCruiseSpeed(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Boolean,System.Int32,System.Single,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)
        void TaskGoToCoordAnyMeansExtraParamsWithCruiseSpeed(float x, float y, float z, float speed, int p5, bool p6, int walkingStyle, float p8, int p9, int p10, int p11, int p12, int p13);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskGoToCoordWhileAimingAtCoord(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Boolean,System.Single,System.Single,System.Boolean,System.Int32,System.Boolean,System.UInt32)
        void TaskGoToCoordWhileAimingAtCoord(float x, float y, float z, float aimAtX, float aimAtY, float aimAtZ, float moveSpeed, bool p8, float p9, float p10, bool p11, int flags, bool p13, uint firingPattern);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskGotoEntityAiming(System.Int32,System.Int32,System.Single,System.Single)
        void TaskGotoEntityAiming(int target, float distanceToStopAt, float StartAimingDist);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskGotoEntityOffset(System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Int32)
        void TaskGotoEntityOffset(int p1, int p2, float x, float y, float z, int duration);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskGoToEntityWhileAimingAtEntity(System.Int32,System.Int32,System.Int32,System.Single,System.Boolean,System.Single,System.Single,System.Boolean,System.Boolean,System.UInt32)
        void TaskGoToEntityWhileAimingAtEntity(int entityToWalkTo, int entityToAimAt, float speed, bool shootatEntity, float p5, float p6, bool p7, bool p8, uint firingPattern);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskHandsUp(System.Int32,System.Int32,System.Int32,System.Int32,System.Boolean)
        void TaskHandsUp(int duration, int facingPed, int p3, bool p4);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskJump(System.Int32,System.Boolean,System.Int32,System.Int32)
        void TaskJump(bool unused, int p2, int p3);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskLeaveAnyVehicle(System.Int32,System.Int32,System.Int32)
        void TaskLeaveAnyVehicle(int p1, int p2);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskLeaveVehicle(System.Int32,System.Int32,System.Int32)
        void TaskLeaveVehicle(int vehicle, int flags);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskLookAtCoord(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Int32)
        void TaskLookAtCoord(float x, float y, float z, float duration, int p5, int p6);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskLookAtEntity(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)
        void TaskLookAtEntity(int lookAt, int duration, int unknown1, int unknown2);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskMoveNetwork(System.Int32,System.String,System.Single,System.Boolean,System.String,System.Int32)
        void TaskMoveNetwork(string task, float multiplier, bool p3, string animDict, int flags);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskMoveNetworkAdvanced(System.Int32,System.String,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Single,System.Boolean,System.String,System.Int32)
        void TaskMoveNetworkAdvanced(string p1, float p2, float p3, float p4, float p5, float p6, float p7, int p8, float p9, bool p10, string animDict, int flags);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskOpenVehicleDoor(System.Int32,System.Int32,System.Int32,System.Int32,System.Single)
        void TaskOpenVehicleDoor(int vehicle, int timeOut, int doorIndex, float speed);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskParachute(System.Int32,System.Boolean,System.Int32)
        void TaskParachute(bool p1, int p2);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskParachuteToTarget(System.Int32,System.Single,System.Single,System.Single)
        void TaskParachuteToTarget(float x, float y, float z);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPatrol(System.Int32,System.String,System.Int32,System.Boolean,System.Boolean)
        void TaskPatrol(string p1, int p2, bool p3, bool p4);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPause(System.Int32,System.Int32)
        void TaskPause(int ms);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPerformSequence(System.Int32,System.Int32)
        void TaskPerformSequence(int taskSequence);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPlantBomb(System.Int32,System.Single,System.Single,System.Single,System.Single)
        void TaskPlantBomb(float x, float y, float z, float heading);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPlayAnim(System.Int32,System.String,System.String,System.Single,System.Single,System.Int32,System.Int32,System.Single,System.Boolean,System.Boolean,System.Boolean)
        void TaskPlayAnim(string animDict, string animName, float speed, float speedMultiplier, int duration, int flat, int playbackRate, bool lockX, bool lockY, bool lockZ);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPlayAnimAdvanced(System.Int32,System.String,System.String,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Int32,System.Single,System.Int32,System.Int32)
        void TaskPlayAnimAdvanced(string animDict, string animName, float posX, float posY, float posZ, float rotX, float rotY, float rotZ, float speed, float speedMultiplier, int duration, int flag, float animTime, int p14, int p15);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPlayPhoneGestureAnimation(System.Int32,System.String,System.String,System.String,System.Single,System.Single,System.Boolean,System.Boolean)
        void TaskPlayPhoneGestureAnimation(string animDict, string animation, string boneMaskType, float p4, float p5, bool p6, bool p7);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPutPedDirectlyIntoCover(System.Int32,System.Single,System.Single,System.Single,System.Int32,System.Boolean,System.Single,System.Boolean,System.Boolean,System.Int32,System.Boolean)
        void TaskPutDirectlyIntoCover(float x, float y, float z, int timeout, bool p5, float p6, bool p7, bool p8, int p9, bool p10);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPutPedDirectlyIntoMelee(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Boolean)
        void TaskPutDirectlyIntoMelee(int meleeTarget, float p2, float p3, float p4, bool p5);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskRappelFromHeli(System.Int32,System.Int32)
        void TaskRappelFromHeli(int unused);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskReactAndFleePed(System.Int32,System.Int32)
        void TaskReactAndFlee(int fleeTarget);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskReloadWeapon(System.Int32,System.Boolean)
        void TaskReloadWeapon(bool unused);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskScriptedAnimation(System.Int32,System.Int32@,System.Int32@,System.Int32@,System.Single,System.Single)
        void TaskScriptedAnimation(ref int p1, ref int p2, ref int p3, float p4, float p5);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskSeekCoverFromPed(System.Int32,System.Int32,System.Int32,System.Boolean)
        void TaskSeekCoverFrom(int target, int duration, bool p3);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskSeekCoverFromPos(System.Int32,System.Single,System.Single,System.Single,System.Int32,System.Boolean)
        void TaskSeekCoverFromPos(float x, float y, float z, int duration, bool p5);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskSeekCoverToCoords(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Boolean)
        void TaskSeekCoverToCoords(float x1, float y1, float z1, float x2, float y2, float z2, int p7, bool p8);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskSetBlockingOfNonTemporaryEvents(System.Int32,System.Boolean)
        void TaskSetBlockingOfNonTemporaryEvents(bool toggle);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskShockingEventReact(System.Int32,System.Int32)
        void TaskShockingEventReact(int eventHandle);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskShootAtEntity(System.Int32,System.Int32,System.Int32,System.UInt32)
        void TaskShootAt(int target, int duration, uint firingPattern);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskShootAtCoord(System.Int32,System.Single,System.Single,System.Single,System.Int32,System.UInt32)
        void TaskShootAtCoord(float x, float y, float z, int duration, uint firingPattern);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskShuffleToNextVehicleSeat(System.Int32,System.Int32,System.Int32)
        void TaskShuffleToNextVehicleSeat(int vehicle, int p2);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskSkyDive(System.Int32,System.Int32)
        void TaskSkyDive(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPedSlideToCoord(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single)
        void TaskSlideToCoord(float x, float y, float z, float heading, float p5);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskPedSlideToCoordHdgRate(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single)
        void TaskSlideToCoordHdgRate(float x, float y, float z, float heading, float p5, float p6);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskSmartFleePed(System.Int32,System.Int32,System.Single,System.Int32,System.Boolean,System.Boolean)
        void TaskSmartFlee(int fleeTarget, float distance, int fleeTime, bool p4, bool p5);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskSmartFleeCoord(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Boolean,System.Boolean)
        void TaskSmartFleeCoord(float x, float y, float z, float distance, int time, bool p6, bool p7);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskStandGuard(System.Int32,System.Single,System.Single,System.Single,System.Single,System.String)
        void TaskStandGuard(float x, float y, float z, float heading, string scenarioName);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskStandStill(System.Int32,System.Int32)
        void TaskStandStill(int time);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskStartScenarioAtPosition(System.Int32,System.String,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Boolean,System.Boolean)
        void TaskStartScenarioAtPosition(string scenarioName, float x, float y, float z, float heading, int duration, bool sittingScenario, bool teleport);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskStartScenarioInPlace(System.Int32,System.String,System.Int32,System.Boolean)
        void TaskStartScenarioInPlace(string scenarioName, int unkDelay, bool playEnterAnim);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskStayInCover(System.Int32)
        void TaskStayInCover();
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskStopPhoneGestureAnimation(System.Int32,System.Int32)
        void TaskStopPhoneGestureAnimation(int p1);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskSwapWeapon(System.Int32,System.Boolean)
        void TaskSwapWeapon(bool p1);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskSweepAimEntity(System.Int32,System.String,System.String,System.String,System.String,System.Int32,System.Int32,System.Single,System.Single)
        void TaskSweepAimEntity(string anim, string p2, string p3, string p4, int p5, int vehicle, float p7, float p8);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskSynchronizedScene(System.Int32,System.Int32,System.String,System.String,System.Single,System.Single,System.Int32,System.Int32,System.Single,System.Int32)
        void TaskSynchronizedScene(int scene, string animDictionary, string animationName, float speed, float speedMultiplier, int duration, int flag, float playbackRate, int p9);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskThrowProjectile(System.Int32,System.Single,System.Single,System.Single,System.Int32,System.Int32)
        void TaskThrowProjectile(float x, float y, float z, int p4, int p5);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskTurnPedToFaceCoord(System.Int32,System.Single,System.Single,System.Single,System.Int32)
        void TaskTurnToFaceCoord(float x, float y, float z, int duration);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskTurnPedToFaceEntity(System.Int32,System.Int32,System.Int32)
        void TaskTurnToFaceEntity(int entity, int duration);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskUseMobilePhone(System.Int32,System.Int32,System.Int32)
        void TaskUseMobilePhone(int p1, int p2);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskUseMobilePhoneTimed(System.Int32,System.Int32)
        void TaskUseMobilePhoneTimed(int duration);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskUseNearestScenarioToCoord(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32)
        void TaskUseNearestScenarioToCoord(float x, float y, float z, float distance, int duration);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskUseNearestScenarioToCoordWarp(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32)
        void TaskUseNearestScenarioToCoordWarp(float x, float y, float z, float radius, int p5);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleAimAtPed(System.Int32,System.Int32)
        void TaskVehicleAimAt(int target);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleAimAtCoord(System.Int32,System.Single,System.Single,System.Single)
        void TaskVehicleAimAtCoord(float x, float y, float z);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleDriveToCoord(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.UInt32,System.Int32,System.Single,System.Single)
        void TaskVehicleDriveToCoord(int vehicle, float x, float y, float z, float speed, int p6, uint vehicleModel, int drivingMode, float stopRange, float p10);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleDriveToCoordLongrange(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Single)
        void TaskVehicleDriveToCoordLongrange(int vehicle, float x, float y, float z, float speed, int driveMode, float stopRange);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleDriveWander(System.Int32,System.Int32,System.Single,System.Int32)
        void TaskVehicleDriveWander(int vehicle, float speed, int drivingStyle);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleEscort(System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Int32,System.Single,System.Int32,System.Single)
        void TaskVehicleEscort(int vehicle, int targetVehicle, int mode, float speed, int drivingStyle, float minDistance, int p7, float noRoadsDistance);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleFollowWaypointRecording(System.Int32,System.Int32,System.String,System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Boolean,System.Single)
        void TaskVehicleFollowWaypointRecording(int vehicle, string WPRecording, int p3, int p4, int p5, int p6, float p7, bool p8, float p9);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleGotoNavmesh(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Single)
        void TaskVehicleGotoNavmesh(int vehicle, float x, float y, float z, float speed, int behaviorFlag, float stoppingRange);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleMissionCoorsTarget(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Boolean)
        void TaskVehicleMissionCoorsTarget(int vehicle, float x, float y, float z, int p5, int p6, int p7, float p8, float p9, bool p10);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleMissionPedTarget(System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Int32,System.Single,System.Single,System.Boolean)
        void TaskVehicleMissionTarget(int vehicle, int pedTarget, int mode, float maxSpeed, int drivingStyle, float minDistance, float p7, bool p8);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehiclePark(System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32,System.Single,System.Boolean)
        void TaskVehiclePark(int vehicle, float x, float y, float z, float heading, int mode, float radius, bool keepEngineOn);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleShootAtPed(System.Int32,System.Int32,System.Single)
        void TaskVehicleShootAt(int target, float p2);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskVehicleShootAtCoord(System.Int32,System.Single,System.Single,System.Single,System.Single)
        void TaskVehicleShootAtCoord(float x, float y, float z, float p4);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskWanderInArea(System.Int32,System.Single,System.Single,System.Single,System.Single,System.Single,System.Single)
        void TaskWanderInArea(float x, float y, float z, float radius, float minimalLength, float timeBetweenWalks);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskWanderStandard(System.Int32,System.Single,System.Int32)
        void TaskWanderStandard(float p1, int p2);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskWarpPedIntoVehicle(System.Int32,System.Int32,System.Int32)
        void TaskWarpIntoVehicle(int vehicle, int seat);
        //
        // Summary:
        //     see RAGE.Game.Ai.TaskWrithe(System.Int32,System.Int32,System.Int32,System.Int32,System.Int32,System.Int32)
        void TaskWrithe(int target, int time, int p3, int p4, int p5);
        //
        // Summary:
        //     see RAGE.Game.Network.PedToNet(System.Int32)
        int ToNet();
        //
        // Summary:
        //     see RAGE.Game.Ai.UncuffPed(System.Int32)
        void Uncuff();
        //
        // Summary:
        //     see RAGE.Game.Ped.UpdatePedHeadBlendData(System.Int32,System.Single,System.Single,System.Single)
        void UpdateHeadBlendData(float shapeMix, float skinMix, float thirdMix);
        //
        // Summary:
        //     see RAGE.Game.Ai.UpdateTaskHandsUpDuration(System.Int32,System.Int32)
        void UpdateTaskHandsUpDuration(int duration);
        //
        // Summary:
        //     see RAGE.Game.Ai.UpdateTaskSweepAimEntity(System.Int32,System.Int32)
        void UpdateTaskSweepAimEntity(int entity);
        //
        // Summary:
        //     see RAGE.Game.Ped.WasPedKilledByStealth(System.Int32)
        bool WasKilledByStealth();
        //
        // Summary:
        //     see RAGE.Game.Ped.WasPedKilledByTakedown(System.Int32)
        bool WasKilledByTakedown();
        //
        // Summary:
        //     see RAGE.Game.Ped.WasPedSkeletonUpdated(System.Int32)
        bool WasSkeletonUpdated();
    }
}
