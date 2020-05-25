using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Interfaces.ModAPI.Vehicle;
using TDS_Client.RAGEAPI.Entity;
using TDS_Client.RAGEAPI.Extensions;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.RAGEAPI.Vehicle
{
    internal class Vehicle : Entity.EntityBase, IVehicle
    {
        #region Private Fields

        private readonly EntityConvertingHandler _entityConvertingHandler;
        private readonly RAGE.Elements.Vehicle _instance;

        #endregion Private Fields

        #region Public Constructors

        public Vehicle(RAGE.Elements.Vehicle instance, EntityConvertingHandler entityConvertingHandler) : base(instance)
            => (_instance, _entityConvertingHandler) = (instance, entityConvertingHandler);

        #endregion Public Constructors

        #region Public Properties

        public IPlayer Controller => _instance.Controller is null ? null : _entityConvertingHandler.GetEntity(_instance.Controller);

        #endregion Public Properties

        #region Public Methods

        public bool AddClanDecalTo(int ped, int boneIndex, float x1, float x2, float x3, float y1, float y2, float y3, float z1, float z2, float z3, float scale, int p13, int alpha)
            => _instance.AddClanDecalTo(ped, boneIndex, x1, x2, x3, y1, y2, y3, z1, z2, z3, scale, p13, alpha);

        public void AddUpsidedownCheck()
            => _instance.AddUpsidedownCheck();

        public bool AnyPassengersRappeling()
            => _instance.AnyPassengersRappeling();

        public bool AreAllWindowsIntact()
            => _instance.AreAllWindowsIntact();

        public bool AreAnySeatsFree()
            => _instance.AreAnySeatsFree();

        public bool ArePropellersUndamaged()
            => _instance.ArePropellersUndamaged();

        public bool AreWingsIntact()
            => _instance.AreWingsIntact();

        public void AttachToCargobob(int cargobob, int p2, float x, float y, float z)
            => _instance.AttachToCargobob(cargobob, p2, x, y, z);

        public void AttachToTrailer(int trailer, float radius)
            => _instance.AttachToTrailer(trailer, radius);

        public void BlipSiren()
            => _instance.BlipSiren();

        public int CanParachuteBeActivated()
            => _instance.CanParachuteBeActivated();

        public bool CanShuffleSeat(int p1)
            => _instance.CanShuffleSeat(p1);

        public void ClearCustomPrimaryColour()
            => _instance.ClearCustomPrimaryColour();

        public void ClearCustomSecondaryColour()
            => _instance.ClearCustomSecondaryColour();

        public void CloseBombBayDoors()
            => _instance.CloseBombBayDoors();

        public void ControlLandingGear(int state)
            => _instance.ControlLandingGear(state);

        public int CreatePedInside(int pedType, uint modelHash, int seat, bool isNetwork, bool p5)
            => _instance.CreatePedInside(pedType, modelHash, seat, isNetwork, p5);

        public int CreateRandomPedAsDriver(bool returnHandle)
            => _instance.CreateRandomPedAsDriver(returnHandle);

        public bool DetachFromAnyCargobob()
            => _instance.DetachFromAnyCargobob();

        public bool DetachFromAnyTowTruck()
            => _instance.DetachFromAnyTowTruck();

        public void DetachFromCargobob(int cargobob)
            => _instance.DetachFromCargobob(cargobob);

        public void DetachFromTrailer()
            => _instance.DetachFromTrailer();

        public void DetachWindscreen()
            => _instance.DetachWindscreen();

        public void DisablePlaneAileron(bool p1, bool p2)
            => _instance.DisablePlaneAileron(p1, p2);

        public bool DoesExtraExist(int extraId)
            => _instance.DoesExtraExist(extraId);

        public bool DoesHaveDecal(int p1)
            => _instance.DoesHaveDecal(p1);

        public int DoesHaveDoor(int doorIndex)
            => _instance.DoesHaveDoor(doorIndex);

        public bool DoesHaveRoof()
            => _instance.DoesHaveRoof();

        public bool DoesHaveStuckVehicleCheck()
            => _instance.DoesHaveStuckVehicleCheck();

        public bool DoesHaveWeapons()
            => _instance.DoesHaveWeapons();

        public void EjectJb700Roof(float x, float y, float z)
            => _instance.EjectJb700Roof(x, y, z);

        public void Explode(bool isAudible, bool isInvisible)
            => _instance.Explode(isAudible, isInvisible);

        public void ExplodeInCutscene(bool p1)
            => _instance.ExplodeInCutscene(p1);

        public void FixWindow(int index)
            => _instance.FixWindow(index);

        public float GetAcceleration()
            => _instance.GetAcceleration();

        public bool GetBoatAnchor()
            => _instance.GetBoatAnchor();

        public float GetBodyHealth()
            => _instance.GetBodyHealth();

        public float GetBodyHealth2(int p1, int p2, int p3, int p4, int p5, int p6)
            => _instance.GetBodyHealth2(p1, p2, p3, p4, p5, p6);

        public uint GetCauseOfDestruction()
            => _instance.GetCauseOfDestruction();

        public int GetClass()
            => _instance.GetClass();

        public void GetColor(ref int r, ref int g, ref int b)
            => _instance.GetColor(ref r, ref g, ref b);

        public int GetColourCombination()
            => _instance.GetColourCombination();

        public void GetColours(ref int colorPrimary, ref int colorSecondary)
            => _instance.GetColours(ref colorPrimary, ref colorSecondary);

        public int GetConvertibleRoofState()
            => _instance.GetConvertibleRoofState();

        public void GetCustomPrimaryColour(ref int r, ref int g, ref int b)
            => _instance.GetCustomPrimaryColour(ref r, ref g, ref b);

        public void GetCustomSecondaryColour(ref int r, ref int g, ref int b)
            => _instance.GetCustomSecondaryColour(ref r, ref g, ref b);

        public void GetDashboardColour(ref int color)
            => _instance.GetDashboardColour(ref color);

        public uint GetDefaultHorn()
            => _instance.GetDefaultHorn();

        public Position3D GetDeformationAtPos(float offsetX, float offsetY, float offsetZ)
            => _instance.GetDeformationAtPos(offsetX, offsetY, offsetZ).ToPosition3D();

        public float GetDirtLevel()
            => _instance.GetDirtLevel();

        public float GetDoorAngleRatio(int door)
            => _instance.GetDoorAngleRatio(door);

        public int GetDoorLockStatus()
            => _instance.GetDoorLockStatus();

        public bool GetDoorsLockedForPlayer(int player)
            => _instance.GetDoorsLockedForPlayer(player);

        public float GetEngineHealth()
            => _instance.GetEngineHealth();

        public Position3D GetEntryPositionOfDoor(int doorIndex)
            => _instance.GetEntryPositionOfDoor(doorIndex).ToPosition3D();

        public float GetEnveffScale()
            => _instance.GetEnveffScale();

        public void GetExtraColours(ref int pearlescentColor, ref int wheelColor)
            => _instance.GetExtraColours(ref pearlescentColor, ref wheelColor);

        public int GetHasLowerableWheels()
            => _instance.GetHasLowerableWheels();

        public float GetHeliEngineHealth()
            => _instance.GetHeliEngineHealth();

        public float GetHeliMainRotorHealth()
            => _instance.GetHeliMainRotorHealth();

        public float GetHeliTailRotorHealth()
            => _instance.GetHeliTailRotorHealth();

        public uint GetHornHash()
            => _instance.GetHornHash();

        public void GetInteriorColour(ref int color)
            => _instance.GetInteriorColour(ref color);

        public bool GetIsEngineRunning()
            => _instance.GetIsEngineRunning();

        public bool GetIsLeftHeadlightDamaged()
            => _instance.GetIsLeftHeadlightDamaged();

        public bool GetIsPrimaryColourCustom()
            => _instance.GetIsPrimaryColourCustom();

        public bool GetIsRightHeadlightDamaged()
            => _instance.GetIsRightHeadlightDamaged();

        public bool GetIsSecondaryColourCustom()
            => _instance.GetIsSecondaryColourCustom();

        public int GetLandingGearState()
            => _instance.GetLandingGearState();

        public int GetLastPedInSeat(int seatIndex)
            => _instance.GetLastPedInSeat(seatIndex);

        public uint GetLayoutHash()
            => _instance.GetLayoutHash();

        public bool GetLightsState(ref int lightsOn, ref int highbeamsOn)
            => _instance.GetLightsState(ref lightsOn, ref highbeamsOn);

        public int GetLivery()
            => _instance.GetLivery();

        public int GetLiveryCount()
            => _instance.GetLiveryCount();

        public string GetLiveryName(int liveryIndex)
            => _instance.GetLiveryName(liveryIndex);

        public float GetMaxBraking()
            => _instance.GetMaxBraking();

        public int GetMaxNumberOfPassengers()
            => _instance.GetMaxNumberOfPassengers();

        public float GetMaxTraction()
            => _instance.GetMaxTraction();

        public int GetMod(int modType)
            => _instance.GetMod(modType);

        public void GetModColor1(ref int paintType, ref int color, ref int p3)
            => _instance.GetModColor1(ref paintType, ref color, ref p3);

        public string GetModColor1Name(bool p1)
            => _instance.GetModColor1Name(p1);

        public void GetModColor2(ref int paintType, ref int color)
            => _instance.GetModColor2(ref paintType, ref color);

        public string GetModColor2Name()
            => _instance.GetModColor2Name();

        public int GetModData(int modType, int modIndex)
            => _instance.GetModData(modType, modIndex);

        public int GetModKit()
            => _instance.GetModKit();

        public int GetModKitType()
            => _instance.GetModKitType();

        public int GetModModifierValue(int modType, int modIndex)
            => _instance.GetModModifierValue(modType, modIndex);

        public string GetModSlotName(int modType)
            => _instance.GetModSlotName(modType);

        public string GetModTextLabel(int modType, int modValue)
            => _instance.GetModTextLabel(modType, modValue);

        public bool GetModVariation(int modType)
            => _instance.GetModVariation(modType);

        public void GetNeonLightsColour(ref int r, ref int g, ref int b)
            => _instance.GetNeonLightsColour(ref r, ref g, ref b);

        public int GetNumberOfColours()
            => _instance.GetNumberOfColours();

        public int GetNumberOfDoors()
            => _instance.GetNumberOfDoors();

        public int GetNumberOfPassengers()
            => _instance.GetNumberOfPassengers();

        public string GetNumberPlateText()
            => _instance.GetNumberPlateText();

        public int GetNumberPlateTextIndex()
            => _instance.GetNumberPlateTextIndex();

        public int GetNumModKits()
            => _instance.GetNumModKits();

        public int GetNumMods(int modType)
            => _instance.GetNumMods(modType);

        public bool GetOwner(ref int entity)
            => _instance.GetOwner(ref entity);

        public int GetPedInSeat(int index, int p2)
            => _instance.GetPedInSeat(index, p2);

        public int GetPedUsingDoor(int doorIndex)
            => _instance.GetPedUsingDoor(doorIndex);

        public float GetPetrolTankHealth()
            => _instance.GetPetrolTankHealth();

        public int GetPlateType()
            => _instance.GetPlateType();

        public float GetSuspensionHeight()
            => _instance.GetSuspensionHeight();

        public bool GetTrailerVehicle(ref int trailer)
            => _instance.GetTrailerVehicle(ref trailer);

        public bool GetTyresCanBurst()
            => _instance.GetTyresCanBurst();

        public void GetTyreSmokeColor(ref int r, ref int g, ref int b)
            => _instance.GetTyreSmokeColor(ref r, ref g, ref b);

        public int GetWheelType()
            => _instance.GetWheelType();

        public int GetWindowTint()
            => _instance.GetWindowTint();

        public int HasJumpingAbility()
            => _instance.HasJumpingAbility();

        public bool HasLandingGear()
            => _instance.HasLandingGear();

        public int HasParachute()
            => _instance.HasParachute();

        public int HasRocketBoost()
            => _instance.HasRocketBoost();

        public bool IsAConvertible(bool p1)
            => _instance.IsAConvertible(p1);

        public bool IsAlarmActivated()
            => _instance.IsAlarmActivated();

        public bool IsAttachedToCargobob(int vehicleAttached)
            => _instance.IsAttachedToCargobob(vehicleAttached);

        public bool IsAttachedToTowTruck(int vehicle)
            => _instance.IsAttachedToTowTruck(vehicle);

        public bool IsAttachedToTrailer()
            => _instance.IsAttachedToTrailer();

        public bool IsBig()
            => _instance.IsBig();

        public bool IsBumperBrokenOff(bool front)
            => _instance.IsBumperBrokenOff(front);

        public bool IsDamaged()
            => _instance.IsDamaged();

        public bool IsDoorDamaged(int doorID)
            => _instance.IsDoorDamaged(doorID);

        public bool IsDoorFullyOpen(int doorIndex)
            => _instance.IsDoorFullyOpen(doorIndex);

        public bool IsDriveable(bool isOnFireCheck)
            => _instance.IsDriveable(isOnFireCheck);

        public bool IsExtraTurnedOn(int extraId)
            => _instance.IsExtraTurnedOn(extraId);

        public bool IsHeliPartBroken(bool p1, bool p2, bool p3)
            => _instance.IsHeliPartBroken(p1, p2, p3);

        public bool IsHighDetail()
            => _instance.IsHighDetail();

        public bool IsHornActive()
            => _instance.IsHornActive();

        public bool IsInBurnout()
            => _instance.IsInBurnout();

        public bool IsModel(uint model)
            => _instance.IsModel(model);

        public bool IsModLoadDone()
            => _instance.IsModLoadDone();

        public bool IsNearEntity(int entity)
            => _instance.IsNearEntity(entity);

        public bool IsNeonLightEnabled(int index)
            => _instance.IsNeonLightEnabled(index);

        public bool IsNodeIdValid()
            => _instance.IsNodeIdValid();

        public bool IsOnAllWheels()
            => _instance.IsOnAllWheels();

        public bool IsRadioLoud()
            => _instance.IsRadioLoud();

        public int IsRocketBoostActive()
            => _instance.IsRocketBoostActive();

        public bool IsSearchlightOn()
            => _instance.IsSearchlightOn();

        public bool IsSeatFree(VehicleSeat seat)
            => _instance.IsSeatFree((int)seat - 1, 1);

        public bool IsShopResprayAllowed()
            => _instance.IsShopResprayAllowed();

        public bool IsSirenOn()
            => _instance.IsSirenOn();

        public bool IsSirenSoundOn()
            => _instance.IsSirenSoundOn();

        public bool IsStolen()
            => _instance.IsStolen();

        public bool IsStopped()
            => _instance.IsStopped();

        public bool IsStoppedAtTrafficLights()
            => _instance.IsStoppedAtTrafficLights();

        public bool IsStuckOnRoof()
            => _instance.IsStuckOnRoof();

        public bool IsStuckTimerUp(int p1, int p2)
            => _instance.IsStuckTimerUp(p1, p2);

        public bool IsTaxiLightOn()
            => _instance.IsTaxiLightOn();

        public bool IsToggleModOn(int modType)
            => _instance.IsToggleModOn(modType);

        public bool IsTyreBurst(int wheelID, bool completely)
            => _instance.IsTyreBurst(wheelID, completely);

        public bool IsWindowIntact(int windowIndex)
            => _instance.IsWindowIntact(windowIndex);

        public void Jitter(bool p1, float yaw, float pitch, float roll)
            => _instance.Jitter(p1, yaw, pitch, roll);

        public void LowerConvertibleRoof(bool instantlyLower)
        => _instance.LowerConvertibleRoof(instantlyLower);

        public int NetworkExplode(bool isAudible, bool isInvisible, bool p3)
        => _instance.NetworkExplode(isAudible, isInvisible, p3);

        public void OpenBombBayDoors()
        => _instance.OpenBombBayDoors();

        public void OverrideVehHorn(bool mute, int p2)
            => _instance.OverrideVehHorn(mute, p2);

        public void PlayDoorCloseSound(int p1)
            => _instance.PlayDoorCloseSound(p1);

        public void PlayDoorOpenSound(int p1)
            => _instance.PlayDoorOpenSound(p1);

        public void PlayStreamFrom()
            => _instance.PlayStreamFrom();

        public void RaiseConvertibleRoof(bool instantlyRaise)
            => _instance.RaiseConvertibleRoof(instantlyRaise);

        public void RaiseLowerableWheels()
            => _instance.RaiseLowerableWheels();

        public void ReleasePreloadMods()
            => _instance.ReleasePreloadMods();

        public void RemoveDecalsFrom()
            => _instance.RemoveDecalsFrom();

        public void RemoveHighDetailModel()
            => _instance.RemoveHighDetailModel();

        public void RemoveMod(int modType)
            => _instance.RemoveMod(modType);

        public void RemoveStuckCheck()
            => _instance.RemoveStuckCheck();

        public void RemoveUpsidedownCheck()
            => _instance.RemoveUpsidedownCheck();

        public void RemoveWindow(int windowIndex)
            => _instance.RemoveWindow(windowIndex);

        public void RequestHighDetailModel()
            => _instance.RequestHighDetailModel();

        public void ResetStuckTimer(int nullAttributes)
            => _instance.ResetStuckTimer(nullAttributes);

        public void ResetWheels(bool toggle)
            => _instance.ResetWheels(toggle);

        public void RollDownWindow(int windowIndex)
            => _instance.RollDownWindow(windowIndex);

        public void RollDownWindows()
            => _instance.RollDownWindows();

        public void RollUpWindow(int windowIndex)
            => _instance.RollUpWindow(windowIndex);

        public void SetAlarm(bool state)
            => _instance.SetAlarm(state);

        public void SetAllowNoPassengersLockon(bool toggle)
            => _instance.SetAllowNoPassengersLockon(toggle);

        public void SetAudio(string audioName)
            => _instance.SetAudio(audioName);

        public void SetAudioPriority(int p1)
            => _instance.SetAudioPriority(p1);

        public int SetAutomaticallyAttaches(int p1, int p2)
            => _instance.SetAutomaticallyAttaches(p1, p2);

        public void SetBikeLeanAngle(float x, float y)
            => _instance.SetBikeLeanAngle(x, y);

        public void SetBoatAnchor(bool toggle)
            => _instance.SetBoatAnchor(toggle);

        public void SetBodyHealth(float value)
            => _instance.SetBodyHealth(value);

        public void SetBombs(int amount)
            => _instance.SetBombs(amount);

        public int SetBombs()
            => _instance.SetBombs();

        public void SetBoostActive(bool toggle)
            => _instance.SetBoostActive(toggle);

        public void SetBrakeLights(bool toggle)
            => _instance.SetBrakeLights(toggle);

        public void SetBurnout(bool toggle)
            => _instance.SetBurnout(toggle);

        public void SetCanBeTargetted(bool state)
            => _instance.SetCanBeTargetted(state);

        public void SetCanBeUsedByFleeingPeds(bool toggle)
            => _instance.SetCanBeUsedByFleeingPeds(toggle);

        public void SetCanBeVisiblyDamaged(bool state)
            => _instance.SetCanBeVisiblyDamaged(state);

        public void SetCanBreak(bool toggle)
            => _instance.SetCanBreak(toggle);

        public void SetCanRespray(bool state)
            => _instance.SetCanRespray(state);

        public void SetCeilingHeight(float p1)
            => _instance.SetCeilingHeight(p1);

        public void SetColourCombination(int colorCombination)
            => _instance.SetColourCombination(colorCombination);

        public void SetColours(int colorPrimary, int colorSecondary)
            => _instance.SetColours(colorPrimary, colorSecondary);

        public void SetConvertibleRoof(bool p1)
            => _instance.SetConvertibleRoof(p1);

        public void SetCountermeasures(int amount)
            => _instance.SetCountermeasures(amount);

        public void SetCreatesMoneyPickupsWhenExploded(bool toggle)
            => _instance.SetCreatesMoneyPickupsWhenExploded(toggle);

        public void SetCustomParachuteModel(uint parachuteModel)
            => _instance.SetCustomParachuteModel(parachuteModel);

        public void SetCustomParachuteTexture(int colorIndex)
            => _instance.SetCustomParachuteTexture(colorIndex);

        public void SetCustomPrimaryColour(int r, int g, int b)
            => _instance.SetCustomPrimaryColour(r, g, b);

        public void SetCustomSecondaryColour(int r, int g, int b)
            => _instance.SetCustomSecondaryColour(r, g, b);

        public void SetDamage(float xOffset, float yOffset, float zOffset, float damage, float radius, bool p6)
            => _instance.SetDamage(xOffset, yOffset, zOffset, damage, radius, p6);

        public void SetDashboardColour(int color)
            => _instance.SetDashboardColour(color);

        public void SetDeformationFixed()
            => _instance.SetDeformationFixed();

        public void SetDirtLevel(float dirtLevel)
            => _instance.SetDirtLevel(dirtLevel);

        public void SetDisablePetrolTankDamage(bool toggle)
            => _instance.SetDisablePetrolTankDamage(toggle);

        public void SetDisablePetrolTankFires(bool toggle)
            => _instance.SetDisablePetrolTankFires(toggle);

        public void SetDoorBroken(int doorIndex, bool deleteDoor)
            => _instance.SetDoorBroken(doorIndex, deleteDoor);

        public void SetDoorCanBreak(int doorIndex, bool isBreakable)
            => _instance.SetDoorCanBreak(doorIndex, isBreakable);

        public void SetDoorControl(int doorIndex, int speed, float angle)
            => _instance.SetDoorControl(doorIndex, speed, angle);

        public void SetDoorLatched(int doorIndex, bool p2, bool p3, bool p4)
            => _instance.SetDoorLatched(doorIndex, p2, p3, p4);

        public void SetDoorOpen(int doorIndex, bool loose, bool openInstantly)
            => _instance.SetDoorOpen(doorIndex, loose, openInstantly);

        public void SetDoorShut(int doorIndex, bool closeInstantly)
            => _instance.SetDoorShut(doorIndex, closeInstantly);

        public void SetDoorsLocked(int doorLockStatus)
            => _instance.SetDoorsLocked(doorLockStatus);

        public void SetDoorsLockedForAllPlayers(bool toggle)
            => _instance.SetDoorsLockedForAllPlayers(toggle);

        public void SetDoorsLockedForPlayer(int player, bool toggle)
            => _instance.SetDoorsLockedForPlayer(player, toggle);

        public void SetDoorsLockedForTeam(int team, bool toggle)
            => _instance.SetDoorsLockedForTeam(team, toggle);

        public void SetDoorsShut(bool closeInstantly)
            => _instance.SetDoorsShut(closeInstantly);

        public void SetEngineCanDegrade(bool toggle)
            => _instance.SetEngineCanDegrade(toggle);

        public void SetEngineHealth(float health)
            => _instance.SetEngineHealth(health);

        public void SetEngineOn(bool value, bool instantly, bool otherwise)
            => _instance.SetEngineOn(value, instantly, otherwise);

        public void SetEnginePowerMultiplier(float value)
            => _instance.SetEnginePowerMultiplier(value);

        public void SetEngineTorqueMultiplier(float value)
            => _instance.SetEngineTorqueMultiplier(value);

        public void SetEnveffScale(float fade)
            => _instance.SetEnveffScale(fade);

        public void SetExclusiveDriver(bool p1)
            => _instance.SetExclusiveDriver(p1);

        public void SetExclusiveDriver2(int ped, int p2)
            => _instance.SetExclusiveDriver2(ped, p2);

        public void SetExplodesOnHighExplosionDamage(bool toggle)
            => _instance.SetExplodesOnHighExplosionDamage(toggle);

        public void SetExtra(int extraId, bool toggle)
            => _instance.SetExtra(extraId, toggle);

        public void SetExtraColours(int pearlescentColor, int wheelColor)
            => _instance.SetExtraColours(pearlescentColor, wheelColor);

        public void SetFixed()
            => _instance.SetFixed();

        public void SetForceHd(bool toggle)
            => _instance.SetForceHd(toggle);

        public void SetForwardSpeed(float speed)
            => _instance.SetForwardSpeed(speed);

        public void SetFrictionOverride(float friction)
            => _instance.SetFrictionOverride(friction);

        public void SetFullbeam(bool toggle)
            => _instance.SetFullbeam(toggle);

        public void SetGravity(bool toggle)
            => _instance.SetGravity(toggle);

        public void SetHalt(float distance, int killEngine, bool unknown)
            => _instance.SetHalt(distance, killEngine, unknown);

        public void SetHandbrake(bool toggle)
            => _instance.SetHandbrake(toggle);

        public void SetHasBeenOwnedByPlayer(bool owned)
            => _instance.SetHasBeenOwnedByPlayer(owned);

        public void SetHasStrongAxles(bool toggle)
            => _instance.SetHasStrongAxles(toggle);

        public void SetHeliBladesFullSpeed()
            => _instance.SetHeliBladesFullSpeed();

        public void SetHeliBladesSpeed(float speed)
            => _instance.SetHeliBladesSpeed(speed);

        public void SetHornEnabled(bool toggle)
            => _instance.SetHornEnabled(toggle);

        public void SetHudSpecialAbilityBarActive(bool active)
            => _instance.SetHudSpecialAbilityBarActive(active);

        public void SetIndicatorLights(int turnSignal, bool toggle)
            => _instance.SetIndicatorLights(turnSignal, toggle);

        public void SetInteriorColour(int color)
            => _instance.SetInteriorColour(color);

        public void SetInteriorlight(bool toggle)
            => _instance.SetInteriorlight(toggle);

        public void SetIsConsideredByPlayer(bool toggle)
            => _instance.SetIsConsideredByPlayer(toggle);

        public void SetIsStolen(bool isStolen)
            => _instance.SetIsStolen(isStolen);

        public void SetIsWanted(bool state)
            => _instance.SetIsWanted(state);

        public void SetJetEngineOn(bool toggle)
            => _instance.SetJetEngineOn(toggle);

        public void SetLastDriven()
            => _instance.SetLastDriven();

        public void SetLightMultiplier(float multiplier)
            => _instance.SetLightMultiplier(multiplier);

        public void SetLights(int state)
            => _instance.SetLights(state);

        public void SetLightsMode(int p1)
            => _instance.SetLightsMode(p1);

        public void SetLivery(int livery)
            => _instance.SetLivery(livery);

        public void SetLodMultiplier(float multiplier)
            => _instance.SetLodMultiplier(multiplier);

        public void SetMod(int modType, int modIndex, bool customTires)
            => _instance.SetMod(modType, modIndex, customTires);

        public void SetModColor1(int paintType, int color, int p3)
            => _instance.SetModColor1(paintType, color, p3);

        public void SetModColor2(int paintType, int color)
            => _instance.SetModColor2(paintType, color);

        public void SetModKit(int modKit)
            => _instance.SetModKit(modKit);

        public void SetNameDebug(string name)
            => _instance.SetNameDebug(name);

        public void SetNeedsToBeHotwired(bool toggle)
            => _instance.SetNeedsToBeHotwired(toggle);

        public void SetNeonLightEnabled(int index, bool toggle)
            => _instance.SetNeonLightEnabled(index, toggle);

        public void SetNeonLightsColour(int r, int g, int b)
            => _instance.SetNeonLightsColour(r, g, b);

        public void SetNetworkNonContact(bool toggle)
            => _instance.SetNetworkNonContact(toggle);

        public void SetNumberPlateText(string plateText)
            => _instance.SetNumberPlateText(plateText);

        public void SetNumberPlateTextIndex(int plateIndex)
            => _instance.SetNumberPlateTextIndex(plateIndex);

        public bool SetOnGroundProperly(int p1)
            => _instance.SetOnGroundProperly(p1);

        public void SetOutOfControl(bool killDriver, bool explodeOnImpact)
            => _instance.SetOutOfControl(killDriver, explodeOnImpact);

        public void SetParachuteActive(bool active)
            => _instance.SetParachuteActive(active);

        public void SetPedTargettableDestroy(int vehicleComponent, int destroyType)
            => _instance.SetPedTargettableDestroy(vehicleComponent, destroyType);

        public void SetPetrolTankHealth(float health)
            => _instance.SetPetrolTankHealth(health);

        public void SetPlaybackSpeed(float speed)
            => _instance.SetPlaybackSpeed(speed);

        public void SetPlaybackToUseAi(int flag)
            => _instance.SetPlaybackToUseAi(flag);

        public void SetPlayersLast()
            => _instance.SetPlayersLast();

        public void SetPopulationBudget()
            => _instance.SetPopulationBudget();

        public void SetProvidesCover(bool toggle)
            => _instance.SetProvidesCover(toggle);

        public void SetRadioEnabled(bool toggle)
            => _instance.SetRadioEnabled(toggle);

        public void SetRadioLoud(bool toggle)
            => _instance.SetRadioLoud(toggle);

        public void SetRampReceivesRampDamage(bool receivesDamage)
            => _instance.SetRampReceivesRampDamage(receivesDamage);

        public void SetReduceGrip(bool toggle)
            => _instance.SetReduceGrip(toggle);

        public void SetRocketBoostActive(bool active)
            => _instance.SetRocketBoostActive(active);

        public void SetRocketBoostPercentage(float percentage)
            => _instance.SetRocketBoostPercentage(percentage);

        public void SetRocketBoostRefillTime(float time)
            => _instance.SetRocketBoostRefillTime(time);

        public void SetRudderBroken(bool p1)
            => _instance.SetRudderBroken(p1);

        public void SetSearchlight(bool toggle, bool canBeUsedByAI)
            => _instance.SetSearchlight(toggle, canBeUsedByAI);

        public void SetShootAtTarget(int entity, float xTarget, float yTarget, float zTarget)
            => _instance.SetShootAtTarget(entity, xTarget, yTarget, zTarget);

        public void SetSilent(bool toggle)
            => _instance.SetSilent(toggle);

        public void SetSiren(bool toggle)
            => _instance.SetSiren(toggle);

        public void SetSirenSound(bool toggle)
            => _instance.SetSirenSound(toggle);

        public void SetSirenWithNoDriver(bool toggle)
            => _instance.SetSirenWithNoDriver(toggle);

        public void SetSteerBias(float value)
            => _instance.SetSteerBias(value);

        public void SetStrong(bool toggle)
            => _instance.SetStrong(toggle);

        public void SetTaxiLights(bool state)
            => _instance.SetTaxiLights(state);

        public void SetTimedExplosion(int ped, bool toggle)
            => _instance.SetTimedExplosion(ped, toggle);

        public void SetTyreBurst(int index, bool onRim, float p3)
            => _instance.SetTyreBurst(index, onRim, p3);

        public void SetTyreFixed(int tyreIndex)
            => _instance.SetTyreFixed(tyreIndex);

        public void SetTyresCanBurst(bool toggle)
            => _instance.SetTyresCanBurst(toggle);

        public void SetTyreSmokeColor(int r, int g, int b)
            => _instance.SetTyreSmokeColor(r, g, b);

        public void SetUndriveable(bool toggle)
            => _instance.SetUndriveable(toggle);

        public void SetVehRadioStation(string radioStation)
            => _instance.SetVehRadioStation(radioStation);

        public void SetWheelsCanBreak(bool enabled)
            => _instance.SetWheelsCanBreak(enabled);

        public void SetWheelsCanBreakOffWhenBlowUp(bool toggle)
            => _instance.SetWheelsCanBreakOffWhenBlowUp(toggle);

        public void SetWheelType(int WheelType)
            => _instance.SetWheelType(WheelType);

        public void SetWindowTint(int tint)
            => _instance.SetWindowTint(tint);

        public void SmashWindow(int index)
            => _instance.SmashWindow(index);

        public void SoundHornThisFrame()
            => _instance.SoundHornThisFrame();

        public void StartAlarm()
            => _instance.StartAlarm();

        public void StartHorn(int duration, uint mode, bool forever)
            => _instance.StartHorn(duration, mode, forever);

        public void StartPlaybackRecorded(int p1, string playback, bool p3)
            => _instance.StartPlaybackRecorded(p1, playback, p3);

        public void StartPlaybackRecordedWithFlags(int p1, string playback, int p3, int p4, int p5)
            => _instance.StartPlaybackRecordedWithFlags(p1, playback, p3, p4, p5);

        public void TaskEveryoneLeave()
            => _instance.TaskEveryoneLeave();

        public void TaskPlayAnim(string animation_set, string animation_name)
            => _instance.TaskPlayAnim(animation_set, animation_name);

        public void ToggleMod(int modType, bool toggle)
            => _instance.ToggleMod(modType, toggle);

        public void TrackVisibility()
            => _instance.TrackVisibility();

        public void UseSirenAsHorn(bool toggle)
            => _instance.UseSirenAsHorn(toggle);

        public int VehToNet()
            => _instance.VehToNet();

        public void WashDecalsFrom(float p1)
            => _instance.WashDecalsFrom(p1);

        #endregion Public Methods
    }
}
