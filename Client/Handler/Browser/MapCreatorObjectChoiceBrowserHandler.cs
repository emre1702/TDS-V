using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Shared.Core;

namespace TDS_Client.Manager.Browser.Angular
{
    public class MapCreatorObjectChoiceBrowserHandler : BrowserHandlerBase
    {
        public MapCreatorObjectChoiceBrowserHandler(IModAPI modAPI, Serializer serializer)
            : base(modAPI, serializer, Constants.AngularMapCreatorObjectChoiceBrowserPath)
        {

        }
    }
}
