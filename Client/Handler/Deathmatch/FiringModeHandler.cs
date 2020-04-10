using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Entities.Draw.Scaleform;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models.GTA;

namespace TDS_Client.Handler.Deathmatch
{
    public class FiringModeHandler
    {
        // weapons in these groups are completely ignored
        private readonly HashSet<uint> _ignoredWeaponGroups;

        // if a weapon's group is already in burstFireAllowedGroups, don't put it here
        private readonly HashSet<WeaponHash> _burstFireAllowedWeapons;

        private readonly HashSet<uint> _burstFireAllowedGroups;

        // weapons in here are not able to use single fire mode
        private readonly HashSet<WeaponHash> _singleFireBlacklist;

        private WeaponHash _currentWeapon;
        private bool _ignoreCurrentWeapon;
        private FiringMode _currentFiringMode;
        private int _currentBurstShots = 0;
        private Dictionary<WeaponHash, FiringMode> _lastFiringModeByWeapon = new Dictionary<WeaponHash, FiringMode>();
        private InstructionalButton _instructionalButton;
        private bool _isActive;

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

        private readonly EventMethodData<TickDelegate> _tickEventMethod;
        private readonly EventMethodData<WeaponShotDelegate> _weaponShotEventMethod;

        private readonly IModAPI _modAPI;
        private readonly BrowserHandler _browserHandler;
        private readonly BindsHandler _bindsHandler;
        private readonly EventsHandler _eventsHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly InstructionalButtonHandler _instructionalButtonHandler;

        public FiringModeHandler(IModAPI modAPI, BrowserHandler browserHandler, BindsHandler bindsHandler, EventsHandler eventsHandler, SettingsHandler settingsHandler,
            InstructionalButtonHandler instructionalButtonHandler)
        {
            _ignoredWeaponGroups = new HashSet<uint>
            {
                modAPI.Misc.GetHashKey("GROUP_UNARMED"),
                modAPI.Misc.GetHashKey("GROUP_MELEE"),
                modAPI.Misc.GetHashKey("GROUP_FIREEXTINGUISHER"),
                modAPI.Misc.GetHashKey("GROUP_PARACHUTE"),
                modAPI.Misc.GetHashKey("GROUP_STUNGUN"),
                modAPI.Misc.GetHashKey("GROUP_THROWN"),
                modAPI.Misc.GetHashKey("GROUP_PETROLCAN"),
                modAPI.Misc.GetHashKey("GROUP_DIGISCANNER"),
                modAPI.Misc.GetHashKey("GROUP_HEAVY")
            };

            _burstFireAllowedWeapons = new HashSet<WeaponHash>
            {
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_APPISTOL"),
            };

            _burstFireAllowedGroups = new HashSet<uint>
            {
                modAPI.Misc.GetHashKey("GROUP_SMG"),
                modAPI.Misc.GetHashKey("GROUP_MG"),
                modAPI.Misc.GetHashKey("GROUP_RIFLE")
            };

            _singleFireBlacklist = new HashSet<WeaponHash>
            {
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_STUNGUN"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_FLAREGUN"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_MARKSMANPISTOL"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_REVOLVER"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_REVOLVER_MK2"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_DOUBLEACTION"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_PUMPSHOTGUN"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_PUMPSHOTGUN_MK2"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_SAWNOFFSHOTGUN"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_BULLPUPSHOTGUN"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_MUSKET"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_DBSHOTGUN"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_SNIPERRIFLE"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_HEAVYSNIPER"),
                (WeaponHash)modAPI.Misc.GetHashKey("WEAPON_HEAVYSNIPER_MK2"),
            };

            _tickEventMethod = new EventMethodData<TickDelegate>(OnTick);
            _weaponShotEventMethod = new EventMethodData<WeaponShotDelegate>(OnWeaponShot);

            _modAPI = modAPI;
            _browserHandler = browserHandler;
            _bindsHandler = bindsHandler;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _instructionalButtonHandler = instructionalButtonHandler;

            _eventsHandler.InFightStatusChanged += EventsHandler_InFightStatusChanged;
        }

        public void Start()
        {
            if (_isActive)
                return;
            _isActive = true;
            _bindsHandler.Add(Key.F6, ToggleFireMode);
            _modAPI.Event.Tick.Add(_tickEventMethod);
            _modAPI.Event.WeaponShot.Add(_weaponShotEventMethod);
            _eventsHandler.WeaponChanged += CustomEventManager_OnWeaponChange;
            _eventsHandler.LanguageChanged += CustomEventManager_OnLanguageChanged;

            _instructionalButton = _instructionalButtonHandler.Add(_settingsHandler.Language.FIRING_MODE, "F6", true);

            _modAPI.Audio.SetAudioFlag("LoadMPData", true);
        }

        public void Stop()
        {
            if (!_isActive)
                return;
            _isActive = false;
            _bindsHandler.Remove(Key.F6, ToggleFireMode);
            _modAPI.Event.Tick.Remove(_tickEventMethod);
            _modAPI.Event.WeaponShot.Remove(_weaponShotEventMethod);
            _eventsHandler.WeaponChanged -= CustomEventManager_OnWeaponChange;
            if (_instructionalButton != null)
            {
                _instructionalButtonHandler.Remove(_instructionalButton);
                _instructionalButton = null;
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

                _modAPI.Audio.PlaySoundFrontend(-1, "Faster_Click", "RESPAWN_ONLINE_SOUNDSET", true);
                _lastFiringModeByWeapon[_currentWeapon] = _currentFiringMode;
            }
        }

        private void OnTick(ulong currentMs)
        {
            if (_ignoreCurrentWeapon)
                return;

            switch (_currentFiringMode)
            {
                case FiringMode.Auto:
                    break;
                case FiringMode.Burst:
                    if (_currentBurstShots > 0 && _currentBurstShots < 3)
                        _modAPI.Control.SetControlNormal(InputGroup.MOVE, Control.Attack, 1f);
                    else if (_currentBurstShots == 3)
                    {
                        _modAPI.LocalPlayer.DisablePlayerFiring(false);
                        if (_modAPI.Control.IsDisabledControlJustReleased(InputGroup.MOVE, Control.Attack))
                            _currentBurstShots = 0;
                    }
                    break;
                case FiringMode.Single:
                    if (_modAPI.Control.IsDisabledControlPressed(InputGroup.MOVE, Control.Attack))
                        _modAPI.LocalPlayer.DisablePlayerFiring(false);
                    break;
                    /* case FiringMode.Safe:
                         Player.DisablePlayerFiring(false);
                         if (Pad.IsDisabledControlJustPressed((int)EInputGroup.MOVE, (int)Control.Attack))
                             Audio.PlaySoundFrontend(-1, "Faster_Click", "RESPAWN_ONLINE_SOUNDSET", true);
                         break; */
            }
        }

        private void OnWeaponShot(Position3D targetPos, IPlayer target, CancelEventArgs cancel)
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

        private void CustomEventManager_OnLanguageChanged(ILanguage newLang, bool beforeLogin)
        {
            _instructionalButton?.SetTitle(newLang.FIRING_MODE);
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
            uint group = _modAPI.Weapon.GetWeapontypeGroup(weaponHash);
            return _ignoredWeaponGroups.Contains(group);
        }

        private bool CanWeaponUseBurstFire(WeaponHash weaponHash)
        {
            return _burstFireAllowedGroups.Contains(_modAPI.Weapon.GetWeapontypeGroup(weaponHash)) || _burstFireAllowedWeapons.Contains(weaponHash);
        }

        private bool CanWeaponUseSingleFire(WeaponHash weaponHash)
        {
            return !_singleFireBlacklist.Contains(weaponHash);
        }
    }
}
