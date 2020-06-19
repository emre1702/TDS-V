using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.ModAPI.Thread;

namespace TDS_Server.RAGEAPI.Thread
{
    internal class ThreadAPI : IThreadAPI
    {
        #region Public Methods

        public void QueueIntoMainThread(Action action)
        {
            GTANetworkAPI.NAPI.Task.Run(action);
        }

        public async Task RunInMainThread(Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            GTANetworkAPI.NAPI.Task.Run(() =>
            {
                action();
                taskCompletionSource.SetResult(true);
            });
            await taskCompletionSource.Task;
        }

        public Task<T> RunInMainThread<T>(Func<T> action)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            GTANetworkAPI.NAPI.Task.Run(() =>
            {
                var result = action();
                taskCompletionSource.SetResult(result);
            });
            return taskCompletionSource.Task;
        }

        #endregion Public Methods
    }
}
