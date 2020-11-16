using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Extensions;
using TDS.Shared.Default;
using TDS.Server.Handler.Extensions;

namespace TDS.Server.Handler
{
    public class SpectateHandler
    {
        public void SetPlayerToSpectatePlayer(ITDSPlayer player, ITDSPlayer? targetPlayer)
        {
            if (player.Spectates == targetPlayer)
                return;

            if (player.Spectates is { })
                player.Spectates.SpectateHandler.RemoveSpectator(player);

            if (targetPlayer is { })
            {
                targetPlayer.SpectateHandler.AddSpectator(player);
                NAPI.Task.RunSafe(() =>
                {
                    player.Position = targetPlayer.Position.AddToZ(10);
                    _ = new TDS.Shared.Core.TDSTimer(() =>
                        NAPI.Task.Run(() => 
                            player.TriggerEvent(ToClientEvent.SetPlayerToSpectatePlayer, targetPlayer.RemoteId)), 2000);
                });
            }
        }
    }
}
