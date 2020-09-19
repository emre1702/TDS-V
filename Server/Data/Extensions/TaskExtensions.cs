using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Extensions
{
    public static class TaskExtensions
    {
        [ThreadStatic]
        public static bool IsMainThread = false;

        public static void Run(this GTANetworkMethods.Task task, Action action)
        {
            if (IsMainThread)
                action();
            else
                task.Run(action);
        }

        public static async Task RunWait(this GTANetworkMethods.Task task, Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            Run(task, () =>
            {
                action();
                taskCompletionSource.SetResult(true);
            });
            await taskCompletionSource.Task;
        }

        public static Task<T> RunWait<T>(this GTANetworkMethods.Task task, Func<T> action)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            Run(task, () =>
            {
                var result = action();
                taskCompletionSource.SetResult(result);
            });
            return taskCompletionSource.Task;
        }
    }
}
