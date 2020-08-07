using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Player
{
    public class PlayerCrouchHandler
    {
        #region Private Fields

        private readonly DataSyncHandler _dataSyncHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public PlayerCrouchHandler(IModAPI modAPI, DataSyncHandler dataSyncHandler, ITDSPlayerHandler tdsPlayerHandler)
        {
            _dataSyncHandler = dataSyncHandler;
            _tdsPlayerHandler = tdsPlayerHandler;

            modAPI.ClientEvent.Add(ToServerEvent.ToggleCrouch, this, OnToggleCrouch);
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnToggleCrouch(IPlayer modPlayer)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            player.IsCrouched = !player.IsCrouched;
            _dataSyncHandler.SetData(player, PlayerDataKey.Crouched, DataSyncMode.Lobby, player.IsCrouched);
        }

        #endregion Public Methods
    }
}
