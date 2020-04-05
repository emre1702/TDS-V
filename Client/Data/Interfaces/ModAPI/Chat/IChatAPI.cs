namespace TDS_Client.Data.Interfaces.ModAPI.Chat
{
    public interface IChatAPI
    {
        void Output(string msg);

        bool SafeMode { get; set; }

        void Show(bool show);
    }
}
