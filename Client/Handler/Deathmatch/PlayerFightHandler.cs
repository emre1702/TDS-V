using System;
using System.Collections.Generic;
using TDS_Client.Data.Abstracts.Entities.GTA;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Models;
using TDS_Client.Handler.Browser;
using TDS_Client.Handler.Draw;
using TDS_Client.Handler.Events;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;
using static RAGE.Events;

namespace TDS_Client.Handler.Deathmatch
{
    public class PlayerFightHandler : ServiceBase
    {
        public int CurrentArmor;

        public int CurrentHp;

        private readonly BrowserHandler _browserHandler;

        private readonly CamerasHandler _camerasHandler;

        private readonly EventsHandler _eventsHandler;

        private readonly FloatingDamageInfoHandler _floatingDamageInfoHandler;

        private readonly SettingsHandler _settingsHandler;

        private readonly UtilsHandler _utilsHandler;

        private readonly TimerHandler _timerHandler;

        private WeaponHash _currentWeapon;

        private bool _inFight;

        private int _lastBloodscreenUpdateTick;

        private int _lastHudAmmoUpdateMs;

        private int _lastHudHealthUpdateTick;

        private int _lastHudUpdateAmmoInClip;

        private int _lastHudUpdateArmor;

        private int _lastHudUpdateHp;

        private int _lastHudUpdateTotalAmmo;

        public PlayerFightHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler, SettingsHandler settingsHandler, BrowserHandler browserHandler,
            FloatingDamageInfoHandler floatingDamageInfoHandler,
            UtilsHandler utilsHandler, CamerasHandler camerasHandler, TimerHandler timerHandler)
            : base(loggingHandler)
        {
            _eventsHandler = eventsHandler;
            _settingsHandler = settingsHandler;
            _browserHandler = browserHandler;
            _floatingDamageInfoHandler = floatingDamageInfoHandler;
            _utilsHandler = utilsHandler;
            _camerasHandler = camerasHandler;
            _timerHandler = timerHandler;

            eventsHandler.WeaponChanged += WeaponChanged;
            eventsHandler.LobbyLeft += _ => SetNotInFight();
            eventsHandler.LocalPlayerDied += SetNotInFight;
            eventsHandler.MapChanged += SetNotInFight;
            eventsHandler.MapCleared += SetNotInFight;
            eventsHandler.RoundStarted += EventsHandler_RoundStarted;
            eventsHandler.RoundEnded += _ => SetNotInFight();

            RAGE.Events.Add(ToClientEvent.HitOpponent, OnHitOpponentMethod);
            RAGE.Events.Add(ToClientEvent.PlayerRespawned, OnPlayerRespawnedMethod);
        }

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
                    Tick += OnTick;
                }
                else
                {
                    Tick -= OnTick;
                }
            }
        }

        public void HittedOpponent()
        {
            if (_settingsHandler.PlayerSettings.Hitsound)
                _browserHandler.PlainMain.PlayHitsound();
        }

        public void HittedOpponent(ITDSPlayer hitted, int damage)
        {
            if (_settingsHandler.PlayerSettings.FloatingDamageInfo && hitted != null)
                _floatingDamageInfoHandler.Add(hitted, damage);
        }

        public void OnTick(List<TickNametagData> _)
        {
            int previousArmor = CurrentArmor;
            int previousHp = CurrentHp;
            CurrentArmor = RAGE.Elements.Player.LocalPlayer.GetArmour();
            CurrentHp = Math.Max(RAGE.Elements.Player.LocalPlayer.GetHealth() - 100, 0);

            int healthLost = previousArmor + previousHp - CurrentArmor - CurrentHp;
            //Damagesys.CheckDamage(healthLost);

            if (healthLost != 0)
            {
                if (healthLost > 0 && (int)(_timerHandler.ElapsedMs - _lastBloodscreenUpdateTick) >= _settingsHandler.PlayerSettings.BloodscreenCooldownMs)
                {
                    //MainBrowser.ShowBloodscreen();
                    _lastBloodscreenUpdateTick = _timerHandler.ElapsedMs;
                }

                if ((int)(_timerHandler.ElapsedMs - _lastHudHealthUpdateTick) >= _settingsHandler.PlayerSettings.HudHealthUpdateCooldownMs)
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
                    _lastHudHealthUpdateTick = _timerHandler.ElapsedMs;
                }
            }

            if ((int)(_timerHandler.ElapsedMs - _lastHudAmmoUpdateMs) >= _settingsHandler.PlayerSettings.HudAmmoUpdateCooldownMs)
            {
                int ammoInClip = 0;
                RAGE.Elements.Player.LocalPlayer.GetAmmoInClip((uint)_currentWeapon, ref ammoInClip);
                if (ammoInClip != _lastHudUpdateAmmoInClip)
                {
                    _browserHandler.Angular.SyncHudDataChange(HudDataType.AmmoInClip, ammoInClip);
                    _lastHudUpdateAmmoInClip = ammoInClip;
                }

                int totalAmmo = RAGE.Elements.Player.LocalPlayer.GetAmmoInWeapon((uint)_currentWeapon) - ammoInClip;
                if (totalAmmo != _lastHudUpdateTotalAmmo)
                {
                    _browserHandler.Angular.SyncHudDataChange(HudDataType.AmmoTotal, totalAmmo);
                    _lastHudUpdateTotalAmmo = totalAmmo;
                }

                _lastHudAmmoUpdateMs = _timerHandler.ElapsedMs;
            }
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

            CurrentArmor = RAGE.Elements.Player.LocalPlayer.GetArmour();
            CurrentHp = Math.Max(RAGE.Elements.Player.LocalPlayer.GetHealth() - 100, 0);
            _browserHandler.Angular.SyncHudDataChange(HudDataType.Armor, CurrentArmor);
            _browserHandler.Angular.SyncHudDataChange(HudDataType.HP, CurrentHp);

            RAGE.Elements.Player.LocalPlayer.ClearLastDamageBone();
            RAGE.Elements.Player.LocalPlayer.ClearLastDamageEntity();
            RAGE.Elements.Player.LocalPlayer.ClearLastWeaponDamage();
            RAGE.Elements.Player.LocalPlayer.ResetVisibleDamage();
            RAGE.Elements.Player.LocalPlayer.ClearBloodDamage();
        }

        private void EventsHandler_RoundStarted(bool isSpectator)
        {
            InFight = !isSpectator;
        }

        private void OnHitOpponentMethod(object[] args)
        {
            ushort targetHandle = Convert.ToUInt16(args[0]);
            int damage = (int)args[1];
            var target = _utilsHandler.GetPlayerByHandleValue(targetHandle);

            HittedOpponent(target, damage);
        }

        private void OnPlayerRespawnedMethod(object[] args)
        {
            if (!_camerasHandler.Spectating.IsSpectator)
                InFight = true;
            _eventsHandler.OnRespawned(InFight);
        }

        private void SetNotInFight()
        {
            InFight = false;
            Reset();
        }

        private void WeaponChanged(WeaponHash _, WeaponHash newWeaponHash)
        {
            if (_currentWeapon == newWeaponHash)
                return;

            _currentWeapon = newWeaponHash;
        }
    }
}
