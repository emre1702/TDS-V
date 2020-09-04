using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Extensions;
using TDS_Shared.Default;

namespace TDS_Server.Handler
{
    public class SpectateHandler
    {
        public void SetPlayerToSpectatePlayer(ITDSPlayer player, ITDSPlayer? targetPlayer)
        {
            if (player.Spectates == targetPlayer)
                return;
            if (player.Spectates is null)
            {
                SetPlayerToSpectator(player, true);
            }
            else
            {
                player.Spectates.Spectators.Remove(player);
            }

            if (targetPlayer is { })
            {
                targetPlayer.Spectators.Add(player);
                player.Position = targetPlayer.Position.AddToZ(10);
                SetPlayerToSpectator(player, true);
                new TDS_Shared.Core.TDSTimer(() =>
                    player.TriggerEvent(ToClientEvent.SetPlayerToSpectatePlayer, targetPlayer.RemoteId), 2000);
            }
            else
            {
                SetPlayerToSpectator(player, false);
            }
        }

        public void SetPlayerToSpectator(ITDSPlayer player, bool inSpectator)
        {
            if (player.Lobby is null)
                return;

            if (inSpectator)
            {
                player.Transparency = 0;
                player.SetCollisionsless(true);
            }
            else
            {
                player.Transparency = 255;
                player.SetCollisionsless(false);

                player.TriggerEvent(ToClientEvent.StopSpectator);
            }
        }
    }
}
