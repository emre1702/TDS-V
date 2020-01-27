using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace TDS_Server_DB
{
    class CustomDBLogger : ILoggerProvider
    {
        private static object locker = new object();

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        private class CustomLogger : ILogger
        {
            private readonly Queue<string> _logQuery = new Queue<string>();

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
                    lock (locker)
                    {
                        if (_logQuery.Count > 0)
                        {
                            foreach (var str in _logQuery)
                            {
                                File.AppendAllText(@"D:\DBLogs\FromCsharp\log.txt", str);
                            }
                            _logQuery.Clear();
                        }

                        File.AppendAllText(@"D:\DBLogs\FromCsharp\log.txt", msg);
                    }
                   
                }
                catch
                {
                    _logQuery.Enqueue(msg);
                }
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}
