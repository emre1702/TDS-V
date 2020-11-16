using TDS.Client.Data.Defaults;
using TDS.Shared.Core;

namespace TDS.Client.Handler.Browser
{
    public class MapCreatorVehicleChoiceBrowserHandler : BrowserHandlerBase
    {
        public MapCreatorVehicleChoiceBrowserHandler(LoggingHandler loggingHandler)
            : base(loggingHandler, Constants.AngularMapCreatorVehicleChoiceBrowserPath)
        {
        }
    }
}