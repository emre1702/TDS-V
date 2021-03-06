﻿using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TDS.Server.Handler.Extensions
{
    public static class SemaphoreSlimExtensions
    {
        private const int _semaphoreTimeout = Timeout.Infinite;

        public static async Task Do(this SemaphoreSlim semaphore, Action action)
        {
            await semaphore.WaitAsync(_semaphoreTimeout).ConfigureAwait(false);
            try
            {
                action();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task<TOutput> Do<TOutput>(this SemaphoreSlim semaphore, Func<TOutput> func)
        {
            await semaphore.WaitAsync(_semaphoreTimeout).ConfigureAwait(false);
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task<TOutput> DoAsync<TOutput>(this SemaphoreSlim semaphore, Func<Task<TOutput>> func)
        {
            await semaphore.WaitAsync(_semaphoreTimeout).ConfigureAwait(false);

            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task<TOutput> Do<TInput, TOutput>(this SemaphoreSlim semaphore, Func<TInput, TOutput> func, TInput input)
        {
            await semaphore.WaitAsync(_semaphoreTimeout).ConfigureAwait(false);

            try
            {
                return func(input);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public static async Task<TOutput> Do<TInput1, TInput2, TOutput>(this SemaphoreSlim semaphore, Func<TInput1, TInput2, TOutput> func, TInput1 input1, TInput2 input2)
        {
            await semaphore.WaitAsync(_semaphoreTimeout).ConfigureAwait(false);

            try
            {
                return func(input1, input2);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
