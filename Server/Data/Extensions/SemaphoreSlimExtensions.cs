using System;
using System.Threading;
using System.Threading.Tasks;

namespace TDS_Server.Data.Extensions
{
    public static class SemaphoreSlimExtensions
    {
        public static async Task Do(this SemaphoreSlim semaphore, Action action)
        {
            await semaphore.WaitAsync(5000);

            try
            {
                action();
            }
            catch
            {
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task<TOutput> Do<TOutput>(this SemaphoreSlim semaphore, Func<TOutput> func)
        {
            await semaphore.WaitAsync(5000);

            try
            {
                return func();
            }
            catch
            {
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task<TOutput> DoAsync<TOutput>(this SemaphoreSlim semaphore, Func<Task<TOutput>> func)
        {
            await semaphore.WaitAsync(5000);

            try
            {
                return await func();
            }
            catch
            {
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task<TOutput> Do<TInput, TOutput>(this SemaphoreSlim semaphore, Func<TInput, TOutput> func, TInput input)
        {
            await semaphore.WaitAsync(5000);

            try
            {
                return func(input);
            }
            catch
            {
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task<TOutput> Do<TInput1, TInput2, TOutput>(this SemaphoreSlim semaphore, Func<TInput1, TInput2, TOutput> func, TInput1 input1, TInput2 input2)
        {
            await semaphore.WaitAsync(5000);

            try
            {
                return func(input1, input2);
            }
            catch
            {
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
