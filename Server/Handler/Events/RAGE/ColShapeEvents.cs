using GTANetworkAPI;
using TDS_Server.Data.Abstracts.Entities.GTA;

namespace TDS_Server.Handler.Events.RAGE
{
    public class ColShapeEvents : Script
    {
        [ServerEvent(Event.PlayerEnterColshape)]
        public void PlayerEnterColshape(ITDSColShape colShape, ITDSPlayer player)
        {
            EventsHandler.Instance.OnPlayerEnterColshape(colShape, player);
        }
    }
}
