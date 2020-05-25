using TDS_Client.Data.Interfaces.ModAPI;

namespace TDS_Client.Handler
{
    public abstract class ServiceBase
    {
        #region Protected Constructors

        protected ServiceBase(IModAPI modAPI, LoggingHandler loggingHandler)
        {
            ModAPI = modAPI;
            Logging = loggingHandler;
        }

        #endregion Protected Constructors

        #region Protected Properties

        protected LoggingHandler Logging { get; }
        protected IModAPI ModAPI { get; }

        #endregion Protected Properties
    }
}
