using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TDS.Server.Data.Utility
{
#nullable enable

    public class AsyncValueTaskEvent<T>
    {
        private readonly List<Func<T, ValueTask>> _invocationList;
        private readonly object _locker;

        private AsyncValueTaskEvent()
        {
            _invocationList = new List<Func<T, ValueTask>>();
            _locker = new object();
        }

        public static AsyncValueTaskEvent<T> operator -(
            AsyncValueTaskEvent<T> e, Func<T, ValueTask> callback)
        {
            lock (e._locker)
            {
                e._invocationList.Remove(callback);
            }
            return e;
        }

        public static AsyncValueTaskEvent<T> operator +(
                    AsyncValueTaskEvent<T>? e, Func<T, ValueTask> callback)
        {
            if (e is null) e = new AsyncValueTaskEvent<T>();

            lock (e._locker)
            {
                e._invocationList.Add(callback);
            }
            return e;
        }

        public async ValueTask InvokeAsync(T arg)
        {
            List<Func<T, ValueTask>> tmpInvocationList;
            lock (_locker)
            {
                tmpInvocationList = new List<Func<T, ValueTask>>(_invocationList);
            }

            foreach (var callback in tmpInvocationList)
            {
                await callback(arg).ConfigureAwait(false);
            }
        }
    }

    public class AsyncValueTaskEvent
    {
        private readonly List<Func<ValueTask>> _invocationList;
        private readonly object _locker;

        private AsyncValueTaskEvent()
        {
            _invocationList = new List<Func<ValueTask>>();
            _locker = new object();
        }

        public static AsyncValueTaskEvent operator -(
            AsyncValueTaskEvent e, Func<ValueTask> callback)
        {
            lock (e._locker)
            {
                e._invocationList.Remove(callback);
            }
            return e;
        }

        public static AsyncValueTaskEvent operator +(
                    AsyncValueTaskEvent? e, Func<ValueTask> callback)
        {
            if (e is null) e = new AsyncValueTaskEvent();

            lock (e._locker)
            {
                e._invocationList.Add(callback);
            }
            return e;
        }

        public async ValueTask InvokeAsync()
        {
            List<Func<ValueTask>> tmpInvocationList;
            lock (_locker)
            {
                tmpInvocationList = new List<Func<ValueTask>>(_invocationList);
            }

            foreach (var callback in tmpInvocationList)
            {
                await callback().ConfigureAwait(false);
            }
        }
    }
}
