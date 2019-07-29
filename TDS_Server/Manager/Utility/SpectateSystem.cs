using GTANetworkAPI;
using System;
using System.Collections.Generic;
using System.Text;
using TDS_Common.Default;
using TDS_Server.Instance.Player;

namespace TDS_Server.Manager.Utility
{
    class SpectateSystem
    {
        

        public static void SetPlayerToSpectator(TDSPlayer player, bool inSpectator)
        {
            if (inSpectator)
            {
                player.Client.Transparency = 0;
                Workaround.FreezePlayer(player.Client, true);
                Workaround.SetEntityCollisionless(player.Client, true, player.CurrentLobby);
                Workaround.SetPlayerInvincible(player.Client, true);
            }
            else
            {
                player.Client.Transparency = 255;
                Workaround.SetPlayerInvincible(player.Client, false);
                Workaround.SetEntityCollisionless(player.Client, false, player.CurrentLobby);
                NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.StopSpectator);
            }

        }

        public static void SetPlayerToSpectatePlayer(TDSPlayer player, TDSPlayer targetPlayer)
        {
            NAPI.ClientEvent.TriggerClientEvent(player.Client, DToClientEvent.SetPlayerToSpectatePlayer, targetPlayer.Client.Handle.Value);
        }

    }
}
