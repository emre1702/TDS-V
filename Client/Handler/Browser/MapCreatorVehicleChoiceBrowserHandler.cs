using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class MapCreatorVehicleChoiceBrowserHandler : BrowserHandlerBase
    {
        #region Public Constructors

        public MapCreatorVehicleChoiceBrowserHandler(IModAPI modAPI, LoggingHandler loggingHandler, Serializer serializer)
            : base(modAPI, loggingHandler, serializer, Constants.AngularMapCreatorVehicleChoiceBrowserPath)
        {
        }

        #endregion Public Constructors
    }
}
