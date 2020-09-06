using TDS_Server.Data.Interfaces;

namespace TDS_Server.Handler
{
    public class WeaponLevelHandler
    {
        private readonly ILoggingHandler _loggingHandler;

        public WeaponLevelHandler(ILoggingHandler loggingHandler)
            => _loggingHandler = loggingHandler;
    }
}
