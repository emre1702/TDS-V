using AltV.Net;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Player
{
    public class PlayerFreecamHandler
    {
        #region Public Constructors

        public PlayerFreecamHandler()
        {
            Alt.OnClient<ITDSPlayer, bool>(ToServerEvent.SetInFreecam, OnSetInFreeCam);
        }

        #endregion Public Constructors

        #region Public Methods

        public void OnSetInFreeCam(ITDSPlayer player, bool inFreeCam)
        {
            player.SetStreamSyncedMetaData(PlayerDataKey.InFreeCam.ToString(), inFreeCam);
        }

        #endregion Public Methods
    }
}
