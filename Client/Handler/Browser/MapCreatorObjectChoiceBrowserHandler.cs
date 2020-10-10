using TDS_Client.Data.Defaults;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class MapCreatorObjectChoiceBrowserHandler : BrowserHandlerBase
    {
        public MapCreatorObjectChoiceBrowserHandler(LoggingHandler loggingHandler)
            : base(loggingHandler, Constants.AngularMapCreatorObjectChoiceBrowserPath)
        {
        }
    }
}