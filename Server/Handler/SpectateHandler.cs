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
            if (player.Spectates is { })
            {
                player.Spectates.Spectators.Remove(player);
            }

            if (targetPlayer is { })
            {
                targetPlayer.Spectators.Add(player);
                player.Position = targetPlayer.Position.AddToZ(10);
                new TDS_Shared.Core.TDSTimer(() =>
                    player.TriggerEvent(ToClientEvent.SetPlayerToSpectatePlayer, targetPlayer.RemoteId), 2000);
            }
        }
    }
}
