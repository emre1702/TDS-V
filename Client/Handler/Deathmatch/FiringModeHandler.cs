﻿using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System.Collections.Generic;
using System.Linq;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Entities.Draw.Scaleform;
using TDS_Client.Handler.Events;
using static RAGE.Events;

namespace TDS_Client.Handler.Deathmatch
{
    public class FiringModeHandler
    {
        private readonly BindsHandler _bindsHandler;

        private readonly BrowserHandler _browserHandler;

        private readonly HashSet<uint> _burstFireAllowedGroups;

        // if a weapon's group is already in burstFireAllowedGroups, don't put it here
        private readonly HashSet<WeaponHash> _burstFireAllowedWeapons;

        private readonly EventsHandler _eventsHandler;

        // weapons in these groups are completely ignored
        private readonly HashSet<uint> _ignoredWeaponGroups;

        private readonly InstructionalButtonHandler _instructionalButtonHandler;

        private readonly SettingsHandler _settingsHandler;

        // weapons in here are not able to use single fire mode
        private readonly HashSet<WeaponHash> _singleFireBlacklist;

        private int _currentBurstShots = 0;
        private FiringMode _currentFiringMode;
        private WeaponHash _currentWeapon;
        private bool _ignoreCurrentWeapon;
        private InstructionalButton _instructionalButton;
        private bool _isActive;
        private Dictionary<WeaponHash, FiringMode> _lastFiringModeByWeapon = new Dictionary<WeaponHash, FiringMode>();

        public FiringModeHandler(BrowserHandler browserHandler, BindsHandler bindsHandler, EventsHandler eventsHandler, SettingsHandler settingsHandler,
            InstructionalButtonHandler instructionalButtonHandler)
        {
            _ignoredWeaponGroups = new HashSet<uint>
            {
                RAGE.Game.Misc.GetHashKey("GROUP_UNARMED"),
                RAGE.Game.Misc.GetHashKey("GROUP_MELEE"),
                RAGE.Game.Misc.GetHashKey("GROUP_FIREEXTINGUISHER"),
                RAGE.Game.Misc.GetHashKey("GROUP_PARACHUTE"),
                RAGE.Game.Misc.GetHashKey("GROUP_STUNGUN"),
                RAGE.Game.Misc.GetHashKey("GROUP_THROWN"),
                RAGE.Game.Misc.GetHashKey("GROUP_PETROLCAN"),
                RAGE.Game.Misc.GetHashKey("GROUP_DIGISCANNER"),
                RAGE.Game.Misc.GetHashKey("GROUP_HEAVY")
            };

            _burstFireAllowedWeapons = new HashSet<WeaponHash>
            {
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_APPISTOL"),
            };

            _burstFireAllowedGroups = new HashSet<uint>
            {
                RAGE.Game.Misc.GetHashKey("GROUP_SMG"),
                RAGE.Game.Misc.GetHashKey("GROUP_MG"),
                RAGE.Game.Misc.GetHashKey("GROUP_RIFLE")
            };

            _singleFireBlacklist = new HashSet<WeaponHash>
            {
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_STUNGUN"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_FLAREGUN"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_MARKSMANPISTOL"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_REVOLVER"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_REVOLVER_MK2"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_DOUBLEACTION"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_PUMPSHOTGUN"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_PUMPSHOTGUN_MK2"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_SAWNOFFSHOTGUN"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_BULLPUPSHOTGUN"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_MUSKET"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_DBSHOTGUN"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_SNIPERRIFLE"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_HEAVYSNIPER"),
                (WeaponHash)RAGE.Game.Misc.GetHashKey("WEAPON_HEAVYSNIPER_MK2"),
            };

            _browserHandler = browserHandler;
            _bindsHandler = bindsHandler;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _instructionalButtonHandler = instructionalButtonHandler;

            _eventsHandler.InFightStatusChanged += EventsHandler_InFightStatusChanged;
        }

        private FiringMode CurrentFiringMode
        {
            get => _currentFiringMode;
            set
            {
                if (_currentFiringMode != value)
                    _browserHandler.Angular.SyncHudDataChange(HudDataType.FiringMode, (int)value);
                _currentFiringMode = value;
            }
        }

        public void Start()
        {
            if (_isActive)
                return;
            _isActive = true;
            _bindsHandler.Add(Key.F6, ToggleFireMode);
            Tick += OnTick;
            OnPlayerWeaponShot += OnWeaponShot;
            _eventsHandler.WeaponChanged += CustomEventManager_OnWeaponChange;
            _eventsHandler.LanguageChanged += CustomEventManager_OnLanguageChanged;

            _instructionalButton = _instructionalButtonHandler.Add(_settingsHandler.Language.FIRING_MODE, "F6", true);
        }

        public void Stop()
        {
            if (!_isActive)
                return;
            _isActive = false;
            _bindsHandler.Remove(Key.F6, ToggleFireMode);
            Tick -= OnTick;
            OnPlayerWeaponShot -= OnWeaponShot;
            _eventsHandler.WeaponChanged -= CustomEventManager_OnWeaponChange;
            if (_instructionalButton != null)
            {
                _instructionalButtonHandler.Remove(_instructionalButton);
                _instructionalButton = null;
            }
        }

        private bool CanWeaponUseBurstFire(WeaponHash weaponHash)
        {
            return _burstFireAllowedGroups.Contains(RAGE.Game.Weapon.GetWeapontypeGroup((uint)weaponHash)) || _burstFireAllowedWeapons.Contains(weaponHash);
        }

        private bool CanWeaponUseSingleFire(WeaponHash weaponHash)
        {
            return !_singleFireBlacklist.Contains(weaponHash);
        }

        private void CustomEventManager_OnLanguageChanged(ILanguage newLang, bool beforeLogin)
        {
            _instructionalButton?.SetTitle(newLang.FIRING_MODE);
        }

        private void CustomEventManager_OnWeaponChange(WeaponHash _, WeaponHash newWeaponHash)
        {
            _currentWeapon = newWeaponHash;
            _ignoreCurrentWeapon = IsWeaponIgnored(newWeaponHash);
            if (_lastFiringModeByWeapon.TryGetValue(newWeaponHash, out FiringMode newFiringMode))
            {
                CurrentFiringMode = newFiringMode;
            }
            else
            {
                CurrentFiringMode = FiringMode.Auto;
            }
            _currentBurstShots = 0;
        }

        private void EventsHandler_InFightStatusChanged(bool boolean)
        {
            if (boolean)
                Start();
            else
                Stop();
        }

        private bool IsWeaponIgnored(WeaponHash weaponHash)
        {
            uint group = RAGE.Game.Weapon.GetWeapontypeGroup((uint)weaponHash);
            return _ignoredWeaponGroups.Contains(group);
        }

        private void OnTick(List<TickNametagData> _)
        {
            if (_ignoreCurrentWeapon)
                return;

            switch (_currentFiringMode)
            {
                case FiringMode.Auto:
                    break;

                case FiringMode.Burst:
                    if (_currentBurstShots > 0 && _currentBurstShots < 3)
                        RAGE.Game.Pad.SetControlNormal((int)InputGroup.MOVE, (int)Control.Attack, 1f);
                    else if (_currentBurstShots == 3)
                    {
                        RAGE.Game.Player.DisablePlayerFiring(false);
                        if (RAGE.Game.Pad.IsDisabledControlJustReleased((int)InputGroup.MOVE, (int)Control.Attack))
                            _currentBurstShots = 0;
                    }
                    break;

                case FiringMode.Single:
                    if (RAGE.Game.Pad.IsDisabledControlPressed((int)InputGroup.MOVE, (int)Control.Attack))
                        RAGE.Game.Player.DisablePlayerFiring(false);
                    break;
                    /* case FiringMode.Safe:
                         Player.DisablePlayerFiring(false);
                         if (Pad.IsDisabledControlJustPressed((int)EInputGroup.MOVE, (int)Control.Attack))
                             Audio.PlaySoundFrontend(-1, "Faster_Click", "RESPAWN_ONLINE_SOUNDSET", true);
                         break; */
            }
        }

        private void OnWeaponShot(Vector3 targetPos, RAGE.Elements.Player target, CancelEventArgs cancel)
        {
            switch (_currentFiringMode)
            {
                case FiringMode.Auto:
                case FiringMode.Single:
                    break;

                case FiringMode.Burst:
                    ++_currentBurstShots;
                    break;
            }
        }

        private void ToggleFireMode(Key _)
        {
            if (_ignoreCurrentWeapon)
                return;

            FiringMode newFiringMode = (int)_currentFiringMode + 1 > System.Enum.GetValues(typeof(FiringMode)).Cast<int>().Max() ? 0 : _currentFiringMode + 1;

            switch (newFiringMode)
            {
                case FiringMode.Burst:
                    if (!CanWeaponUseBurstFire(_currentWeapon))
                        newFiringMode = CanWeaponUseSingleFire(_currentWeapon) ? FiringMode.Single : FiringMode.Auto;
                    break;

                case FiringMode.Single:
                    if (!CanWeaponUseSingleFire(_currentWeapon))
                        newFiringMode = FiringMode.Auto;
                    break;
            }

            if (newFiringMode != _currentFiringMode)
            {
                CurrentFiringMode = newFiringMode;
                _currentBurstShots = 0;

                RAGE.Game.Audio.PlaySoundFrontend(-1, AudioName.FASTER_CLICK, AudioRef.RESPAWN_ONLINE_SOUNDSET, true);
                _lastFiringModeByWeapon[_currentWeapon] = _currentFiringMode;
            }
        }
    }
}
