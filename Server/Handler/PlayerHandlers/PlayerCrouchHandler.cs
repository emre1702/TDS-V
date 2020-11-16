using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Enums;
using TDS.Server.Data.Extensions;
using TDS.Server.Data.Interfaces;
using TDS.Server.Handler.Extensions;
using TDS.Server.Handler.Sync;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Server.Handler.PlayerHandlers
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
