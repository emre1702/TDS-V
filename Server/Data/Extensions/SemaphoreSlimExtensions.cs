﻿using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TDS_Server.Data.Extensions
{
    public static class SemaphoreSlimExtensions
    {
        private const int _semaphoreTimeout = Timeout.Infinite;

        public static async Task Do(this SemaphoreSlim semaphore, Action action, [CallerMemberName] string calledFrom = "")
        {
            await semaphore.WaitAsync(_semaphoreTimeout);
            Console.WriteLine($"Semaphore started from: {calledFrom}");
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
                Console.WriteLine($"Semaphore ended from: {calledFrom}");
            }
        }

        public static async Task<TOutput> Do<TOutput>(this SemaphoreSlim semaphore, Func<TOutput> func, [CallerMemberName] string calledFrom = "")
        {
            await semaphore.WaitAsync(_semaphoreTimeout);
            Console.WriteLine($"Semaphore started from: {calledFrom}");
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
                Console.WriteLine($"Semaphore ended from: {calledFrom}");
            }
        }

        public static async Task<TOutput> DoAsync<TOutput>(this SemaphoreSlim semaphore, Func<Task<TOutput>> func, [CallerMemberName] string calledFrom = "")
        {
            await semaphore.WaitAsync(_semaphoreTimeout);
            Console.WriteLine($"Semaphore started from: {calledFrom}");

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
                Console.WriteLine($"Semaphore ended from: {calledFrom}");
            }
        }

        public static async Task<TOutput> Do<TInput, TOutput>(this SemaphoreSlim semaphore, Func<TInput, TOutput> func, TInput input, [CallerMemberName] string calledFrom = "")
        {
            await semaphore.WaitAsync(_semaphoreTimeout);
            Console.WriteLine($"Semaphore started from: {calledFrom}");

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
                Console.WriteLine($"Semaphore ended from: {calledFrom}");
            }
        }

        public static async Task<TOutput> Do<TInput1, TInput2, TOutput>(this SemaphoreSlim semaphore, Func<TInput1, TInput2, TOutput> func, TInput1 input1, TInput2 input2, [CallerMemberName] string calledFrom = "")
        {
            await semaphore.WaitAsync(_semaphoreTimeout);
            Console.WriteLine($"Semaphore started from: {calledFrom}");

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
                Console.WriteLine($"Semaphore ended from: {calledFrom}");
            }
        }
    }
}
