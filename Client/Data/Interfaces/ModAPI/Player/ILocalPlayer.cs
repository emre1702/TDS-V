using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Data.Interfaces.ModAPI.Player
{
    public interface ILocalPlayer : IPlayer
    {
        void Call(string eventName, params object[] args);
        bool AreFlashingStarsAboutToDrop();
        bool AreStarsGreyedOut();
        void AssistedMovementCloseRoute();
        void AssistedMovementFlushRoute();
        bool CanStartMission();
        //
        // Summary:
        //     b2 and/or b3 maybe got something to do with keeping values from the last ped.
        //     Both of them set to 1 works great. Examples from the decompiled scripts: PLAYER::CHANGE_PLAYER_PED(PLAYER::PLAYER_ID(),
        //     l_5C04/14/, 0, 1); PLAYER::CHANGE_PLAYER_PED(PLAYER::PLAYER_ID(), a_0a_0._f7/1/,
        //     a_2, 0); =========================================================== The only
        //     way I ever got this to work in GTA Online once is by setting both to 0, 0. However,
        //     when you switch from your online character to whomever, your character will start
        //     walking away as if you left the game. If from there you attempt to call this
        //     native once more to switch back to you online ped. You will freeze or if you
        //     try changing to another ped. Ive tried all posibilities so far. 1, 1 (Freeze),
        //     0, 0(Works Once), 1, 0 AND0, 1 (Freeze). Note of course trying to call this on
        //     another online player will crash. Anyone have any idea if implementing a blr
        //     within the xex itself on a possible check if it would prevent this freezing?
        //     ===========================================================
        void ChangePed(int ped, bool b2, bool b3);
        void ClearHasDamagedAtLeastOneNonAnimalPed();
        void ClearHasDamagedAtLeastOnePed();
        void ClearParachuteModelOverride();
        void ClearParachutePackModelOverride();
        void ClearParachuteVariationOverride();
        //
        // Summary:
        //     This executes at the same as speed as PLAYER::SET_PLAYER_WANTED_LEVEL(player,
        //     0, false); PLAYER::GET_PLAYER_WANTED_LEVEL(player); executes in less than half
        //     the time. Which means that its worth first checking if the wanted level needs
        //     to be cleared before clearing. However, this is mostly about good code practice
        //     and can important in other situations. The difference in time in this example
        //     is negligible.
        void ClearWantedLevel();
        //
        // Summary:
        //     Inhibits the player from using any method of combat including melee and firearms.
        //     NOTE: Only disables the firing for one frame
        void DisableFiring(bool toggle);
        void DisableVehicleRewards();
        //
        // Summary:
        //     Purpose of the BOOL currently unknown. Both, true and false, work
        void DisplaySystemSigninUi(bool unk);
        void EnableSpecialAbility(bool toggle);
        //
        // Summary:
        //     Appears only 3 times in the scripts, more specifically in michael1.ysc Console
        //     hash: 0x64ddb07d - This can be used to prevent dying if you are out of the world
        void ExpandWorldLimits(float x, float y, float z);
        //
        // Summary:
        //     used with 1,2,8,64,128 in the scripts
        void ForceCleanup(int cleanupFlags);
        //
        // Summary:
        //     PLAYER::FORCE_CLEANUP_FOR_ALL_THREADS_WITH_THIS_NAME(pb_prostitute, 1); // Found
        //     in decompilation
        void ForceCleanupForAllThreadsWithThisName(string name, int cleanupFlags);
        void ForceCleanupForThreadWithThisId(int id, int cleanupFlags);
        int GetCauseOfMostRecentForceCleanup();
        //
        // Summary:
        //     Returns TRUE if it found an entity in your crosshair within range of your weapon.
        //     Assigns the handle of the target to the entity that you pass it. Returns false
        //     if no entity found.
        bool GetIsFreeAimingAt(ref int entity);
        //
        // Summary:
        //     Gets the maximum wanted level the player can get. Ranges from 0 to 5.
        int GetMaxWantedLevel();
        float GetCurrentStealthNoise();
        //
        // Summary:
        //     Returns the group ID the player is member of.
        int GetGroup();
        bool GetHasReserveParachute();
        //
        // Summary:
        //     Returns the same as PLAYER_ID and NETWORK_PLAYER_ID_TO_INT
        int GetIndex();
        //
        // Summary:
        //     Returns the Players Invincible status. This function will always return false
        //     if 0x733A643B5B0C53C1 is used to set the invincibility status. To always get
        //     the correct result, use this: bool IsPlayerInvincible(Player player) { auto addr
        //     = getScriptHandleBaseAddress(GET_PLAYER_PED(player)); if (addr) { DWORD flag
        //     = (DWORD )(addr + 0x188); return ((flag AND(1 8)) != 0) OR ((flag AND(1 9)) !=
        //     0); } return false; }
        bool GetInvincible();
        int GetMaxArmour();
        string GetName();
        void GetParachutePackTintIndex(ref int tintIndex);
        void GetParachuteSmokeTrailColor(ref int r, ref int g, ref int b);
        //
        // Summary:
        //     Tints: None = -1, Rainbow = 0, Red = 1, SeasideStripes = 2, WidowMaker = 3, Patriot
        //     = 4, Blue = 5, Black = 6, Hornet = 7, AirFocce = 8, Desert = 9, Shadow = 10,
        //     HighAltitude = 11, Airbone = 12, Sunrise = 13,
        new void GetParachuteTintIndex(ref int tintIndex);
        //
        // Summary:
        //     Gets the ped id of a player - Please update and release your native hash translation
        //     table quicker, AB Team. Thanks, :).
        int GetPed();
        //
        // Summary:
        //     Does the same like PLAYER::GET_PLAYER_PED why does it return an entity and not
        //     a ped?
        int GetPedScriptIndex();
        //
        // Summary:
        //     Tints: None = -1, Rainbow = 0, Red = 1, SeasideStripes = 2, WidowMaker = 3, Patriot
        //     = 4, Blue = 5, Black = 6, Hornet = 7, AirFocce = 8, Desert = 9, Shadow = 10,
        //     HighAltitude = 11, Airbone = 12, Sunrise = 13,
        void GetReserveParachuteTintIndex(ref int index);
        //
        // Summary:
        //     Returns RGB color of the player (duh)
        void GetRgbColour(ref int r, ref int g, ref int b);
        //
        // Summary:
        //     Alternative: GET_VEHICLE_PED_IS_IN(PLAYER_PED_ID(), 1);
        int GetLastVehicle();
        float GetSprintStaminaRemaining();
        float GetSprintTimeRemaining();
        //
        // Summary:
        //     Assigns the handle of locked-on melee target to entity that you pass it. Returns
        //     false if no entity found.
        bool GetTargetEntity(ref int entity);
        //
        // Summary:
        //     Gets the players team. Does nothing in singleplayer.
        int GetTeam();
        float GetUnderwaterTimeRemaining();
        Position3D GetWantedCentrePosition();
        int GetWantedLevel();
        //
        // Summary:
        //     Returns the time since the character was arrested in (ms) milliseconds. example
        //     var time = Function.callint(Hash.GET_TIME_SINCE_LAST_ARREST(); UI.DrawSubtitle(time.ToString());
        //     if player has not been arrested, the int returned will be -1.
        int GetTimeSinceLastArrest();
        //
        // Summary:
        //     Returns the time since the character died in (ms) milliseconds. example var time
        //     = Function.callint(Hash.GET_TIME_SINCE_LAST_DEATH(); UI.DrawSubtitle(time.ToString());
        //     if player has not died, the int returned will be -1.
        int GetTimeSinceLastDeath();
        int GetTimeSinceDroveAgainstTraffic();
        int GetTimeSinceDroveOnPavement();
        int GetTimeSinceHitPed();
        int GetTimeSinceHitVehicle();
        //
        // Summary:
        //     Remnant from GTA IV. Does nothing in GTA V.
        float GetWantedLevelRadius();
        //
        // Summary:
        //     Drft
        int GetWantedLevelThreshold(int wantedLevel);
        //
        // Summary:
        //     Achievements from 0-57 more achievements came with update 1.29 (freemode events
        //     update), Id say that they now go to 60, but Ill need to check.
        bool GiveAchievement(int achievement);
        void GiveRagdollControl(bool toggle);
        bool HasAchievementBeenPassed(int achievement);
        bool HasForceCleanupOccurred(int cleanupFlags);
        bool HasBeenSpottedInStolenVehicle();
        bool HasDamagedAtLeastOneNonAnimalPed();
        bool HasDamagedAtLeastOnePed();
        //
        // Summary:
        //     Gets the players info and calls a function that checks the players ped position.
        //     Heres the decompiled function that checks the position: pastebin.com/ZdHG2E7n
        bool HasLeftTheWorld();
        bool HasTeleportFinished();
        //
        // Summary:
        //     Simply returns whatever is passed to it (Regardless of whether the handle is
        //     valid or not). -------------------------------------------------------- if (NETWORK::NETWORK_IS_PARTICIPANT_ACTIVE(PLAYER::INT_TO_PARTICIPANTINDEX(i)))
        int IntToParticipantindex(int value);
        //
        // Summary:
        //     Simply returns whatever is passed to it (Regardless of whether the handle is
        //     valid or not).
        int IntToPlayerindex(int value);
        //
        // Summary:
        //     Return true while player is being arrested / busted. If atArresting is set to
        //     1, this function will return 1 when player is being arrested (while player is
        //     putting his hand up, but still have control) If atArresting is set to 0, this
        //     function will return 1 only when the busted screen is shown.
        bool IsBeingArrested(bool atArresting);
        //
        // Summary:
        //     Returns true when the player is not able to control the cam i.e. when running
        //     a benchmark test, switching the player or viewing a cutscene. Note: I am not
        //     100% sure if the native actually checks if the cam control is disabled but it
        //     seems promising.
        bool IsCamControlDisabled();
        //
        // Summary:
        //     Returns TRUE if the player (s ped) is climbing at the moment.
        new bool IsClimbing();
        //
        // Summary:
        //     Can the player control himself, used to disable controls for player for things
        //     like a cutscene. --- You cant disable controls with this, use SET_PLAYER_CONTROL(...)
        //     for this.
        bool IsControlOn();
        bool IsDead();
        //
        // Summary:
        //     Gets a value indicating whether the specified player is currently aiming freely.
        bool IsFreeAiming();
        //
        // Summary:
        //     Gets a value indicating whether the specified player is currently aiming freely
        //     at the specified entity.
        bool IsFreeAimingAtEntity(int entity);
        bool IsFreeForAmbientTask();
        //
        // Summary:
        //     this function is hard-coded to always return 0.
        bool IsLoggingInNp();
        //
        // Summary:
        //     Checks whether the specified player has a Ped, the Ped is not dead, is not injured
        //     and is not arrested.
        bool IsPlaying();
        bool IsPressingHorn();
        bool IsReadyForCutscene();
        //
        // Summary:
        //     Returns true if the player is riding a train.
        bool IsRidingTrain();
        bool IsScriptControlOn();
        bool IsTargettingAnything();
        bool IsTargettingEntity(int entity);
        bool IsTeleportActive();
        bool IsWantedLevelGreater(int wantedLevel);
        bool IsSpecialAbilityActive();
        bool IsSpecialAbilityEnabled();
        bool IsSpecialAbilityMeterFull();
        bool IsSpecialAbilityUnlocked(uint playerModel);
        bool IsSystemUiBeingDisplayed();
        //
        // Summary:
        //     Does exactly the same thing as PLAYER_ID()
        int NetworkPlayerIdToInt();
        //
        // Summary:
        //     Only 1 match. ob_sofa_michael. PLAYER::PLAYER_ATTACH_VIRTUAL_BOUND(-804.5928f,
        //     173.1801f, 71.68436f, 0f, 0f, 0.590625f, 1f, 0.7f);1.0.335.2, 1.0.350.1/2, 1.0.372.2,
        //     1.0.393.2, 1.0.393.4, 1.0.463.1;
        void AttachVirtualBound(float p0, float p1, float p2, float p3, float p4, float p5, float p6, float p7);
        //
        // Summary:
        //     1.0.335.2, 1.0.350.1/2, 1.0.372.2, 1.0.393.2, 1.0.393.4, 1.0.463.1;
        void DetachVirtualBound();
        //
        // Summary:
        //     This returns YOUR identity as a Player type. Always returns 0 in story mode.
        int PlayerId();
        //
        // Summary:
        //     Returns current player ped
        int PlayerPedId();
        new void RemoveHelmet(bool p2);
        //
        // Summary:
        //     PLAYER::REPORT_CRIME(PLAYER::PLAYER_ID(), 37, PLAYER::GET_WANTED_LEVEL_THRESHOLD(1));
        //     From am_armybase.ysc.c4: PLAYER::REPORT_CRIME(PLAYER::PLAYER_ID(4), 36, PLAYER::GET_WANTED_LEVEL_THRESHOLD(4));
        //     ----- This was taken from the GTAV.exe v1.334. The function is called sub_140592CE8.
        //     For a full decompilation of the function, see here: pastebin.com/09qSMsN7 -----
        //     crimeType: 1: Firearms possession 2: Person running a red light (5-0-5) 3: Reckless
        //     driver 4: Speeding vehicle (a 5-10) 5: Traffic violation (a 5-0-5) 6: Motorcycle
        //     rider without a helmet 7: Vehicle theft (a 5-0-3) 8: Grand Theft Auto 9: ???
        //     10: ??? 11: Assault on a civilian (a 2-40) 12: Assault on an officer 13: Assault
        //     with a deadly weapon (a 2-45) 14: Officer shot (a 2-45) 15: Pedestrian struck
        //     by a vehicle 16: Officer struck by a vehicle 17: Helicopter down (an AC?) 18:
        //     Civilian on fire (a 2-40) 19: Officer set on fire (a 10-99) 20: Car on fire 21:
        //     Air unit down (an AC?) 22: An explosion (a 9-96) 23: A stabbing (a 2-45) (also
        //     something else I couldnt understand) 24: Officer stabbed (also something else
        //     I couldnt understand) 25: Attack on a vehicle (MDV?) 26: Damage to property 27:
        //     Suspect threatening officer with a firearm 28: Shots fired 29: ??? 30: ??? 31:
        //     ??? 32: ??? 33: ??? 34: A 2-45 35: ??? 36: A 9-25 37: ??? 38: ??? 39: ??? 40:
        //     ??? 41: ??? 42: ??? 43: Possible disturbance 44: Civilian in need of assistance
        //     45: ??? 46: ???
        void ReportCrime(int crimeType, int wantedLvlThresh);
        void ResetArrestState();
        void ResetInputGait();
        void ResetStamina();
        void ResetWantedLevelDifficulty();
        void RestoreStamina(float p1);
        //
        // Summary:
        //     This can be between 1.0f - 14.9f You can change the max in IDA from 15.0. I say
        //     15.0 as the function blrs if what you input is greater than or equal to 15.0
        //     hence why its 14.9 max default.
        void SetAirDragMultiplierForVehicle(float multiplier);
        void SetAllRandomPedsFlee(bool toggle);
        void SetAllRandomPedsFleeThisFrame();
        void SetAutoGiveParachuteWhenEnterPlane(bool toggle);
        void SetDisableAmbientMeleeMove(bool toggle);
        void SetDispatchCops(bool toggle);
        void SetEveryoneIgnoreMe(bool toggle);
        void SetIgnoreLowPriorityShockingEvents(bool toggle);
        void SetMaxWantedLevel(int maxWantedLevel);
        //
        // Summary:
        //     Sets whether this player can be hassled by gangs.
        void SetCanBeHassledByGangs(bool toggle);
        //
        // Summary:
        //     Set whether this player should be able to do drive-bys. A drive-by is when a
        //     ped is aiming/shooting from vehicle. This includes middle finger taunts. By setting
        //     this value to false I confirm the player is unable to do all that. Tested on
        //     tick.
        void SetCanDoDriveBy(bool toggle);
        void SetCanLeaveParachuteSmokeTrail(bool enabled);
        //
        // Summary:
        //     Sets whether this player can take cover.
        void SetCanUseCover(bool toggle);
        //
        // Summary:
        //     6 matches across 4 scripts. 5 occurrences were 240. The other was 255.
        void SetClothLockCounter(int value);
        //
        // Summary:
        //     Every occurrence was either 0 or 2.
        void SetClothPackageIndex(int index);
        //
        // Summary:
        //     Every occurrence of p1 I found was true.1.0.335.2, 1.0.350.1/2, 1.0.372.2, 1.0.393.2,
        //     1.0.393.4, 1.0.463.1;
        void SetClothPinFrames(bool toggle);
        //
        // Summary:
        //     Flags used in the scripts: 0,4,16,24,32,56,60,64,128,134,256,260,384,512,640,768,896,900,952,1024,1280,2048,2560
        //     Note to people who needs this with camera mods, etc.: Flags(0, 4, 16, 24, 32,
        //     56, 60, 64, 128, 134, 512, 640, 1024, 2048, 2560) - Disables camera rotation
        //     as well. Flags(256, 260, 384, 768, 896, 900, 952, 1280) - Allows camera rotation.
        void SetControl(bool toggle, int possiblyFlags);
        void SetForcedAim(bool toggle);
        void SetForcedZoom(bool toggle);
        void SetForceSkipAimIntro(bool toggle);
        void SetHasReserveParachute();
        void SetHealthRechargeMultiplier(float regenRate);
        //
        // Summary:
        //     Simply sets you as invincible (Health will not deplete). Use 0x733A643B5B0C53C1
        //     instead if you want Ragdoll enabled, which is equal to: (DWORD )(playerPedAddress
        //     + 0x188) OR (1 9);
        new void SetInvincible(bool toggle);
        //
        // Summary:
        //     Example from fm_mission_controler.ysc.c4: PLAYER::SET_PLAYER_LOCKON(PLAYER::PLAYER_ID(),
        //     1); All other decompiled scripts using this seem to be using the player id as
        //     the first parameter, so I feel the need to confirm it as so. No need to confirm
        //     it says PLAYER_ID() so it uses PLAYER_ID() lol.
        void SetLockon(bool toggle);
        //
        // Summary:
        //     Affects the range of auto aim target.
        void SetLockonRangeOverride(float range);
        //
        // Summary:
        //     Default is 100. Use player id and not ped id. For instance: PLAYER::SET_PLAYER_MAX_ARMOUR(PLAYER::PLAYER_ID(),
        //     100); // main_persistent.ct4
        void SetMaxArmour(int value);
        void SetMayNotEnterAnyVehicle();
        void SetMayOnlyEnterThisVehicle(int vehicle);
        void SetMeleeWeaponDamageModifier(float modifier, int p2);
        void SetMeleeWeaponDefenseModifier(float modifier);
        //
        // Summary:
        //     Make sure to request the model first and wait until it has loaded.
        void SetModel(uint model);
        void SetNoiseMultiplier(float multiplier);
        //
        // Summary:
        //     example: PLAYER::SET_PLAYER_PARACHUTE_MODEL_OVERRIDE(PLAYER::PLAYER_ID(), 0x73268708);
        void SetParachuteModelOverride(uint model);
        void SetParachutePackModelOverride(uint model);
        //
        // Summary:
        //     tints 0- 13 0 - unkown 1 - unkown 2 - unkown 3 - unkown 4 - unkown
        void SetParachutePackTintIndex(int tintIndex);
        void SetParachuteSmokeTrailColor(int r, int g, int b);
        //
        // Summary:
        //     Tints: None = -1, Rainbow = 0, Red = 1, SeasideStripes = 2, WidowMaker = 3, Patriot
        //     = 4, Blue = 5, Black = 6, Hornet = 7, AirFocce = 8, Desert = 9, Shadow = 10,
        //     HighAltitude = 11, Airbone = 12, Sunrise = 13,
        new void SetParachuteTintIndex(int tintIndex);
        //
        // Summary:
        //     p1 was always 5. p4 was always false.
        void SetParachuteVariationOverride(int p1, int p2, int p3, bool p4);
        //
        // Summary:
        //     Tints: None = -1, Rainbow = 0, Red = 1, SeasideStripes = 2, WidowMaker = 3, Patriot
        //     = 4, Blue = 5, Black = 6, Hornet = 7, AirFocce = 8, Desert = 9, Shadow = 10,
        //     HighAltitude = 11, Airbone = 12, Sunrise = 13,
        new void SetReserveParachuteTintIndex(int index);
        //
        // Summary:
        //     example: flags: 0-6 PLAYER::SET_PLAYER_RESET_FLAG_PREFER_REAR_SEATS(PLAYER::PLAYER_ID(),
        //     6); wouldnt the flag be the seatIndex?
        void SetResetFlagPreferRearSeats(int flags);
        void SetSimulateAiming(bool toggle);
        //
        // Summary:
        //     Values around 1.0f to 2.0f used in game scripts.
        void SetSneakingNoiseMultiplier(float multiplier);
        void SetSprint(bool toggle);
        void SetStealthPerceptionModifier(float value);
        //
        // Summary:
        //     Sets your targeting mode. 0 = Traditional GTA 1 = Assisted Aiming 2 = Free Aim
        //     Even tho gtaforums nor Alexander B supports this, if youre online in freemode
        //     already its nice to have this since retail or otherwise you have to go to SP
        //     to change it.
        void SetTargetingMode(int targetMode);
        //
        // Summary:
        //     Set player team on deathmatch and last team standing..
        void SetTeam(int team);
        void SetVehicleDamageModifier(float damageAmount);
        void SetVehicleDefenseModifier(float modifier);
        //
        // Summary:
        //     # Predominant call signatures PLAYER::SET_PLAYER_WANTED_CENTRE_POSITION(PLAYER::PLAYER_ID(),
        //     ENTITY::GET_ENTITY_COORDS(PLAYER::PLAYER_PED_ID(), 1)); # Parameter value ranges
        //     P0: PLAYER::PLAYER_ID() P1: ENTITY::GET_ENTITY_COORDS(PLAYER::PLAYER_PED_ID(),
        //     1) P2: Not set by any call
        void SetWantedCentrePosition(Position3D position, bool p2, bool p3);
        //
        // Summary:
        //     Call SET_PLAYER_WANTED_LEVEL_NOW for immediate effect wantedLevel is an integer
        //     value representing 0 to 5 stars even though the game supports the 6th wanted
        //     level but no police will appear since no definitions are present for it in the
        //     game files disableNoMission- Disables When Off Mission- appears to always be
        //     false
        void SetWantedLevel(int wantedLevel, bool disableNoMission);
        //
        // Summary:
        //     p2 is always false in R scripts
        void SetWantedLevelNoDrop(int wantedLevel, bool p2);
        //
        // Summary:
        //     Forces any pending wanted level to be applied to the specified player immediately.
        //     Call SET_PLAYER_WANTED_LEVEL with the desired wanted level, followed by SET_PLAYER_WANTED_LEVEL_NOW.
        //     Second parameter is unknown (always false).
        void SetWantedLevelNow(bool p1);
        //
        // Summary:
        //     This modifies the damage value of your weapon. Whether it is a multiplier or
        //     base damage is unknown. Based on tests, it is unlikely to be a multiplier.
        void SetWeaponDamageModifier(float damageAmount);
        void SetWeaponDefenseModifier(float modifier);
        //
        // Summary:
        //     The player will be ignored by the police if toggle is set to true
        void SetPoliceIgnore(bool toggle);
        //
        // Summary:
        //     If toggle is set to false: The police wont be shown on the (mini)map If toggle
        //     is set to true: The police will be shown on the (mini)map
        void SetPoliceRadarBlips(bool toggle);
        //
        // Summary:
        //     Multiplier goes up to 1.49 any value above will be completely overruled by the
        //     game and the multiplier will not take effect, this can be edited in memory however.
        //     Just call it one time, it is not required to be called once every tick. Note:
        //     At least the IDA method if you change the max float multiplier from 1.5 it will
        //     change it for both this and SWIM above. I say 1.5 as the function blrs if what
        //     you input is greater than or equal to 1.5 hence why its 1.49 max default.
        void SetRunSprintMultiplier(float multiplier);
        void SetSpecialAbilityMultiplier(float multiplier);
        //
        // Summary:
        //     Swim speed multiplier. Multiplier goes up to 1.49 Just call it one time, it is
        //     not required to be called once every tick. - Note copied from below native. Note:
        //     At least the IDA method if you change the max float multiplier from 1.5 it will
        //     change it for both this and RUN_SPRINT below. I say 1.5 as the function blrs
        //     if what you input is greater than or equal to 1.5 hence why its 1.49 max default.
        void SetSwimMultiplier(float multiplier);
        //
        // Summary:
        //     Max value is 1.0
        void SetWantedLevelDifficulty(float difficulty);
        void SetWantedLevelMultiplier(float multiplier);
        //
        // Summary:
        //     This is to make the player walk without accepting input from INPUT. gaitType
        //     is in increments of 100s. 2000, 500, 300, 200, etc. p4 is always 1 and p5 is
        //     always 0. C# Example : Function.Call(Hash.SIMULATE_PLAYER_INPUT_GAIT, Game.Player,
        //     1.0f, 100, 1.0f, 1, 0); //Player will go forward for 100ms
        void SimulateInputGait(float amount, int gaitType, float speed, bool p4, bool p5);
        //
        // Summary:
        //     p1 appears as 5, 10, 15, 25, or 30. p2 is always true.
        void SpecialAbilityChargeAbsolute(int p1, bool p2);
        //
        // Summary:
        //     p1 appears to always be 1 (only comes up twice)
        void SpecialAbilityChargeContinuous(int p2);
        //
        // Summary:
        //     2 matches. p1 was always true.
        void SpecialAbilityChargeLarge(bool p1, bool p2);
        //
        // Summary:
        //     Only 1 match. Both p1 ANDp2 were true.
        void SpecialAbilityChargeMedium(bool p1, bool p2);
        //
        // Summary:
        //     normalizedValue is from 0.0 - 1.0 p2 is always 1
        void SpecialAbilityChargeNormalized(float normalizedValue, bool p2);
        //
        // Summary:
        //     Every occurrence of p1 ANDp2 were both true.
        void SpecialAbilityChargeSmall(bool p1, bool p2);
        void SpecialAbilityDeactivate();
        void SpecialAbilityDeactivateFast();
        //
        // Summary:
        //     p1 was always true.
        void SpecialAbilityDepleteMeter(bool p1);
        //
        // Summary:
        //     Also known as _RECHARGE_SPECIAL_ABILITY
        void SpecialAbilityFillMeter(bool p1);
        void SpecialAbilityLock(uint playerModel);
        void SpecialAbilityReset();
        void SpecialAbilityUnlock(uint playerModel);
        void StartFiringAmnesty(int duration);
        void StartTeleport(float x, float y, float z, float heading, bool p5, bool p6, bool p7);
        //
        // Summary:
        //     Disables the players teleportation
        void StopTeleport();
        //
        // Summary:
        //     This was previously named as RESERVE_ENTITY_EXPLODES_ON_HIGH_EXPLOSION_COMBO
        //     which is obviously incorrect. Seems to only appear in scripts used in Singleplayer.
        //     p1 ranges from 2 - 46. I assume this switches the crime type
        void SwitchCrimeType(int p1);
 
    }
}
