using TDS_Client.Data.Enums;
using TDS_Client.Data.Interfaces.RAGE.Ui.Console;

namespace TDS_Client.RAGEAPI.Console
{
    internal class ConsoleAPI : IConsoleAPI
    {
        #region Public Methods

        public void Clear()
        {
            RAGE.Ui.Console.Clear();
        }

        public void Log(ConsoleVerbosity info, string text, bool saveInFile = false, bool saveInFileAsync = true)
        {
            RAGE.Ui.Console.Log((RAGE.Ui.ConsoleVerbosity)info, text, saveInFile, saveInFileAsync);
        }

        #endregion Public Methods
    }
}