namespace TDS_Client.Data.Interfaces.ModAPI.Browser
{
    public interface IBrowser
    {
        void Destroy();
        void ExecuteJs(string js);
        void MarkAsChat();
        void Call(string eventName, params object[] args);
    }
}
