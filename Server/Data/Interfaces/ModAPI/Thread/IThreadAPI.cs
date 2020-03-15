using System;

namespace TDS_Server.Data.Interfaces.ModAPI.Thread
{
    public interface IThreadAPI
    {
        void RunInMainThread(Action action);
    }
}
