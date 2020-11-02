using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Data.Enums;
using TDS_Server.Data.Extensions;
using TDS_Server.Data.Interfaces;
using TDS_Server.Handler.Extensions;
using TDS_Server.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Server.Handler.PlayerHandlers
{
    public class PlayerCrouchHandler
    {
        private readonly DataSyncHandler _dataSyncHandler;

        public PlayerCrouchHandler(DataSyncHandler dataSyncHandler)
        {
            _dataSyncHandler = dataSyncHandler;

            NAPI.ClientEvent.Register<ITDSPlayer>(ToServerEvent.ToggleCrouch, this, OnToggleCrouch);
        }

        public void OnToggleCrouch(ITDSPlayer player)
        {
            if (!player.LoggedIn)
                return;
            player.IsCrouched = !player.IsCrouched;
            NAPI.Task.RunSafe(() => 
                _dataSyncHandler.SetData(player, PlayerDataKey.Crouched, DataSyncMode.Lobby, player.IsCrouched));
        }

    }
}
