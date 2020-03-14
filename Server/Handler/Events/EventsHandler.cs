using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Entities.Player;
using TDS_Server.Handler.Player;

namespace TDS_Server.Handler.Events
{
    public class EventsHandler
    {
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        public EventsHandler(TDSPlayerHandler tdsPlayerHandler)
            => _tdsPlayerHandler = tdsPlayerHandler;

        public delegate void PlayerDelegate(TDSPlayer player);
        public event PlayerDelegate? PlayerConnected;
        public event PlayerDelegate? PlayerDisconnected;
        public event PlayerDelegate? PlayerLoggedIn;
        public event PlayerDelegate? PlayerRegistered;
        public event PlayerDelegate? PlayerLoggedOut;

        public delegate void PlayerLobbyDelegate(TDSPlayer player, Lobby lobby);
        public event PlayerLobbyDelegate? PlayerJoinedLobby;
        public event PlayerLobbyDelegate? PlayerLeftLobby;

        public delegate void CounterDelegate(int counter);
        public event CounterDelegate? OnSecond;
        public event CounterDelegate? OnMinute;
        public event CounterDelegate? OnHour;

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

        public void OnPlayerLogin(TDSPlayer tdsPlayer)
        {
            PlayerLoggedIn?.Invoke(tdsPlayer);
        }

        public void OnPlayerRegister(TDSPlayer tdsPlayer)
        {
            PlayerRegistered?.Invoke(tdsPlayer);
        }
    }
}
