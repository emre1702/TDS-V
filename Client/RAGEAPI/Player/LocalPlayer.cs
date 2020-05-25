using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Player
{
    internal class LocalPlayer : Player, ILocalPlayer
    {
        #region Private Fields

        private readonly EntityConvertingHandler _entityConvertingHandler;
        private readonly RAGE.Elements.Player _instance;

        #endregion Private Fields

        #region Public Constructors

        public LocalPlayer(RAGE.Elements.Player player, EntityConvertingHandler entityConvertingHandler)
            : base(player, entityConvertingHandler)
            => (_instance, _entityConvertingHandler) = (player, entityConvertingHandler);

        #endregion Public Constructors

        #region Public Methods

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

        public void Call(string eventName, params object[] args)
            => _instance.Call(eventName, args);

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

        public void ExpandWorldLimits(float x, float y, float z)
            => RAGE.Game.Player.ExpandWorldLimits(x, y, z);

        public void ForceCleanup(int cleanupFlags)
            => RAGE.Game.Player.ForceCleanup(cleanupFlags);

        public void ForceCleanupForAllThreadsWithThisName(string name, int cleanupFlags)
            => RAGE.Game.Player.ForceCleanupForAllThreadsWithThisName(name, cleanupFlags);

        public void ForceCleanupForThreadWithThisId(int id, int cleanupFlags)
            => RAGE.Game.Player.ForceCleanupForThreadWithThisId(id, cleanupFlags);

        public int GetCauseOfMostRecentForceCleanup()
            => RAGE.Game.Player.GetCauseOfMostRecentForceCleanup();

        public float GetCurrentStealthNoise()
            => RAGE.Game.Player.GetPlayerCurrentStealthNoise();

        public int GetGroup()
            => RAGE.Game.Player.GetPlayerGroup();

        public bool GetHasReserveParachute()
            => RAGE.Game.Player.GetPlayerHasReserveParachute();

        public int GetIndex()
            => RAGE.Game.Player.GetPlayerIndex();

        public bool GetInvincible()
            => RAGE.Game.Player.GetPlayerInvincible();

        public bool GetIsFreeAimingAt(ref int entity)
            => RAGE.Game.Player.GetEntityPlayerIsFreeAimingAt(ref entity);

        public int GetLastVehicle()
            => RAGE.Game.Player.GetPlayersLastVehicle();

        public int GetMaxArmour()
            => RAGE.Game.Player.GetPlayerMaxArmour();

        public int GetMaxWantedLevel()
            => RAGE.Game.Player.GetMaxWantedLevel();

        public string GetName()
            => RAGE.Game.Player.GetPlayerName();

        public void GetParachutePackTintIndex(ref int tintIndex)
            => RAGE.Game.Player.GetPlayerParachutePackTintIndex(ref tintIndex);

        public void GetParachuteSmokeTrailColor(ref int r, ref int g, ref int b)
            => RAGE.Game.Player.GetPlayerParachuteSmokeTrailColor(ref r, ref g, ref b);

        public void GetParachuteTintIndex(ref int tintIndex)
            => RAGE.Game.Player.GetPlayerParachuteTintIndex(ref tintIndex);

        public int GetPed()
            => RAGE.Game.Player.GetPlayerPed();

        public int GetPedScriptIndex()
            => RAGE.Game.Player.GetPlayerPedScriptIndex();

        public void GetReserveParachuteTintIndex(ref int index)
            => RAGE.Game.Player.GetPlayerReserveParachuteTintIndex(ref index);

        public void GetRgbColour(ref int r, ref int g, ref int b)
            => RAGE.Game.Player.GetPlayerRgbColour(ref r, ref g, ref b);

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

        public Position3D GetWantedCentrePosition()
            => RAGE.Game.Player.GetPlayerWantedCentrePosition().ToPosition3D();

        public int GetWantedLevel()
            => RAGE.Game.Player.GetPlayerWantedLevel();

        public float GetWantedLevelRadius()
            => RAGE.Game.Player.GetWantedLevelRadius();

        public int GetWantedLevelThreshold(int wantedLevel)
            => RAGE.Game.Player.GetWantedLevelThreshold(wantedLevel);

        public bool GiveAchievement(int achievement)
            => RAGE.Game.Player.GiveAchievementToPlayer(achievement);

        public void GiveRagdollControl(bool toggle)
            => RAGE.Game.Player.GivePlayerRagdollControl(toggle);

        public bool HasAchievementBeenPassed(int achievement)
            => RAGE.Game.Player.HasAchievementBeenPassed(achievement);

        public bool HasBeenSpottedInStolenVehicle()
            => RAGE.Game.Player.HasPlayerBeenSpottedInStolenVehicle();

        public bool HasDamagedAtLeastOneNonAnimalPed()
            => RAGE.Game.Player.HasPlayerDamagedAtLeastOneNonAnimalPed();

        public bool HasDamagedAtLeastOnePed()
            => RAGE.Game.Player.HasPlayerDamagedAtLeastOnePed();

        public bool HasForceCleanupOccurred(int cleanupFlags)
            => RAGE.Game.Player.HasForceCleanupOccurred(cleanupFlags);

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

        public bool IsFreeAiming()
            => RAGE.Game.Player.IsPlayerFreeAiming();

        public bool IsFreeAimingAtEntity(int entity)
            => RAGE.Game.Player.IsPlayerFreeAimingAtEntity(entity);

        public bool IsFreeForAmbientTask()
            => RAGE.Game.Player.IsPlayerFreeForAmbientTask();

        public bool IsLoggingInNp()
            => RAGE.Game.Player.IsPlayerLoggingInNp();

        public bool IsPlaying()
            => RAGE.Game.Player.IsPlayerPlaying();

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

        public int NetworkPlayerIdToInt()
            => RAGE.Game.Player.NetworkPlayerIdToInt();

        public int PlayerId()
            => RAGE.Game.Player.PlayerId();

        public int PlayerPedId()
            => RAGE.Game.Player.PlayerPedId();

        public new void RemoveHelmet(bool p2)
            => RAGE.Game.Player.RemovePlayerHelmet(p2);

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

        public void SetAutoGiveParachuteWhenEnterPlane(bool toggle)
            => RAGE.Game.Player.SetAutoGiveParachuteWhenEnterPlane(toggle);

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

        public void SetDisableAmbientMeleeMove(bool toggle)
            => RAGE.Game.Player.SetDisableAmbientMeleeMove(toggle);

        public void SetDispatchCops(bool toggle)
            => RAGE.Game.Player.SetDispatchCopsForPlayer(toggle);

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

        public void SetHealthRechargeMultiplier(float regenRate)
            => RAGE.Game.Player.SetPlayerHealthRechargeMultiplier(regenRate);

        public void SetIgnoreLowPriorityShockingEvents(bool toggle)
            => RAGE.Game.Player.SetIgnoreLowPriorityShockingEvents(toggle);

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

        #endregion Public Methods
    }
}
