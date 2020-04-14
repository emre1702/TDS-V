﻿using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.IncomingConnection)]
        public void OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            var cancelEventArgs = new TDS_Shared.Data.Models.CancelEventArgs();
            Init.TDSCore.EventsHandler.OnIncomingConnection(ip, serial, socialClubName, socialClubId, cancelEventArgs);

            cancel.Cancel = cancelEventArgs.Cancel;
        }
    }
}