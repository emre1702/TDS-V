using TDS.Server.Data.Interfaces;

namespace TDS.Server.Handler
{
    public class WeaponLevelHandler
    {
        private readonly ILoggingHandler _loggingHandler;

        public WeaponLevelHandler(ILoggingHandler loggingHandler)
            => _loggingHandler = loggingHandler;
    }
}
