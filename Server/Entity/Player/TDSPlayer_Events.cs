﻿using TDS_Shared.Default;

namespace TDS_Server.Entity.Player
{
    partial class TDSPlayer
    {
        #region Public Methods

        public void SendBrowserEvent(string eventName, params object[] args)
        {
            object[] newArgs = new object[args.Length + 1];
            newArgs[0] = eventName;
            for (int i = 0; i < args.Length; ++i)
            {
                newArgs[i + 1] = args[i];
            }
            Emit(ToClientEvent.ToBrowserEvent, newArgs);
        }

        public void SendEvent(string eventName, params object[] args)
        {
            Emit(eventName, args);
        }

        #endregion Public Methods
    }
}
