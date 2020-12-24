using GTANetworkAPI;
using System;
using TDS.Server.Data.Abstracts.Entities.GTA;
using TDS.Server.Handler;
using TDS.Server.Handler.Events;

namespace TDS.Server.Core.Events
{
    public class ColShapeEvents : Script
    {
        [ServerEvent(Event.PlayerEnterColshape)]
        public void PlayerEnterColshape(ITDSColshape colShape, ITDSPlayer player)
        {
            try
            {
                EventsHandler.Instance.OnPlayerEnterColshape(colShape, player);
            }
            catch (Exception ex)
            {
                LoggingHandler.Instance?.LogError(ex);
            }
        }
    }
}
