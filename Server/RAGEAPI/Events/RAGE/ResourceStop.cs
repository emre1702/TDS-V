using GTANetworkAPI;

namespace TDS_Server.RAGEAPI.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        #region Public Methods

        [ServerEvent(Event.ResourceStop)]
        public void ResourceStop()
        {
            Init.TDSCore.EventsHandler.OnResourceStop();
        }

        #endregion Public Methods
    }
}
