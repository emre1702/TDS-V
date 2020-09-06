namespace TDS_Client.Handler
{
    public abstract class ServiceBase
    {
        protected LoggingHandler Logging { get; }

        protected ServiceBase(LoggingHandler loggingHandler) => Logging = loggingHandler;
    }
}
