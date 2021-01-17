using RAGE.Ui;
using TDS.Shared.Default;

namespace TDS.Client.Handler
{
    public class LoggingHandler
    {
#pragma warning disable 649
        private readonly bool _outputLogInfo;
#pragma warning restore 649

        public LoggingHandler() => _outputLogInfo = false;

        public void LogToServer(string msg, string source = "")
        {
            RAGE.Events.CallRemote(ToServerEvent.LogMessageToServer, msg, source);
        }

        public void LogToServer(System.Exception ex, string title = null)
        {
            string message = title is null ? ex.GetBaseException().Message : $"[{title}] " + ex.GetBaseException().Message;
            RAGE.Events.CallRemote(ToServerEvent.LogExceptionToServer, message, ex.StackTrace, ex.GetType().Name);
        }

        public void LogError(System.Exception ex, string title = null)
        {
            if (title != null)
                Console.LogLine(ConsoleVerbosity.Error, title, true, false);
            else
                Console.LogLine(ConsoleVerbosity.Error, "Exception occured");
            Console.LogLine(ConsoleVerbosity.Error, ex.GetBaseException().Message, true, false);
            Console.LogLine(ConsoleVerbosity.Error, ex.StackTrace, true, false);
        }

        public void LogInfo(string msg, string source = "", bool isEnd = false)
        {
            if (!_outputLogInfo)
                return;

            if (source.Length > 0)
                source = "[" + source + "]";
            if (isEnd)
                source += "[END] ";
            else
                source += " ";
            Console.LogLine(ConsoleVerbosity.Info, "[I] " + source + msg, true, false);
        }

        public void LogWarning(string msg, string source = "")
        {
            if (source.Length > 0)
                source = "[" + source + "] ";
            else
                source += " ";
            Console.LogLine(ConsoleVerbosity.Warning, "[W]" + source + msg, true, false);
        }
    }
}