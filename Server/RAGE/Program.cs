using AutoMapper;
using GTANetworkAPI;

namespace TDS_Server.RAGE.Startup
{
    class Program : Script
    {
        public static Core.Startup.Program TDSCore;
        internal static BaseAPI BaseAPI;

        public Program()
        {
            BaseAPI = new BaseAPI();

            TDSCore = new Core.Startup.Program(BaseAPI);

        }

    }
}
