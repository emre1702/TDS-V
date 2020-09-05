using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.PlayerHandlers
{
    public class PlayerFreecamHandler
    {
        private readonly DataSyncHandler _dataSyncHandler;

        public PlayerFreecamHandler(DataSyncHandler dataSyncHandler)
        {
            _dataSyncHandler = dataSyncHandler;

            NAPI.ClientEvent.Register<ITDSPlayer, bool>(ToServerEvent.SetInFreecam, this, OnSetInFreeCam);
        }

        public void OnSetInFreeCam(ITDSPlayer player, bool inFreeCam)
        {
            if (!player.LoggedIn)
                return;
            _dataSyncHandler.SetData(player, PlayerDataKey.InFreeCam, DataSyncMode.Lobby, inFreeCam);
        }
    }
}
