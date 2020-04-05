using System.Collections.Generic;
using TDS_Client.Data.Defaults;
using TDS_Client.Data.Interfaces.ModAPI;
using TDS_Client.Handler.Browser;
using TDS_Shared.Core;

namespace TDS_Client.Manager.Browser.Angular
{
    public class MapCreatorVehicleChoiceBrowserHandler : BrowserHandlerBase
    {
        public MapCreatorVehicleChoiceBrowserHandler(IModAPI modAPI, Serializer serializer)
            : base(modAPI, serializer, Constants.AngularMapCreatorVehicleChoiceBrowserPath)
        {

        }
    }
}
