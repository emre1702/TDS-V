using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TDS_Server.Handler.Entities.Utility
{

    public class AsyncValueTaskEvent<T>
    {
        private readonly List<Func<T, ValueTask>> _invocationList;
        private readonly object _locker;

        private AsyncValueTaskEvent()
        {
            _invocationList = new List<Func<T, ValueTask>>();
            _locker = new object();
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

        public static AsyncValueTaskEvent<T>? operator -(
            AsyncValueTaskEvent<T> e, Func<T, ValueTask> callback)
        {
            if (callback is null) return null;
            if (e is null) return null;

            lock (e._locker)
            {
                e._invocationList.Remove(callback);
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
                //Assuming we want a serial invocation, for a parallel invocation we can use Task.WhenAll instead
                await callback(arg);
            }
        }
    }

}
