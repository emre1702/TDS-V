using System;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Event;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Client.Handler.Deathmatch
{
    public class PlayerFightHandler
    {
        public bool InFight
        {
            get => _inFight;
            set
            {
                if (_inFight == value)
                    return;
                _inFight = value;
                _eventsHandler.OnInFightStatusChanged(value);

                if (value)
                {
                    Reset();
                }
            }
        }

        public int CurrentArmor;
        public int CurrentHp;

        private bool _inFight;
        private int _lastBloodscreenUpdateTick;
        private int _lastHudHealthUpdateTick;
        private int _lastHudAmmoUpdateMs;

        private int _lastHudUpdateArmor;
        private int _lastHudUpdateHp;
        private int _lastHudUpdateTotalAmmo;
        private int _lastHudUpdateAmmoInClip;

        private WeaponHash _currentWeapon;

        private readonly IModAPI _modAPI;
        private readonly EventsHandler _eventsHandler;
        private readonly SettingsHandler _settingsHandler;
        private readonly BrowserHandler _browserHandler;
        private readonly FloatingDamageInfoHandler _floatingDamageInfoHandler;
        private readonly UtilsHandler _utilsHandler;
        private readonly CamerasHandler _camerasHandler;

        public PlayerFightHandler(IModAPI modAPI, EventsHandler eventsHandler, SettingsHandler settingsHandler, BrowserHandler browserHandler, FloatingDamageInfoHandler floatingDamageInfoHandler,
            UtilsHandler utilsHandler, CamerasHandler camerasHandler)
        {
            _modAPI = modAPI;
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _browserHandler = browserHandler;
            _floatingDamageInfoHandler = floatingDamageInfoHandler;
            _utilsHandler = utilsHandler;
            _camerasHandler = camerasHandler;

            eventsHandler.WeaponChanged += WeaponChanged;
            eventsHandler.LobbyLeft += _ => SetNotInFight();
            eventsHandler.LocalPlayerDied += SetNotInFight;
            eventsHandler.MapChanged += SetNotInFight;
            eventsHandler.MapCleared += SetNotInFight;
            eventsHandler.RoundStarted += EventsHandler_RoundStarted;
            eventsHandler.RoundEnded += SetNotInFight;

            modAPI.Event.Add(ToClientEvent.HitOpponent, OnHitOpponentMethod);
            modAPI.Event.Add(ToClientEvent.PlayerRespawned, OnPlayerRespawnedMethod);

            modAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(OnTick, () => InFight));
        }

        public void OnTick(int currentMs)
        {
            int previousArmor = CurrentArmor;
            int previousHp = CurrentHp;
            CurrentArmor = _modAPI.LocalPlayer.Armor;
            CurrentHp = Math.Max(_modAPI.LocalPlayer.Health - 100, 0);

            int healthLost = previousArmor + previousHp - CurrentArmor - CurrentHp;
            //Damagesys.CheckDamage(healthLost);

            if (healthLost != 0)
            {
                if (healthLost > 0 && (int)(currentMs - _lastBloodscreenUpdateTick) >= _settingsHandler.PlayerSettings.BloodscreenCooldownMs)
                {
                    //MainBrowser.ShowBloodscreen();
                    _lastBloodscreenUpdateTick = currentMs;
                }

                if ((int)(currentMs - _lastHudHealthUpdateTick) >= _settingsHandler.PlayerSettings.HudHealthUpdateCooldownMs)
                {
                    if (CurrentArmor != _lastHudUpdateArmor)
                    {
                        _browserHandler.Angular.SyncHudDataChange(HudDataType.Armor, CurrentArmor);
                        _lastHudUpdateArmor = CurrentArmor;
                    }
                    if (CurrentHp != _lastHudUpdateHp)
                    {
                        _browserHandler.Angular.SyncHudDataChange(HudDataType.HP, CurrentHp);
                        _lastHudUpdateHp = CurrentHp;
                    }
                    _lastHudHealthUpdateTick = currentMs;
                }
            }


            if ((int)(currentMs - _lastHudAmmoUpdateMs) >= _settingsHandler.PlayerSettings.HudAmmoUpdateCooldownMs)
            {

                int ammoInClip = 0;
                _modAPI.LocalPlayer.GetAmmoInClip(_currentWeapon, ref ammoInClip);
                if (ammoInClip != _lastHudUpdateAmmoInClip)
                {
                    _browserHandler.Angular.SyncHudDataChange(HudDataType.AmmoInClip, ammoInClip);
                    _lastHudUpdateAmmoInClip = ammoInClip;
                }

                int totalAmmo = _modAPI.LocalPlayer.GetAmmoInWeapon(_currentWeapon) - ammoInClip;
                if (totalAmmo != _lastHudUpdateTotalAmmo)
                {
                    _browserHandler.Angular.SyncHudDataChange(HudDataType.AmmoTotal, totalAmmo);
                    _lastHudUpdateTotalAmmo = totalAmmo;
                }

                _lastHudAmmoUpdateMs = currentMs;
            }
        }

        public void HittedOpponent()
        {
            if (_settingsHandler.PlayerSettings.Hitsound)
                _browserHandler.PlainMain.PlayHitsound();
        }

        public void HittedOpponent(IPlayer hitted, int damage)
        {
            if (_settingsHandler.PlayerSettings.FloatingDamageInfo && hitted != null)
                _floatingDamageInfoHandler.Add(hitted, damage);
        }


        public void Reset()
        {
            _lastHudUpdateArmor = -1;
            _lastHudUpdateHp = -1;
            _lastHudUpdateTotalAmmo = -1;
            _lastHudUpdateAmmoInClip = -1;

            _lastBloodscreenUpdateTick = default;
            _lastHudHealthUpdateTick = default;
            _lastHudAmmoUpdateMs = default;

            CurrentArmor = _modAPI.LocalPlayer.Armor;
            CurrentHp = Math.Max(_modAPI.LocalPlayer.Health - 100, 0);
            _browserHandler.Angular.SyncHudDataChange(HudDataType.Armor, CurrentArmor);
            _browserHandler.Angular.SyncHudDataChange(HudDataType.HP, CurrentHp);

            _modAPI.LocalPlayer.ClearLastDamageBone();
            _modAPI.LocalPlayer.ClearLastDamageEntity();
            _modAPI.LocalPlayer.ClearLastWeaponDamage();
            _modAPI.LocalPlayer.ResetVisibleDamage();
            _modAPI.LocalPlayer.ClearBloodDamage();
        }

        private void WeaponChanged(WeaponHash _, WeaponHash newWeaponHash)
        {
            if (_currentWeapon == newWeaponHash)
                return;

            _currentWeapon = newWeaponHash;
        }

        private void SetNotInFight()
        {
            InFight = false;
            Reset();
        }

        private void OnHitOpponentMethod(object[] args)
        {
            ushort targetHandle = Convert.ToUInt16(args[0]);
            int damage = (int)args[1];
            IPlayer target = _utilsHandler.GetPlayerByHandleValue(targetHandle);

            HittedOpponent(target, damage);
        }

        private void OnPlayerRespawnedMethod(object[] args)
        {
            if (!_camerasHandler.Spectating.IsSpectator)
                InFight = true;
            _eventsHandler.OnRespawned(InFight);
        }

        private void EventsHandler_RoundStarted(bool isSpectator)
        {
            InFight = !isSpectator;
        }
    }
}
