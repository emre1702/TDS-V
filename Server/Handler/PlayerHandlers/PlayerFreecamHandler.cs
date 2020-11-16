using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Extensions;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Sync;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.Handler.PlayerHandlers
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
            NAPI.Task.RunSafe(() => 
                _dataSyncHandler.SetData(player, PlayerDataKey.InFreeCam, DataSyncMode.Lobby, inFreeCam));
        }
    }
}
