using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Shared.Core;

namespace TDS_Client.Handler.Browser
{
    public class MapCreatorObjectChoiceBrowserHandler : BrowserHandlerBase
    {
        public MapCreatorObjectChoiceBrowserHandler(IModAPI modAPI, LoggingHandler loggingHandler, Serializer serializer)
            : base(modAPI, loggingHandler, serializer, Constants.AngularMapCreatorObjectChoiceBrowserPath)
        {

        }
    }
}
