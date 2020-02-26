using RAGE;
using RAGE.Elements;
using System;
using TDS_Client.Enum;
using TDS_Client.Manager.Browser;
using TDS_Client.Manager.Draw;
using TDS_Common.Default;
using Player = RAGE.Elements.Player;

namespace TDS_Client.Manager.Damage
{
    static class Damagesys
    {
        // Body parts: https://pastebin.com/AGQWgCct

        /*public static void CheckDamage(int healthLost)
        {
            if (!Player.LocalPlayer.HasBeenDamagedByAnyPed())
                return;

            int outbone = 0;
            Player.LocalPlayer.GetLastDamageBone(ref outbone);

            foreach (var player in Entities.Players.All)
            {
                if (!player.Exists)
                    continue;
                if (player == Player.LocalPlayer)
                    continue;
                if (!RAGE.Game.Entity.HasEntityBeenDamagedByEntity(Player.LocalPlayer.Handle, player.Handle, true))
                    continue;

                Events.CallRemote(DToServerEvent.GotHit, player.RemoteId, outbone, healthLost > 0 ? healthLost : 0);
            }

            Player.LocalPlayer.ClearLastDamageBone();
            Player.LocalPlayer.ClearLastDamageEntity();
        }*/
    }
}
