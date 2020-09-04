using System;
using System.Threading.Tasks;

namespace TDS_Server.Data.Extensions
{
    public static class TaskExtensions
    {
        public static async Task RunWait(this GTANetworkMethods.Task task, Action action)
        {
            var taskCompletionSource = new TaskCompletionSource<bool>();
            task.Run(() =>
            {
                action();
                taskCompletionSource.SetResult(true);
            });
            await taskCompletionSource.Task;
        }

        public static Task<T> RunWait<T>(this GTANetworkMethods.Task task, Func<T> action)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            task.Run(() =>
            {
                var result = action();
                taskCompletionSource.SetResult(result);
            });
            return taskCompletionSource.Task;
        }
    }
}
