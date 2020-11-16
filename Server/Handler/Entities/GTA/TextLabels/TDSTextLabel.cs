using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;

namespace TDS.Server.Handler.Entities.GTA.TextLabels
{
    public class TDSTextLabel : ITDSTextLabel
    {
        public TDSTextLabel(NetHandle netHandle) : base(netHandle)
        {
        }
    }
}
