using RAGE;
using RAGE.Elements;
using System;
using TDS_Client.Manager.Browser;
using TDS_Common.Default;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Damage
{
    static class Damagesys
    {
        private static int _lastTotalHP = 0;

        public static int CurrentWeaponDamage;

        // Body parts: https://pastebin.com/AGQWgCct

        public static void ShowBloodscreenIfNecessary()
        {
            int currentTotalHP = Player.LocalPlayer.GetHealth() + Player.LocalPlayer.GetArmour();
            if (currentTotalHP == _lastTotalHP)
                return;

            if (currentTotalHP < _lastTotalHP)
                MainBrowser.ShowBloodscreen();
            _lastTotalHP = currentTotalHP;
        }

        public static void CheckOnTick()
        {
            int currentTotalHP = Math.Max(Player.LocalPlayer.GetHealth() - 100, 0) + Player.LocalPlayer.GetArmour();
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

                    Events.CallRemote(DToServerEvent.GotHit, player.RemoteId, outbone, _lastTotalHP - currentTotalHP);

                }

                Player.LocalPlayer.ClearLastDamageBone();
                Player.LocalPlayer.ClearLastDamageEntity();                    
            }
            _lastTotalHP = currentTotalHP;
        }

        public static void ResetLastHP()
        {
            _lastTotalHP = Player.LocalPlayer.GetHealth() + Player.LocalPlayer.GetArmour();
        }
    }
}