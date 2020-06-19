using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Interfaces.ModAPI.Thread
{
    public interface IThreadAPI
    {
        #region Public Methods

        /// <summary>
        /// Queues an action to the main thread. Will not get executed instantly!! QUEUES IT ONLY!
        /// </summary>
        /// <param name="action"></param>
        void QueueIntoMainThread(Action action);

        Task RunInMainThread(Action action);

        Task<T> RunInMainThread<T>(Func<T> action);

        #endregion Public Methods
    }
}
