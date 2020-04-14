using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler
{
    public abstract class ServiceBase
    {
        protected IModAPI ModAPI { get; }
        protected LoggingHandler Logging { get; }

        protected ServiceBase(IModAPI modAPI, LoggingHandler loggingHandler)
        {
            ModAPI = modAPI;
            Logging = loggingHandler;
        }
    }
}
