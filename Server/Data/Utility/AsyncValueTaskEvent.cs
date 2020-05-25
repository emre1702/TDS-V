using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TDS_Server.Data.Utility
{
#nullable enable

    public class AsyncValueTaskEvent<T>
    {
        #region Private Fields

        private readonly List<Func<T, ValueTask>> _invocationList;
        private readonly object _locker;

        #endregion Private Fields

        #region Private Constructors

        private AsyncValueTaskEvent()
        {
            _invocationList = new List<Func<T, ValueTask>>();
            _locker = new object();
        }

        #endregion Private Constructors

        #region Public Methods

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
                //Assuming we want a serial invocation, for a parallel invocation we can use Task.WhenAll instead
                await callback(arg);
            }
        }

        #endregion Public Methods
    }
}
