using RAGE;
using RAGE.Elements;
using System;
using TDS_Client.Enum;
using TDS_Client.Manager.Browser;
using TDS_Common.Default;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Damage
{
    static class Damagesys
    {
        private static int _lastArmor = 0;
        private static int _lastHP = 0;
        private static int _lastTotalHealth = 0;

        private static int LastArmor
        {
            get => _lastArmor;
            set
            {
                if (value != _lastArmor)
                {
                    Browser.Angular.Main.SyncHUDDataChange(EHUDDataType.Armor, value);
                    _lastArmor = value;
                }
            }
        }

        private static int LastHP
        {
            get => _lastHP;
            set
            {
                if (value != _lastHP)
                {
                    Browser.Angular.Main.SyncHUDDataChange(EHUDDataType.HP, value);
                    _lastHP = value;
                }
                    
            }
        }

        //public static int CurrentWeaponDamage;

        // Body parts: https://pastebin.com/AGQWgCct

        public static void ShowBloodscreenIfNecessary(int currentTotalHP)
        {
            if (currentTotalHP == _lastTotalHealth)
                return;

            if (currentTotalHP < _lastTotalHealth)
                MainBrowser.ShowBloodscreen();
            _lastTotalHealth = currentTotalHP;
        }

        public static void CheckOnTick()
        {
            int hp = Math.Max(Player.LocalPlayer.GetHealth() - 100, 0);
            int armor = Player.LocalPlayer.GetArmour();
            int currentTotalHealth = hp + armor;
            if (Player.LocalPlayer.HasBeenDamagedByAnyPed())
            {
                int outbone = 0;
                Player.LocalPlayer.GetLastDamageBone(ref outbone);

                //Todo: Use Players.Streamed after update
                foreach (var player in Entities.Players.All)
                {
                    if (!player.Exists)
                        continue;
                    if (player == Player.LocalPlayer)
                        continue;
                    if (!RAGE.Game.Entity.HasEntityBeenDamagedByEntity(Player.LocalPlayer.Handle, player.Handle, true))
                        continue;

                    Events.CallRemote(DToServerEvent.GotHit, player.RemoteId, outbone, _lastTotalHealth - currentTotalHealth);

                }

                Player.LocalPlayer.ClearLastDamageBone();
                Player.LocalPlayer.ClearLastDamageEntity();                    
            }
            ShowBloodscreenIfNecessary(currentTotalHealth);
            LastArmor = armor;
            LastHP = hp;
            _lastTotalHealth = currentTotalHealth;
        }

        public static void ResetLastHP()
        {
            _lastArmor = -1;
            _lastHP = -1;
            LastArmor = Player.LocalPlayer.GetArmour();
            LastHP = Math.Max(Player.LocalPlayer.GetHealth() - 100, 0);
            _lastTotalHealth = LastArmor + LastHP;

            Player.LocalPlayer.ClearLastDamageBone();
            Player.LocalPlayer.ClearLastDamageEntity();
            Player.LocalPlayer.ClearLastWeaponDamage();
            Player.LocalPlayer.ResetVisibleDamage();
            Player.LocalPlayer.ClearBloodDamage();
        }
    }
}
