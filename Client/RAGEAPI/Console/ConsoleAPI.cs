using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.ModAPI.Console;

namespace TDS_Client.RAGEAPI.Console
{
    class ConsoleAPI : IConsoleAPI
    {
        public void Log(ConsoleVerbosity info, string text, bool saveInFile = false, bool saveInFileAsync = true)
        {
            RAGE.Ui.Console.Log((RAGE.Ui.ConsoleVerbosity)info, text, saveInFile, saveInFileAsync);
        }

        public void Clear()
        {
            RAGE.Ui.Console.Clear();
        }
    }
}
