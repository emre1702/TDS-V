using RAGE;
using RAGE.Elements;
using RAGE.Game;
using System.Collections.Generic;
using System.Linq;
using TDS.Client.Data.Defaults;
using TDS.Client.Data.Enums;
using TDS.Client.Data.Interfaces;
using TDS.Client.Handler.Browser;
using TDS.Client.Handler.Draw;
using TDS.Client.Handler.Entities.Draw.Scaleform;
using TDS.Client.Handler.Events;
using static RAGE.Events;
using Player = RAGE.Game.Player;

namespace TDS.Client.Handler.Deathmatch
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
        private readonly Dictionary<WeaponHash, FiringMode> _lastFiringModeByWeapon = new Dictionary<WeaponHash, FiringMode>();

        public FiringModeHandler(BrowserHandler browserHandler, BindsHandler bindsHandler, EventsHandler eventsHandler, SettingsHandler settingsHandler,
            InstructionalButtonHandler instructionalButtonHandler)
        {
            _ignoredWeaponGroups = new HashSet<uint>
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

            _burstFireAllowedWeapons = new HashSet<WeaponHash>
            {
                (WeaponHash)Misc.GetHashKey("WEAPON_APPISTOL"),
            };

            _burstFireAllowedGroups = new HashSet<uint>
            {
                Misc.GetHashKey("GROUP_SMG"),
                Misc.GetHashKey("GROUP_MG"),
                Misc.GetHashKey("GROUP_RIFLE")
            };

            _singleFireBlacklist = new HashSet<WeaponHash>
            {
                (WeaponHash)Misc.GetHashKey("WEAPON_STUNGUN"),
                (WeaponHash)Misc.GetHashKey("WEAPON_FLAREGUN"),
                (WeaponHash)Misc.GetHashKey("WEAPON_MARKSMANPISTOL"),
                (WeaponHash)Misc.GetHashKey("WEAPON_REVOLVER"),
                (WeaponHash)Misc.GetHashKey("WEAPON_REVOLVER_MK2"),
                (WeaponHash)Misc.GetHashKey("WEAPON_DOUBLEACTION"),
                (WeaponHash)Misc.GetHashKey("WEAPON_PUMPSHOTGUN"),
                (WeaponHash)Misc.GetHashKey("WEAPON_PUMPSHOTGUN_MK2"),
                (WeaponHash)Misc.GetHashKey("WEAPON_SAWNOFFSHOTGUN"),
                (WeaponHash)Misc.GetHashKey("WEAPON_BULLPUPSHOTGUN"),
                (WeaponHash)Misc.GetHashKey("WEAPON_MUSKET"),
                (WeaponHash)Misc.GetHashKey("WEAPON_DBSHOTGUN"),
                (WeaponHash)Misc.GetHashKey("WEAPON_SNIPERRIFLE"),
                (WeaponHash)Misc.GetHashKey("WEAPON_HEAVYSNIPER"),
                (WeaponHash)Misc.GetHashKey("WEAPON_HEAVYSNIPER_MK2"),
            };

            _browserHandler = browserHandler;
            _bindsHandler = bindsHandler;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _instructionalButtonHandler = instructionalButtonHandler;

            _eventsHandler.InFightStatusChanged += EventsHandler_InFightStatusChanged;
        }

        private void SetCurrentFiringMode(FiringMode value)
        {
            if (_currentFiringMode != value)
                _browserHandler.Angular.SyncHudDataChange(HudDataType.FiringMode, (int)value);
            _currentFiringMode = value;
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
            return _burstFireAllowedGroups.Contains(Weapon.GetWeapontypeGroup((uint)weaponHash)) || _burstFireAllowedWeapons.Contains(weaponHash);
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
                SetCurrentFiringMode(newFiringMode);
            }
            else
            {
                SetCurrentFiringMode(FiringMode.Auto);
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
            uint group = Weapon.GetWeapontypeGroup((uint)weaponHash);
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
                        Pad.SetControlNormal((int)InputGroup.MOVE, (int)Control.Attack, 1f);
                    else if (_currentBurstShots == 3)
                    {
                        Player.DisablePlayerFiring(false);
                        if (Pad.IsDisabledControlJustReleased((int)InputGroup.MOVE, (int)Control.Attack))
                            _currentBurstShots = 0;
                    }
                    break;

                case FiringMode.Single:
                    if (Pad.IsDisabledControlPressed((int)InputGroup.MOVE, (int)Control.Attack))
                        Player.DisablePlayerFiring(false);
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
                SetCurrentFiringMode(newFiringMode);
                _currentBurstShots = 0;

                Audio.PlaySoundFrontend(-1, AudioName.FASTER_CLICK, AudioRef.RESPAWN_ONLINE_SOUNDSET, true);
                _lastFiringModeByWeapon[_currentWeapon] = _currentFiringMode;
            }
        }
    }
}
