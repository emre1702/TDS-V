using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Shared.Core;
using TDS_Shared.Default;

namespace TDS_Server.Handler
{
    public class SpectateHandler
    {
        #region Public Constructors

        public SpectateHandler() { }

        #endregion Public Constructors

        #region Public Methods

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
                new TDSTimer(() =>
                    player.SendEvent(ToClientEvent.SetPlayerToSpectatePlayer, targetPlayer), 2000);
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
                player.SetCollisionsless(true, player.Lobby);
            }
            else
            {
                player.Transparency = 255;
                player.SetCollisionsless(false, player.Lobby);

                player.SendEvent(ToClientEvent.StopSpectator);
            }
        }

        #endregion Public Methods
    }
}
