using System;
using System.Threading.Tasks;
using TDS_Shared.Core;

namespace TDS_Server.Handler.Extensions
{
    public static class TaskExtensions
    {
        [ThreadStatic]
        public static bool IsMainThread = false;

        public static void RunSafe(this GTANetworkMethods.Task task, Action action, int delayTime = 0)
        {
            try
            {
                if (IsMainThread)
                    if (delayTime == 0)
                        action();
                    else 
                        new TDSTimer(action, (uint)delayTime);
                else
                    task.Run(() => 
                    {
                        try
                        {
                            action();
                        }
                        catch (Exception ex)
                        {
                            LoggingHandler.Instance?.LogError(ex);
                        }
                    }, delayTime);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }

        public static async Task RunWait(this GTANetworkMethods.Task task, Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            RunSafe(task, () =>
            {
                action();
                taskCompletionSource.SetResult(true);
            });
            await taskCompletionSource.Task.ConfigureAwait(false);
        }

        public static Task<T> RunWait<T>(this GTANetworkMethods.Task task, Func<T> action)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            RunSafe(task, () =>
            {
                var result = action();
                taskCompletionSource.SetResult(result);
            });
            return taskCompletionSource.Task;
        }

        public static async void IgnoreResult(this Task task)
        {
            try
            {
                await task.ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }
    }
}
