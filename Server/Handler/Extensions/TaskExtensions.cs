using System;
using System.Threading.Tasks;
using TDS.Shared.Core;

namespace TDS.Server.Handler.Extensions
{
    public static class TaskExtensions
    {
        [ThreadStatic]
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static bool IsMainThread = false;

#pragma warning restore CA2211 // Non-constant fields should not be visible

        public static void RunSafe(this GTANetworkMethods.Task task, Action action, int delayTime = 0)
        {
            try
            {
                if (IsMainThread)
                    if (delayTime == 0)
                        action();
                    else
                        _ = new TDSTimer(action, (uint)delayTime);
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
            var taskCompletionSource = new TaskCompletionSource<T>(TaskCreationOptions.RunContinuationsAsynchronously);
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