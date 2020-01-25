using RAGE;
using RAGE.Game;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TDS_Client.Enum;
using TDS_Client.Instance.Draw.Dx;
using TDS_Client.Instance.Draw.Scaleform;
using TDS_Client.Manager.Draw;
using TDS_Client.Manager.Event;
using TDS_Client.Manager.Utility;
using TDS_Common.Dto;
using static RAGE.Events;

namespace TDS_Client.Manager.Damage
{
    static class FiringMode
    {
        // weapons in these groups are completely ignored
        private static HashSet<uint> _ignoredWeaponGroups = new HashSet<uint>
        {
            Misc.GetHashKey("GROUP_UNARMED"),
            Misc.GetHashKey("GROUP_MELEE"),
            Misc.GetHashKey("GROUP_FIREEXTINGUISHER"),
            Misc.GetHashKey("GROUP_PARACHUTE"),
            Misc.GetHashKey("GROUP_STUNGUN"),
            Misc.GetHashKey("GROUP_THROWN"),
            Misc.GetHashKey("GROUP_PETROLCAN"),
            Misc.GetHashKey("GROUP_DIGISCANNER"),
            Misc.GetHashKey("GROUP_HEAVY")
        };

        // if a weapon's group is already in burstFireAllowedGroups, don't put it here
        private static HashSet<uint> _burstFireAllowedWeapons = new HashSet<uint>
        {
            Misc.GetHashKey("WEAPON_APPISTOL"),
        };

        private static HashSet<uint> _burstFireAllowedGroups = new HashSet<uint>
        {
            Misc.GetHashKey("GROUP_SMG"),
            Misc.GetHashKey("GROUP_MG"),
            Misc.GetHashKey("GROUP_RIFLE")
        };

        // weapons in here are not able to use single fire mode
        private static HashSet<uint> _singleFireBlacklist = new HashSet<uint>
        {
            Misc.GetHashKey("WEAPON_STUNGUN"),
            Misc.GetHashKey("WEAPON_FLAREGUN"),
            Misc.GetHashKey("WEAPON_MARKSMANPISTOL"),
            Misc.GetHashKey("WEAPON_REVOLVER"),
            Misc.GetHashKey("WEAPON_REVOLVER_MK2"),
            Misc.GetHashKey("WEAPON_DOUBLEACTION"),
            Misc.GetHashKey("WEAPON_PUMPSHOTGUN"),
            Misc.GetHashKey("WEAPON_PUMPSHOTGUN_MK2"),
            Misc.GetHashKey("WEAPON_SAWNOFFSHOTGUN"),
            Misc.GetHashKey("WEAPON_BULLPUPSHOTGUN"),
            Misc.GetHashKey("WEAPON_MUSKET"),
            Misc.GetHashKey("WEAPON_DBSHOTGUN"),
            Misc.GetHashKey("WEAPON_SNIPERRIFLE"),
            Misc.GetHashKey("WEAPON_HEAVYSNIPER"),
            Misc.GetHashKey("WEAPON_HEAVYSNIPER_MK2"),
        };

        private static uint _currentWeapon;
        private static bool _ignoreCurrentWeapon;
        private static EFiringMode _currentFiringMode;
        private static int _currentBurstShots = 0;
        private static DateTime? _lastWeaponConfigUpdate;
        private static Dictionary<uint, EFiringMode> _lastFiringModeByWeapon = new Dictionary<uint, EFiringMode>();
        private static InstructionalButton _instructionalButton;
        private static bool _isActive;

        private static EFiringMode CurrentFiringMode
        {
            get => _currentFiringMode;
            set
            {
                if (_currentFiringMode != value)
                    Browser.Angular.Main.SyncHUDDataChange(EHUDDataType.FiringMode, (int)value);
                _currentFiringMode = value;
                
            }
        }


        public static void Start()
        {
            if (_isActive)
                return;
            _isActive = true;
            BindManager.Add(EKey.F6, ToggleFireMode);
            TickManager.Add(OnTick);
            OnPlayerWeaponShot += OnWeaponShot;
            CustomEventManager.OnWeaponChange += CustomEventManager_OnWeaponChange;
            CustomEventManager.OnLanguageChanged += CustomEventManager_OnLanguageChanged;

            _instructionalButton = InstructionalButtonManager.Add(Settings.Language.FIRING_MODE, "F6", true);

            if (_currentWeapon == 0)
            {
                uint weapon = RAGE.Elements.Player.LocalPlayer.GetSelectedWeapon();
                ClientUtils.Notify("Weapon (in FiringMode): " + weapon);
                CustomEventManager_OnWeaponChange(0, weapon);
            }
        }

        public static void Stop()
        {
            if (!_isActive)
                return;
            _isActive = false;
            BindManager.Remove(EKey.F6, ToggleFireMode);
            TickManager.Remove(OnTick);
            OnPlayerWeaponShot -= OnWeaponShot;
            CustomEventManager.OnWeaponChange -= CustomEventManager_OnWeaponChange;
            _currentWeapon = 0;
            if (_instructionalButton != null)
            {
                InstructionalButtonManager.Remove(_instructionalButton);
                _instructionalButton = null;
            }
        }

        private static void ToggleFireMode(EKey _)
        {
            if (_ignoreCurrentWeapon)
                return;

            EFiringMode newFiringMode = (int)_currentFiringMode + 1 > System.Enum.GetValues(typeof(EFiringMode)).Cast<int>().Max() ? 0 : _currentFiringMode + 1;

            switch (newFiringMode)
            {
                case EFiringMode.Burst:
                    if (!CanWeaponUseBurstFire(_currentWeapon))
                        newFiringMode = CanWeaponUseSingleFire(_currentWeapon) ? EFiringMode.Single : EFiringMode.Auto;
                    break;
                case EFiringMode.Single:
                    if (!CanWeaponUseSingleFire(_currentWeapon))
                        newFiringMode = EFiringMode.Auto;
                    break;
            }

            if (newFiringMode != _currentFiringMode)
            {
                CurrentFiringMode = newFiringMode;
                _currentBurstShots = 0;
                _lastWeaponConfigUpdate = DateTime.Now;

                Audio.PlaySoundFrontend(-1, "Faster_Click", "RESPAWN_ONLINE_SOUNDSET", true);
                _lastFiringModeByWeapon[_currentWeapon] = _currentFiringMode;
            }
        }

        private static void OnTick()
        {
            switch (_currentFiringMode)
            {
                case EFiringMode.Auto:
                    break;
                case EFiringMode.Burst:
                    if (_currentBurstShots < 3)
                        Pad.SetControlNormal((int)EInputGroup.MOVE, (int)Control.Attack, 1f);
                    else if (_currentBurstShots == 3)
                    {
                        Player.DisablePlayerFiring(false);
                        if (Pad.IsDisabledControlJustPressed((int)EInputGroup.MOVE, (int)Control.Attack))
                            _currentBurstShots = 0;
                    }
                    break;
                case EFiringMode.Single:
                    if (Pad.IsDisabledControlJustPressed((int)EInputGroup.MOVE, (int)Control.Attack))
                        Player.DisablePlayerFiring(false);
                    break;
               /* case EFiringMode.Safe:
                    Player.DisablePlayerFiring(false);
                    if (Pad.IsDisabledControlJustPressed((int)EInputGroup.MOVE, (int)Control.Attack))
                        Audio.PlaySoundFrontend(-1, "Faster_Click", "RESPAWN_ONLINE_SOUNDSET", true);
                    break; */
            }
        }

        private static void OnWeaponShot(Vector3 targetPos, RAGE.Elements.Player target, CancelEventArgs cancel)
        {
            switch (_currentFiringMode)
            {
                case EFiringMode.Auto:
                case EFiringMode.Single:
                    break;
                case EFiringMode.Burst:
                    ++_currentBurstShots;
                    break;
            }
        }

        private static void CustomEventManager_OnWeaponChange(uint _, uint newWeaponHash)
        {
            _currentWeapon = newWeaponHash;
            _ignoreCurrentWeapon = IsWeaponIgnored(newWeaponHash);
            if (!_lastFiringModeByWeapon.TryGetValue(newWeaponHash, out _currentFiringMode))
            {
                CurrentFiringMode = EFiringMode.Auto;
            }
            _currentBurstShots = 0;
        }

        private static void CustomEventManager_OnLanguageChanged(Interface.ILanguage newLang)
        {
            _instructionalButton?.SetTitle(newLang.FIRING_MODE);
        }


        private static bool IsWeaponIgnored(uint weaponHash)
        {
            return _ignoredWeaponGroups.Contains(weaponHash);
        }

        private static bool CanWeaponUseBurstFire(uint weaponHash)
        {
            return _burstFireAllowedGroups.Contains(Weapon.GetWeapontypeGroup(weaponHash)) || _burstFireAllowedWeapons.Contains(weaponHash);
        }

        private static bool CanWeaponUseSingleFire(uint weaponHash)
        {
            return !_singleFireBlacklist.Contains(weaponHash);
        }
    }
}
