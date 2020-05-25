using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace TDS_Server.Database
{
    public class CustomDBLogger : ILoggerProvider
    {
        #region Private Fields

        private readonly static object _locker = new object();
        private static string _path;
        private readonly SafeHandle _handle = new SafeFileHandle(IntPtr.Zero, true);
        private bool _disposed = false;

        #endregion Private Fields

        #region Public Constructors

        public CustomDBLogger(string path)
            => _path = path;

        #endregion Public Constructors

        #region Public Methods

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _handle.Dispose();
            }

            _disposed = true;
        }

        #endregion Protected Methods

        #region Private Classes

        private class CustomLogger : ILogger
        {
            #region Private Fields

            private readonly Queue<string> _logQuery = new Queue<string>();

            #endregion Private Fields

            #region Public Methods

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                if (!System.Diagnostics.Debugger.IsAttached)
                    return;

                string msg = Environment.NewLine + "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "] " + formatter(state, exception) + Environment.NewLine;
                try
                {
                    lock (_locker)
                    {
                        if (_logQuery.Count > 0)
                        {
                            foreach (var str in _logQuery)
                            {
                                File.AppendAllText(_path, str);
                            }
                            _logQuery.Clear();
                        }

                        File.AppendAllText(_path, msg);
                    }
                }
                catch
                {
                    _logQuery.Enqueue(msg);
                }
            }

            #endregion Public Methods
        }

        #endregion Private Classes
    }
}
