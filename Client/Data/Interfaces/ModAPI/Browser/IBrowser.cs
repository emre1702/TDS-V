namespace TDS_Client.Data.Interfaces.ModAPI.Browser
{
    public interface IBrowser
    {
        void Destroy();
        void ExecuteJs(string js);
    }
}
