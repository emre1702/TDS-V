using GTANetworkAPI;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Handler.Events;

namespace TDS.Server.Core.Events
{
    public class ColShapeEvents : Script
    {
        [ServerEvent(Event.PlayerEnterColshape)]
        public void PlayerEnterColshape(ITDSColshape colShape, ITDSPlayer player)
        {
            EventsHandler.Instance.OnPlayerEnterColshape(colShape, player);
        }
    }
}
