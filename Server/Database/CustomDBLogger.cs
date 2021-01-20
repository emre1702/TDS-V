using Microsoft.Extensions.Logging;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace TDS.Server.Database
{
    public class CustomDBLogger : ILoggerProvider
    {
        private static readonly Dictionary<LogLevel, List<string>> _pathsByLogLevel = new();

        public CustomDBLogger(IEnumerable<(string Level, string Path)> loggingSettings)
        {
            foreach (var loggingSetting in loggingSettings)
            {
                if (!Enum.TryParse<LogLevel>(loggingSetting.Level, true, out var logLevel))

                {
                    Console.WriteLine($"LogLevel {loggingSetting.Level} is used in TDS.Server.config but is not defined in 'LogLevel' enum!");

                    continue;
                }

                if (!_pathsByLogLevel.TryGetValue(logLevel, out var list))
                {
                    list = new();
                    _pathsByLogLevel[logLevel] = list;
                }

                list.Add(loggingSetting.Path);
            }
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        private class CustomLogger : ILogger
        {
            private readonly Queue<string> _logQuery = new Queue<string>();
            private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                lock (_pathsByLogLevel)
                {
                    return _pathsByLogLevel.ContainsKey(logLevel);
                }
            }

            public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                var msg = Environment.NewLine + "[" + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + "] " + formatter(state, exception) + Environment.NewLine;

                await _semaphore.WaitAsync();
                try
                {
                    List<string> paths;
                    lock (_pathsByLogLevel)
                    {
                        if (!_pathsByLogLevel.TryGetValue(logLevel, out paths) || paths.Count == 0)
                            return;
                    }

                    if (_logQuery.Count > 0)
                    {
                        foreach (var str in _logQuery)
                            await WriteText(str, paths);
                        _logQuery.Clear();
                    }
                    await WriteText(msg, paths);
                }
                catch
                {
                    _logQuery.Enqueue(msg);
                }
                finally
                {
                    _semaphore.Release();
                }
            }

            private async Task WriteText(string text, List<string> filePaths)
            {
                foreach (var path in filePaths)
                    await File.AppendAllTextAsync(path, text);
            }
        }
    }
}