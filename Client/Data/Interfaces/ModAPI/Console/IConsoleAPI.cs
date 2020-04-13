using TDS_Client.Data.Enums;

namespace TDS_Client.Data.Interfaces.ModAPI.Console
{
    public interface IConsoleAPI
    {
        void Clear();
        void Log(ConsoleVerbosity info, string text, bool saveInFile = false, bool saveInFileAsync = true);
    }
}
