using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;
using TDS_Server.Handler.Events;

namespace TDS_Server.Core.Events
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
