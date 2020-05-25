using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.Update)]
        public void OnUpdate()
        {
            Init.TDSCore.EventsHandler.OnUpdate();
        }

        #endregion Public Methods
    }
}
