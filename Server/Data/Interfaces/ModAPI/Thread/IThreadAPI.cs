using System;

namespace TDS_Server.Data.Interfaces.ModAPI.Thread
{
    public interface IThreadAPI
    {
        #region Public Methods

        void RunInMainThread(Action action);

        #endregion Public Methods
    }
}
