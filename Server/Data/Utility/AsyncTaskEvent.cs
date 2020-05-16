using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TDS_Server.Data.Utility
{
    #nullable enable
    public class AsyncTaskEvent<T>
    {
        private readonly List<Func<object, T, Task>> _invocationList;
        private readonly object _locker;

        private AsyncTaskEvent()
        {
            _invocationList = new List<Func<object, T, Task>>();
            _locker = new object();
        }

        public static AsyncTaskEvent<T>? operator +(
            AsyncTaskEvent<T> e, Func<object, T, Task> callback)
        {
            if (callback == null) throw new NullReferenceException("callback is null");

            if (e == null) e = new AsyncTaskEvent<T>();

            lock (e._locker)
            {
                e._invocationList.Add(callback);
            }
            return e;
        }

        public static AsyncTaskEvent<T>? operator -(
            AsyncTaskEvent<T> e, Func<object, T, Task> callback)
        {
            if (callback == null) throw new NullReferenceException("callback is null");
            if (e == null) return null;

            lock (e._locker)
            {
                e._invocationList.Remove(callback);
            }
            return e;
        }

        public async Task InvokeAsync(object sender, T eventArgs)
        {
            List<Func<object, T, Task>> tmpInvocationList;
            lock (_locker)
            {
                tmpInvocationList = new List<Func<object, T, Task>>(_invocationList);
            }

            foreach (var callback in tmpInvocationList)
            {
                //Assuming we want a serial invocation, for a parallel invocation we can use Task.WhenAll instead
                await callback(sender, eventArgs);
            }
        }

        
    }
}
