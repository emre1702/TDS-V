using RAGE.Ui;
using TDS_Shared.Default;

namespace TDS_Client.Handler
{
    public class LoggingHandler
    {
#pragma warning disable 649
        private readonly bool _outputLogInfo;
#pragma warning restore 649

        public LoggingHandler()
        {
            _outputLogInfo = false;
        }

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
                Console.Log(ConsoleVerbosity.Error, title + "\n", true, false);
            else
                Console.Log(ConsoleVerbosity.Error, "Exception occured" + "\n");
            Console.Log(ConsoleVerbosity.Error, ex.GetBaseException().Message + "\n", true, false);
            Console.Log(ConsoleVerbosity.Error, ex.StackTrace + "\n", true, false);
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
            Console.Log(ConsoleVerbosity.Info, "[I] " + source + msg + "\n", true, false);
        }

        public void LogWarning(string msg, string source = "")
        {
            if (source.Length > 0)
                source = "[" + source + "] ";
            else
                source += " ";
            Console.Log(ConsoleVerbosity.Warning, "[W]" + source + msg + "\n", true, false);
        }
    }
}
