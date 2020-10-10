using TDS_Client.Data.Defaults;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class MapCreatorVehicleChoiceBrowserHandler : BrowserHandlerBase
    {
        public MapCreatorVehicleChoiceBrowserHandler(LoggingHandler loggingHandler)
            : base(loggingHandler, Constants.AngularMapCreatorVehicleChoiceBrowserPath)
        {
        }
    }
}