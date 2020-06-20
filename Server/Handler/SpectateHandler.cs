using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Shared.Default;

namespace TDS_Server.Handler
{
    public class SpectateHandler
    {
        #region Private Fields

        private readonly IModAPI _modAPI;

        #endregion Private Fields

        #region Public Constructors

        public SpectateHandler(IModAPI modAPI)
            => _modAPI = modAPI;

        #endregion Public Constructors

        #region Public Methods

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
                player.ModPlayer.Position = targetPlayer.ModPlayer.Position.AddToZ(10);
                SetPlayerToSpectator(player, true);
                new TDS_Shared.Core.TDSTimer(() =>
                    _modAPI.Sync.SendEvent(player, ToClientEvent.SetPlayerToSpectatePlayer, targetPlayer.RemoteId), 2000);
            }
            else
            {
                SetPlayerToSpectator(player, false);
            }
        }

        public void SetPlayerToSpectator(ITDSPlayer player, bool inSpectator)
        {
            if (player.ModPlayer is null)
                return;
            if (player.Lobby is null)
                return;

            if (inSpectator)
            {
                player.ModPlayer.Transparency = 0;
                player.ModPlayer.SetCollisionsless(true, player.Lobby);
            }
            else
            {
                player.ModPlayer.Transparency = 255;
                player.ModPlayer.SetCollisionsless(false, player.Lobby);

                _modAPI.Sync.SendEvent(player, ToClientEvent.StopSpectator);
            }
        }

        #endregion Public Methods
    }
}
