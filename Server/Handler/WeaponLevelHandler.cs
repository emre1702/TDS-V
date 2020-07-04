using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;

namespace TDS_Server.Handler
{
    public class WeaponLevelHandler
    {
        private readonly IModAPI _modAPI;
        private readonly ILoggingHandler _loggingHandler;

        public WeaponLevelHandler(IModAPI modAPI, ILoggingHandler loggingHandler)
        {
            _modAPI = modAPI;
            _loggingHandler = loggingHandler;
        }
    }
}
