using GTANetworkAPI;

namespace TDS_Server.RAGE.Startup
{
    class Program : Script
    {
        #nullable disable warnings
        public static BaseAPI BaseAPI;
        public static Core.Startup.Program TDSCore;

        public Program()
        {
            BaseAPI = new BaseAPI();

            TDSCore = new Core.Startup.Program(BaseAPI);
            
        }

    }
}
