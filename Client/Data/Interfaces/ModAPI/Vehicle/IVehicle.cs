using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Vehicle
{
    public interface IVehicle : IEntityBase
    {
        #region Public Properties

        IPlayer Controller { get; }

        #endregion Public Properties

        #region Public Methods

        bool AddClanDecalTo(int ped, int boneIndex, float x1, float x2, float x3, float y1, float y2, float y3, float z1, float z2, float z3, float scale, int p13, int alpha);

        void AddUpsidedownCheck();

        bool AnyPassengersRappeling();

        bool AreAllWindowsIntact();

        bool AreAnySeatsFree();

        bool ArePropellersUndamaged();

        bool AreWingsIntact();

        void AttachToCargobob(int cargobob, int p2, float x, float y, float z);

        void AttachToTrailer(int trailer, float radius);

        void BlipSiren();

        int CanParachuteBeActivated();

        bool CanShuffleSeat(int p1);

        void ClearCustomPrimaryColour();

        void ClearCustomSecondaryColour();

        void CloseBombBayDoors();

        void ControlLandingGear(int state);

        int CreatePedInside(int pedType, uint modelHash, int seat, bool isNetwork, bool p5);

        int CreateRandomPedAsDriver(bool returnHandle);

        bool DetachFromAnyCargobob();

        bool DetachFromAnyTowTruck();

        void DetachFromCargobob(int cargobob);

        void DetachFromTrailer();

        void DetachWindscreen();

        void DisablePlaneAileron(bool p1, bool p2);

        bool DoesExtraExist(int extraId);

        bool DoesHaveDecal(int p1);

        int DoesHaveDoor(int doorIndex);

        bool DoesHaveRoof();

        bool DoesHaveStuckVehicleCheck();

        bool DoesHaveWeapons();

        void EjectJb700Roof(float x, float y, float z);

        void Explode(bool isAudible, bool isInvisible);

        void ExplodeInCutscene(bool p1);

        void FixWindow(int index);

        float GetAcceleration();

        bool GetBoatAnchor();

        float GetBodyHealth();

        float GetBodyHealth2(int p1, int p2, int p3, int p4, int p5, int p6);

        uint GetCauseOfDestruction();

        int GetClass();

        void GetColor(ref int r, ref int g, ref int b);

        int GetColourCombination();

        void GetColours(ref int colorPrimary, ref int colorSecondary);

        int GetConvertibleRoofState();

        void GetCustomPrimaryColour(ref int r, ref int g, ref int b);

        void GetCustomSecondaryColour(ref int r, ref int g, ref int b);

        void GetDashboardColour(ref int color);

        uint GetDefaultHorn();

        Position3D GetDeformationAtPos(float offsetX, float offsetY, float offsetZ);

        float GetDirtLevel();

        float GetDoorAngleRatio(int door);

        int GetDoorLockStatus();

        bool GetDoorsLockedForPlayer(int player);

        float GetEngineHealth();

        Position3D GetEntryPositionOfDoor(int doorIndex);

        float GetEnveffScale();

        void GetExtraColours(ref int pearlescentColor, ref int wheelColor);

        int GetHasLowerableWheels();

        float GetHeliEngineHealth();

        float GetHeliMainRotorHealth();

        float GetHeliTailRotorHealth();

        uint GetHornHash();

        void GetInteriorColour(ref int color);

        bool GetIsEngineRunning();

        bool GetIsLeftHeadlightDamaged();

        bool GetIsPrimaryColourCustom();

        bool GetIsRightHeadlightDamaged();

        bool GetIsSecondaryColourCustom();

        int GetLandingGearState();

        int GetLastPedInSeat(int seatIndex);

        uint GetLayoutHash();

        bool GetLightsState(ref int lightsOn, ref int highbeamsOn);

        int GetLivery();

        int GetLiveryCount();

        string GetLiveryName(int liveryIndex);

        float GetMaxBraking();

        int GetMaxNumberOfPassengers();

        float GetMaxTraction();

        int GetMod(int modType);

        void GetModColor1(ref int paintType, ref int color, ref int p3);

        string GetModColor1Name(bool p1);

        void GetModColor2(ref int paintType, ref int color);

        string GetModColor2Name();

        int GetModData(int modType, int modIndex);

        int GetModKit();

        int GetModKitType();

        int GetModModifierValue(int modType, int modIndex);

        string GetModSlotName(int modType);

        string GetModTextLabel(int modType, int modValue);

        bool GetModVariation(int modType);

        void GetNeonLightsColour(ref int r, ref int g, ref int b);

        int GetNumberOfColours();

        int GetNumberOfDoors();

        int GetNumberOfPassengers();

        string GetNumberPlateText();

        int GetNumberPlateTextIndex();

        int GetNumModKits();

        int GetNumMods(int modType);

        bool GetOwner(ref int entity);

        int GetPedInSeat(int index, int p2);

        int GetPedUsingDoor(int doorIndex);

        float GetPetrolTankHealth();

        int GetPlateType();

        float GetSuspensionHeight();

        bool GetTrailerVehicle(ref int trailer);

        bool GetTyresCanBurst();

        void GetTyreSmokeColor(ref int r, ref int g, ref int b);

        int GetWheelType();

        int GetWindowTint();

        int HasJumpingAbility();

        bool HasLandingGear();

        int HasParachute();

        int HasRocketBoost();

        bool IsAConvertible(bool p1);

        bool IsAlarmActivated();

        bool IsAttachedToCargobob(int vehicleAttached);

        bool IsAttachedToTowTruck(int vehicle);

        bool IsAttachedToTrailer();

        bool IsBig();

        bool IsBumperBrokenOff(bool front);

        bool IsDamaged();

        bool IsDoorDamaged(int doorID);

        bool IsDoorFullyOpen(int doorIndex);

        bool IsDriveable(bool isOnFireCheck);

        bool IsExtraTurnedOn(int extraId);

        bool IsHeliPartBroken(bool p1, bool p2, bool p3);

        bool IsHighDetail();

        bool IsHornActive();

        bool IsInBurnout();

        bool IsModel(uint model);

        bool IsModLoadDone();

        bool IsNearEntity(int entity);

        bool IsNeonLightEnabled(int index);

        bool IsNodeIdValid();

        bool IsOnAllWheels();

        bool IsRadioLoud();

        int IsRocketBoostActive();

        bool IsSearchlightOn();

        bool IsSeatFree(VehicleSeat seat);

        bool IsShopResprayAllowed();

        bool IsSirenOn();

        bool IsSirenSoundOn();

        bool IsStolen();

        bool IsStopped();

        bool IsStoppedAtTrafficLights();

        bool IsStuckOnRoof();

        bool IsStuckTimerUp(int p1, int p2);

        bool IsTaxiLightOn();

        bool IsToggleModOn(int modType);

        bool IsTyreBurst(int wheelID, bool completely);

        new bool IsVisible();

        bool IsWindowIntact(int windowIndex);

        void Jitter(bool p1, float yaw, float pitch, float roll);

        void LowerConvertibleRoof(bool instantlyLower);

        int NetworkExplode(bool isAudible, bool isInvisible, bool p3);

        void OpenBombBayDoors();

        void OverrideVehHorn(bool mute, int p2);

        void PlayDoorCloseSound(int p1);

        void PlayDoorOpenSound(int p1);

        void PlayStreamFrom();

        void RaiseConvertibleRoof(bool instantlyRaise);

        void RaiseLowerableWheels();

        void ReleasePreloadMods();

        void RemoveDecalsFrom();

        void RemoveHighDetailModel();

        void RemoveMod(int modType);

        void RemoveStuckCheck();

        void RemoveUpsidedownCheck();

        void RemoveWindow(int windowIndex);

        void RequestHighDetailModel();

        void ResetStuckTimer(int nullAttributes);

        void ResetWheels(bool toggle);

        void RollDownWindow(int windowIndex);

        void RollDownWindows();

        void RollUpWindow(int windowIndex);

        void SetAlarm(bool state);

        void SetAllowNoPassengersLockon(bool toggle);

        void SetAudio(string audioName);

        void SetAudioPriority(int p1);

        int SetAutomaticallyAttaches(int p1, int p2);

        void SetBikeLeanAngle(float x, float y);

        void SetBoatAnchor(bool toggle);

        void SetBodyHealth(float value);

        void SetBombs(int amount);

        int SetBombs();

        void SetBoostActive(bool Toggle);

        void SetBrakeLights(bool toggle);

        void SetBurnout(bool toggle);

        void SetCanBeTargetted(bool state);

        void SetCanBeUsedByFleeingPeds(bool toggle);

        void SetCanBeVisiblyDamaged(bool state);

        void SetCanBreak(bool toggle);

        void SetCanRespray(bool state);

        void SetCeilingHeight(float p1);

        void SetColourCombination(int colorCombination);

        void SetColours(int colorPrimary, int colorSecondary);

        void SetConvertibleRoof(bool p1);

        void SetCountermeasures(int amount);

        void SetCreatesMoneyPickupsWhenExploded(bool toggle);

        void SetCustomParachuteModel(uint parachuteModel);

        void SetCustomParachuteTexture(int colorIndex);

        void SetCustomPrimaryColour(int r, int g, int b);

        void SetCustomSecondaryColour(int r, int g, int b);

        void SetDamage(float xOffset, float yOffset, float zOffset, float damage, float radius, bool p6);

        void SetDashboardColour(int color);

        void SetDeformationFixed();

        void SetDirtLevel(float dirtLevel);

        void SetDisablePetrolTankDamage(bool toggle);

        void SetDisablePetrolTankFires(bool toggle);

        void SetDoorBroken(int doorIndex, bool deleteDoor);

        void SetDoorCanBreak(int doorIndex, bool isBreakable);

        void SetDoorControl(int doorIndex, int speed, float angle);

        void SetDoorLatched(int doorIndex, bool p2, bool p3, bool p4);

        void SetDoorOpen(int doorIndex, bool loose, bool openInstantly);

        void SetDoorShut(int doorIndex, bool closeInstantly);

        void SetDoorsLocked(int doorLockStatus);

        void SetDoorsLockedForAllPlayers(bool toggle);

        void SetDoorsLockedForPlayer(int player, bool toggle);

        void SetDoorsLockedForTeam(int team, bool toggle);

        void SetDoorsShut(bool closeInstantly);

        void SetEngineCanDegrade(bool toggle);

        void SetEngineHealth(float health);

        void SetEngineOn(bool value, bool instantly, bool otherwise);

        void SetEnginePowerMultiplier(float value);

        void SetEngineTorqueMultiplier(float value);

        void SetEnveffScale(float fade);

        void SetExclusiveDriver(bool p1);

        void SetExclusiveDriver2(int ped, int p2);

        void SetExplodesOnHighExplosionDamage(bool toggle);

        void SetExtra(int extraId, bool toggle);

        void SetExtraColours(int pearlescentColor, int wheelColor);

        void SetFixed();

        void SetForceHd(bool toggle);

        void SetForwardSpeed(float speed);

        void SetFrictionOverride(float friction);

        void SetFullbeam(bool toggle);

        void SetGravity(bool toggle);

        void SetHalt(float distance, int killEngine, bool unknown);

        void SetHandbrake(bool toggle);

        void SetHasBeenOwnedByPlayer(bool owned);

        void SetHasStrongAxles(bool toggle);

        void SetHeliBladesFullSpeed();

        void SetHeliBladesSpeed(float speed);

        void SetHornEnabled(bool toggle);

        void SetHudSpecialAbilityBarActive(bool active);

        void SetIndicatorLights(int turnSignal, bool toggle);

        void SetInteriorColour(int color);

        void SetInteriorlight(bool toggle);

        void SetIsConsideredByPlayer(bool toggle);

        void SetIsStolen(bool isStolen);

        void SetIsWanted(bool state);

        void SetJetEngineOn(bool toggle);

        void SetLastDriven();

        void SetLightMultiplier(float multiplier);

        void SetLights(int state);

        void SetLightsMode(int p1);

        void SetLivery(int livery);

        void SetLodMultiplier(float multiplier);

        void SetMod(int modType, int modIndex, bool customTires);

        void SetModColor1(int paintType, int color, int p3);

        void SetModColor2(int paintType, int color);

        void SetModKit(int modKit);

        void SetNameDebug(string name);

        void SetNeedsToBeHotwired(bool toggle);

        void SetNeonLightEnabled(int index, bool toggle);

        void SetNeonLightsColour(int r, int g, int b);

        void SetNetworkNonContact(bool toggle);

        void SetNumberPlateText(string plateText);

        void SetNumberPlateTextIndex(int plateIndex);

        bool SetOnGroundProperly(int p1);

        void SetOutOfControl(bool killDriver, bool explodeOnImpact);

        void SetParachuteActive(bool active);

        void SetPedTargettableDestroy(int vehicleComponent, int destroyType);

        void SetPetrolTankHealth(float health);

        void SetPlaybackSpeed(float speed);

        void SetPlaybackToUseAi(int flag);

        void SetPlayersLast();

        void SetPopulationBudget();

        void SetProvidesCover(bool toggle);

        void SetRadioEnabled(bool toggle);

        void SetRadioLoud(bool toggle);

        void SetRampReceivesRampDamage(bool receivesDamage);

        void SetReduceGrip(bool toggle);

        void SetRocketBoostActive(bool active);

        void SetRocketBoostPercentage(float percentage);

        void SetRocketBoostRefillTime(float time);

        void SetRudderBroken(bool p1);

        void SetSearchlight(bool toggle, bool canBeUsedByAI);

        void SetShootAtTarget(int entity, float xTarget, float yTarget, float zTarget);

        void SetSilent(bool toggle);

        void SetSiren(bool toggle);

        void SetSirenSound(bool toggle);

        void SetSirenWithNoDriver(bool toggle);

        void SetSteerBias(float value);

        void SetStrong(bool toggle);

        void SetTaxiLights(bool state);

        void SetTimedExplosion(int ped, bool toggle);

        void SetTyreBurst(int index, bool onRim, float p3);

        void SetTyreFixed(int tyreIndex);

        void SetTyresCanBurst(bool toggle);

        void SetTyreSmokeColor(int r, int g, int b);

        void SetUndriveable(bool toggle);

        void SetVehRadioStation(string radioStation);

        void SetWheelsCanBreak(bool enabled);

        void SetWheelsCanBreakOffWhenBlowUp(bool toggle);

        void SetWheelType(int WheelType);

        void SetWindowTint(int tint);

        void SmashWindow(int index);

        void SoundHornThisFrame();

        void StartAlarm();

        void StartHorn(int duration, uint mode, bool forever);

        void StartPlaybackRecorded(int p1, string playback, bool p3);

        void StartPlaybackRecordedWithFlags(int p1, string playback, int p3, int p4, int p5);

        void TaskEveryoneLeave();

        void TaskPlayAnim(string animation_set, string animation_name);

        void ToggleMod(int modType, bool toggle);

        void TrackVisibility();

        void UseSirenAsHorn(bool toggle);

        int VehToNet();

        void WashDecalsFrom(float p1);

        #endregion Public Methods
    }
}
