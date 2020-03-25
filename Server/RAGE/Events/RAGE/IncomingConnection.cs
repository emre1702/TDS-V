using GTANetworkAPI;
using TDS_Server.RAGE.Startup;

namespace TDS_Server.RAGE.Events.RAGE
{
    partial class BaseRAGEEvents
    {
        [ServerEvent(Event.IncomingConnection)]
        public void OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            var cancelEventArgs = new System.ComponentModel.CancelEventArgs();
            Init.TDSCore.EventsHandler.OnIncomingConnection(ip, serial, socialClubName, socialClubId, cancelEventArgs);

            cancel.Cancel = cancelEventArgs.Cancel;
        }
    }
}
