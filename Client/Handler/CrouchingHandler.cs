﻿using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Entity;
using TDS_Client.Data.Interfaces.ModAPI.Player;
using TDS_Client.Handler.Events;
using TDS_Client.Handler.Sync;
using TDS_Shared.Data.Enums;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class CrouchingHandler
    {
        private const string _movementClipSet = "move_ped_crouched";
        private const string _strafeClipSet = "move_ped_crouched_strafing";
        private const float _clipSetSwitchTime = 0.25f;

        private readonly DataSyncHandler _dataSyncHandler;
        private readonly RemoteEventsSender _remoteEventsSender;

        public CrouchingHandler(EventsHandler eventsHandler, DataSyncHandler dataSyncHandler, RemoteEventsSender remoteEventsSender)
        {
            _dataSyncHandler = dataSyncHandler;
            _remoteEventsSender = remoteEventsSender;

            // BindManager.Add(Enum.EKey.LCtrl, ToggleCrouch, Enum.EKeyPressState.Up);
            eventsHandler.DataChanged += PlayerDataSync_OnDataChanged;
        }

        public void OnEntityStreamIn(IEntity entity)
        {
            if (!(entity is IPlayer player))
                return;

            bool isCrouched = _dataSyncHandler.GetData<bool>(player, PlayerDataKey.Crouched);
            if (isCrouched)
            {
                player.SetMovementClipset(_movementClipSet, _clipSetSwitchTime);
                player.SetStrafeClipset(_strafeClipSet);
            }
        }

        private void PlayerDataSync_OnDataChanged(IPlayer player, PlayerDataKey key, object data)
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
