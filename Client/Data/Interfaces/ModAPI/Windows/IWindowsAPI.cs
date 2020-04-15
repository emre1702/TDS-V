namespace TDS_Client.Data.Interfaces.ModAPI.Windows
{
    public interface IWindowsAPI
    {
        bool Focused { get; }
        bool Fullscreen { get; }

        void Notify(string title, string text = "", string attribute = "", int duration = 0, bool silent = false);
    }
}
