using GTANetworkAPI;
using System;
using TDS_Server.Data.Interfaces.ModAPI.Thread;

namespace TDS_Server.RAGEAPI.Thread
{
    class ThreadAPI : IThreadAPI
    {
        public void RunInMainThread(Action action)
        {
            NAPI.Task.Run(action);
        }
    }
}
