using TDS.Client.Data.Defaults;
using TDS.Shared.Core;

namespace TDS.Client.Handler.Browser
{
    public class MapCreatorObjectChoiceBrowserHandler : BrowserHandlerBase
    {
        public MapCreatorObjectChoiceBrowserHandler(LoggingHandler loggingHandler)
            : base(loggingHandler, Constants.AngularMapCreatorObjectChoiceBrowserPath)
        {
        }
    }
}