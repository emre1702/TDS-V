using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Data.Interfaces;

namespace TDS.Server.Handler.Entities.GTA.Objects
{
    public partial class TDSObject : ITDSObject
    {
        private readonly IWorkaroundsHandler _workaroundsHandler;

        public TDSObject(NetHandle netHandle, IWorkaroundsHandler workaroundsHandler) : base(netHandle)
            => _workaroundsHandler = workaroundsHandler;
    }
}