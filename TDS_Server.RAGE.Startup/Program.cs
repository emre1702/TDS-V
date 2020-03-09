using GTANetworkAPI;

namespace TDS_Server.RAGE.Startup
{
    class Program : Script
    {
        public Program()
        {
            new TDS_Server.Core.Startup.Program(new BaseAPI());

            NAPI.Player.SetPlayerHealth
        }
    }
}
