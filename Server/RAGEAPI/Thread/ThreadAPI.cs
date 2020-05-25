using GTANetworkAPI;
using System;
using TDS_Server.Data.Interfaces.ModAPI.Thread;

namespace TDS_Server.RAGEAPI.Thread
{
    internal class ThreadAPI : IThreadAPI
    {
        #region Public Methods

        public void RunInMainThread(Action action)
        {
            NAPI.Task.Run(action);
        }

        #endregion Public Methods
    }
}
