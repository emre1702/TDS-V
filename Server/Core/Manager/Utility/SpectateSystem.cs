using GTANetworkAPI;
using TDS_Common.Default;
using TDS_Server.Instance.PlayerInstance;

namespace TDS_Server.Core.Manager.Utility
{
    class SpectateSystem
    {
        public static void SetPlayerToSpectator(TDSPlayer player, bool inSpectator)
        {
            if (inSpectator)
            {
                player.Player!.Transparency = 0;
                Workaround.FreezePlayer(player.Player, true);
                Workaround.SetEntityCollisionless(player.Player, true, player.CurrentLobby);
                Workaround.SetPlayerInvincible(player.Player, true);
            }
            else
            {
                player.Player!.Transparency = 255;
                Workaround.SetPlayerInvincible(player.Player, false);
                Workaround.SetEntityCollisionless(player.Player, false, player.CurrentLobby);
                NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.StopSpectator);
            }

        }

        public static void SetPlayerToSpectatePlayer(TDSPlayer player, TDSPlayer? targetPlayer)
        {
            if (player.Spectates == targetPlayer)
                return;
            if (player.Player is null)
                return;

            if (player.Spectates is null)
            {
                SetPlayerToSpectator(player, true);
            } 
            else
            {
                player.Spectates.Spectators.Remove(player);
            }

            if (targetPlayer is null || targetPlayer.Player is null)
            {
                SetPlayerToSpectator(player, false);
            }
            else
            {
                targetPlayer.Spectators.Add(player);
                NAPI.ClientEvent.TriggerClientEvent(player.Player, DToClientEvent.SetPlayerToSpectatePlayer, targetPlayer.Player.Handle.Value);
            }
        }
    }
}
