using RAGE.Elements;
using TDS.Client.Data.Abstracts.Entities.GTA;
using TDS.Client.Data.Enums;
using TDS.Client.Handler.Events;
using TDS.Client.Handler.Sync;
using TDS.Shared.Data.Enums;
using TDS.Shared.Default;

namespace TDS.Client.Handler
{
    public class CrouchingHandler : ServiceBase
    {
        private const float _clipSetSwitchTime = 0.25f;
        private const string _movementClipSet = "move_ped_crouched";
        private const string _strafeClipSet = "move_ped_crouched_strafing";
        private readonly DataSyncHandler _dataSyncHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        public CrouchingHandler(LoggingHandler loggingHandler, EventsHandler eventsHandler, DataSyncHandler dataSyncHandler, RemoteEventsSender remoteEventsSender)
            : base(loggingHandler)
        {
            _dataSyncHandler = dataSyncHandler;
            _remoteEventsSender = remoteEventsSender;

            // Do that on loggedin BindManager.Add(Enum.EKey.LCtrl, ToggleCrouch, Enum.EKeyPressState.Up);
            eventsHandler.DataChanged += PlayerDataSync_OnDataChanged;

            // RAGE.Game.Event.EntityStreamIn.Add(new EventMethodData<EntityStreamInDelegate>(OnEntityStreamIn));
        }

        public void OnEntityStreamIn(Entity entity)
        {
            if (!(entity is ITDSPlayer player))
                return;

            bool isCrouched = _dataSyncHandler.GetData<bool>(player, PlayerDataKey.Crouched);
            if (isCrouched)
            {
                player.SetMovementClipset(_movementClipSet, _clipSetSwitchTime);
                player.SetStrafeClipset(_strafeClipSet);
            }
        }

        private void PlayerDataSync_OnDataChanged(ITDSPlayer player, PlayerDataKey key, object data)
        {
            if (key != PlayerDataKey.Crouched)
                return;
            if ((bool)data)
            {
                player.SetMovementClipset(_movementClipSet, _clipSetSwitchTime);
                player.SetStrafeClipset(_strafeClipSet);
            }
            else
            {
                player.ResetMovementClipset(_clipSetSwitchTime);
                player.ResetStrafeClipset();
            }
        }

        private void ToggleCrouch(Key _)
        {
            _remoteEventsSender.Send(ToServerEvent.ToggleCrouch);
        }
    }
}
