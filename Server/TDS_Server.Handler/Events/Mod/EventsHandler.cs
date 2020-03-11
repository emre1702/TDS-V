using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Player;

namespace TDS_Server.Handler.Events.Mod
{
    public partial class EventsHandler
    {
        private TDSPlayerHandler _tdsPlayerHandler;

        public EventsHandler(TDSPlayerHandler tdsPlayerHandler)
            => _tdsPlayerHandler = tdsPlayerHandler;

        public delegate void PlayerDelegate(ITDSPlayer player);
        public event PlayerDelegate? PlayerConnected;
        public event PlayerDelegate? PlayerDisconnected;

        public void OnPlayerConnected(IPlayer modPlayer) 
        {
            var tdsPlayer = _tdsPlayerHandler.GetTDSPlayer(modPlayer);
            PlayerConnected?.Invoke(tdsPlayer);
        } 

        public void OnPlayerDisconnected(IPlayer modPlayer) 
        {
            var tdsPlayer = _tdsPlayerHandler.GetTDSPlayer(modPlayer);
            PlayerDisconnected?.Invoke(tdsPlayer);
        }
    }
}
