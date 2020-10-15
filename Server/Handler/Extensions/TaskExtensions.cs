using System;
using System.Threading.Tasks;

namespace TDS_Server.Handler.Extensions
{
    public static class TaskExtensions
    {
        [ThreadStatic]
        public static bool IsMainThread = false;

        public static void RunCustom(this GTANetworkMethods.Task task, Action action, int delayTime = 0)
        {
            if (IsMainThread)
                action();
            else
                task.Run(action, delayTime);
        }

        public static async Task RunWait(this GTANetworkMethods.Task task, Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            RunCustom(task, () =>
            {
                action();
                taskCompletionSource.SetResult(true);
            });
            await taskCompletionSource.Task;
        }

        public static Task<T> RunWait<T>(this GTANetworkMethods.Task task, Func<T> action)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            RunCustom(task, () =>
            {
                var result = action();
                taskCompletionSource.SetResult(result);
            });
            return taskCompletionSource.Task;
        }
    }
}
