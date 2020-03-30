using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Data.Interfaces.ModAPI.Browser;
using TDS_Client.Data.Interfaces.ModAPI.Native;
using TDS_Client.RAGEAPI.Browser;
using TDS_Client.RAGEAPI.Native;

namespace TDS_Client.RAGEAPI
{
    class BaseAPI : IModAPI
    {
        public IBrowserAPI Browser { get; }
        public INativeAPI Native { get; }

        public BaseAPI()
        {
            Browser = new BrowserAPI();
            Native = new NativeAPI();
        }
    }
}
