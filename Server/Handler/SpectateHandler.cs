using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Handler.Entities.Player;
using TDS_Shared.Default;

namespace TDS_Server.Handler
{
    public class SpectateHandler
    {
        private readonly IModAPI _modAPI;

        public SpectateHandler(IModAPI modAPI) 
            => _modAPI = modAPI;

        public void SetPlayerToSpectator(ITDSPlayer player, bool inSpectator)
        {
            if (player.ModPlayer is null)
                return;

            if (inSpectator)
            {
                player.ModPlayer.Transparency = 0;
                player.ModPlayer.Freeze(true);
                player.ModPlayer.SetCollisionless(true, player.Lobby);
            }
            else
            {
                player.ModPlayer.Transparency = 255;
                player.ModPlayer.Freeze(false);
                player.ModPlayer.SetCollisionless(false, player.Lobby);

                _modAPI.Sync.SendEvent(player, ToClientEvent.StopSpectator);
            }

        }

        public void SetPlayerToSpectatePlayer(ITDSPlayer player, ITDSPlayer? targetPlayer)
        {
            if (player.Spectates == targetPlayer)
                return;
            if (player.ModPlayer is null)
                return;

            if (player.Spectates is null)
            {
                SetPlayerToSpectator(player, true);
            }
            else
            {
                player.Spectates.Spectators.Remove(player);
            }

            if (targetPlayer?.ModPlayer is { } modTarget)
            {
                targetPlayer.Spectators.Add(player);
                _modAPI.Sync.SendEvent(player, ToClientEvent.SetPlayerToSpectatePlayer, targetPlayer.RemoteId);
            }
            else 
            {
                SetPlayerToSpectator(player, false);
            }
        }
    }
}
