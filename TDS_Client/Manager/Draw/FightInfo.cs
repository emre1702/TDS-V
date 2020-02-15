using RAGE.Elements;
using System;
using TDS_Client.Instance.Draw;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Damage;
using TDS_Client.Manager.Event;
using TDS_Client.Manager.Lobby;
using TDS_Client.Manager.Utility;

namespace TDS_Client.Manager.Draw
{
    class FightInfo
    {
        public static int CurrentArmor;
        public static int CurrentHp;

        private static ulong _lastBloodscreenUpdateTick;
        private static ulong _lastHudHealthUpdateTick;
        private static ulong _lastHudAmmoUpdateTick;

        private static int _lastHudUpdateArmor;
        private static int _lastHudUpdateHp;
        private static int _lastHudUpdateTotalAmmo;
        private static int _lastHudUpdateAmmoInClip;

        private static uint _currentWeapon;

        public static void Init()
        {
            CustomEventManager.OnWeaponChange += CustomEventManager_OnWeaponChange;

            TickManager.Add(FightInfo.OnTick, () => Round.InFight);
        }

        public static void OnTick()
        {
            int previousArmor = CurrentArmor;
            int previousHp = CurrentHp;
            CurrentArmor = Player.LocalPlayer.GetArmour();
            CurrentHp = Math.Max(Player.LocalPlayer.GetHealth() - 100, 0);

            int healthLost = previousArmor + previousHp - CurrentArmor - CurrentHp;
            Damagesys.CheckDamage(healthLost);

            var currentTick = TimerManager.ElapsedTicks;

            if (healthLost != 0)
            {
                if (healthLost > 0 && (int)(currentTick - _lastBloodscreenUpdateTick) >= Settings.PlayerSettings.BloodscreenCooldownMs)
                {
                    MainBrowser.ShowBloodscreen();
                    _lastBloodscreenUpdateTick = currentTick;
                }

                if ((int)(currentTick - _lastHudHealthUpdateTick) >= Settings.PlayerSettings.HudHealthUpdateCooldownMs)
                {
                    if (CurrentArmor != _lastHudUpdateArmor)
                    {
                        Browser.Angular.Main.SyncHUDDataChange(Enum.EHUDDataType.Armor, CurrentArmor);
                        _lastHudUpdateArmor = CurrentArmor;
                    }
                    if (CurrentHp != _lastHudUpdateHp)
                    {
                        Browser.Angular.Main.SyncHUDDataChange(Enum.EHUDDataType.Armor, CurrentArmor);
                        _lastHudUpdateArmor = CurrentArmor;
                    }
                    _lastHudHealthUpdateTick = currentTick;
                }
            }


            if ((int)(currentTick - _lastHudAmmoUpdateTick) >= Settings.PlayerSettings.HudAmmoUpdateCooldownMs)
            {
                uint weapon = Player.LocalPlayer.GetSelectedWeapon();

                int ammoInClip = 0;
                Player.LocalPlayer.GetAmmoInClip(weapon, ref ammoInClip);
                if (ammoInClip != _lastHudUpdateAmmoInClip)
                {
                    Browser.Angular.Main.SyncHUDDataChange(Enum.EHUDDataType.AmmoInClip, ammoInClip);
                    _lastHudUpdateAmmoInClip = ammoInClip;
                }

                int totalAmmo = Player.LocalPlayer.GetAmmoInWeapon(weapon) - ammoInClip;
                if (totalAmmo != _lastHudUpdateTotalAmmo)
                {
                    Browser.Angular.Main.SyncHUDDataChange(Enum.EHUDDataType.AmmoTotal, totalAmmo);
                    _lastHudUpdateTotalAmmo = totalAmmo;
                }

                _lastHudAmmoUpdateTick = currentTick;
            }
        }

        public static void HittedOpponent(Player hitted, int damage)
        {
            if (Settings.PlayerSettings.Hitsound)
                MainBrowser.PlayHitsound();

            if (Settings.PlayerSettings.FloatingDamageInfo && hitted != null)
                new FloatingDamageInfo(hitted, damage);
        }


        public static void Reset()
        {
            _lastHudUpdateArmor = default;
            _lastHudUpdateHp = default;
            _lastHudUpdateTotalAmmo = default;
            _lastHudUpdateAmmoInClip = default;

            _lastBloodscreenUpdateTick = default;
            _lastHudHealthUpdateTick = default;
            _lastHudAmmoUpdateTick = default;

            CurrentArmor = default;
            CurrentHp = default;

            Player.LocalPlayer.ClearLastDamageBone();
            Player.LocalPlayer.ClearLastDamageEntity();
            Player.LocalPlayer.ClearLastWeaponDamage();
            Player.LocalPlayer.ResetVisibleDamage();
            Player.LocalPlayer.ClearBloodDamage();
        }

        private static void CustomEventManager_OnWeaponChange(uint _, uint newWeaponHash)
        {
            if (_currentWeapon == newWeaponHash)
                return;

            _currentWeapon = newWeaponHash;
        }
    }
}
