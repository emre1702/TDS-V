using TDS_Server.Data.Interfaces.ModAPI.Events;

namespace TDS_Server.RAGE.Events.Custom
{
    class BaseCustomEvents : IEvents
    {
        public event IEvents.PlayerDelegate PlayerConnected;
        public event IEvents.PlayerDelegate PlayerDisconnected;


        public delegate void PlayerInternalDelegate(GTANetworkAPI.Player player);
        public event PlayerInternalDelegate PlayerConnectedInternal;
        public event PlayerInternalDelegate PlayerDisconnectedInternal;
    }
}
