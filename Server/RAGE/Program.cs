using GTANetworkAPI;

namespace TDS_Server.RAGE.Startup
{
    class Program : Script
    {
        public Program()
        {
            var baseAPI = new BaseAPI();

            var tdsCore = new Core.Startup.Program(baseAPI);
        }

    }
}
