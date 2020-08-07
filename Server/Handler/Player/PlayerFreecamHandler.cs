using TDS_Server.Data.Enums;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Player
{
    public class PlayerFreecamHandler
    {
        #region Private Fields

        private readonly DataSyncHandler _dataSyncHandler;
        private readonly ITDSPlayerHandler _tdsPlayerHandler;

        #endregion Private Fields

        #region Public Constructors

        public PlayerFreecamHandler(IModAPI modAPI, DataSyncHandler dataSyncHandler, ITDSPlayerHandler tdsPlayerHandler)
        {
            _dataSyncHandler = dataSyncHandler;
            _tdsPlayerHandler = tdsPlayerHandler;

            modAPI.ClientEvent.Add<IPlayer, bool>(ToServerEvent.SetInFreecam, this, OnSetInFreeCam);
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnSetInFreeCam(IPlayer modPlayer, bool inFreeCam)
        {
            var player = _tdsPlayerHandler.GetIfLoggedIn(modPlayer);
            if (player is null)
                return;
            _dataSyncHandler.SetData(player, PlayerDataKey.InFreeCam, DataSyncMode.Lobby, inFreeCam);
        }

        #endregion Public Methods
    }
}
