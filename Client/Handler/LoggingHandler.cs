using System;
using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler
{
    public class LoggingHandler
    {
#pragma warning disable 649
        private readonly bool _outputLogInfo;
#pragma warning restore 649

        private readonly IModAPI _modAPI;

        public LoggingHandler(IModAPI modAPI)
        {
            _modAPI = modAPI;
            _outputLogInfo = true;
        }

        public void LogError(Exception ex, string title = null)
        {
            if (title != null)
                _modAPI.Console.Log(ConsoleVerbosity.Error, title + "\n", true, false);
            else
                _modAPI.Console.Log(ConsoleVerbosity.Error, "Exception occured" + "\n");
            _modAPI.Console.Log(ConsoleVerbosity.Error, ex.Message + "\n", true, false);
            _modAPI.Console.Log(ConsoleVerbosity.Error, ex.StackTrace + "\n", true, false);
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
            _modAPI.Console.Log(ConsoleVerbosity.Info, "[I] " + source + msg + "\n", true, false);
        }

        public void LogWarning(string msg, string source = "")
        {
            if (source.Length > 0)
                source = "[" + source + "] ";
            else
                source += " ";
            _modAPI.Console.Log(ConsoleVerbosity.Warning, "[W]" + source + msg + "\n", true, false);
        }
    }
}
