using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Player;

namespace TDS_Server.Handler.Events
{
    public class EventsHandler
    {
        private readonly TDSPlayerHandler _tdsPlayerHandler;
        private readonly ILoggingHandler _loggingHandler;

        public EventsHandler(TDSPlayerHandler tdsPlayerHandler, ILoggingHandler loggingHandler)
            => (_tdsPlayerHandler, _loggingHandler) = (tdsPlayerHandler, loggingHandler);

        public delegate void PlayerDelegate(ITDSPlayer player);
        public event PlayerDelegate? PlayerConnected;

        public event PlayerDelegate? PlayerDisconnected;
        public event PlayerDelegate? PlayerLoggedIn;
        public event PlayerDelegate? PlayerRegistered;
        public event PlayerDelegate? PlayerLoggedOut;

        public delegate ValueTask PlayerAsyncDelegate(ITDSPlayer player);
        public event PlayerAsyncDelegate? PlayerLoggedOutBefore;

        public delegate void PlayerLobbyDelegate(ITDSPlayer player, ILobby lobby);
        public event PlayerLobbyDelegate? PlayerJoinedLobby;
        public event PlayerLobbyDelegate? PlayerLeftLobby;

        public delegate void CounterDelegate(ulong counter);
        public event CounterDelegate? Second;
        public event CounterDelegate? Minute;
        public event CounterDelegate? Hour;

        public delegate void EmptyDelegate();
        public event EmptyDelegate? MapsLoaded;
        public event EmptyDelegate? Update;

        public void OnUpdate()
        {
            Update?.Invoke();
        }

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

        public void OnPlayerLogin(ITDSPlayer tdsPlayer)
        {
            PlayerLoggedIn?.Invoke(tdsPlayer);
        }

        public void OnPlayerRegister(ITDSPlayer tdsPlayer)
        {
            PlayerRegistered?.Invoke(tdsPlayer);
        }

        public void OnMapsLoaded()
        {
            MapsLoaded?.Invoke();
        }

        internal void OnLobbyJoin(ITDSPlayer player, ILobby lobby)
        {
            PlayerJoinedLobby?.Invoke(player, lobby);
        }

        internal void OnLobbyLeave(ITDSPlayer player, ILobby lobby)
        {
            PlayerLeftLobby?.Invoke(player, lobby);
        }

        private ulong _hourCounter;
        internal void OnHour()
        {
            try
            {
                Hour?.Invoke(++_hourCounter);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        private ulong _minuteCounter;
        internal void OnMinute()
        {
            try
            {
                Minute?.Invoke(++_minuteCounter);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }

        private ulong _secondCounter;
        internal void OnSecond()
        {
            try
            {
                Second?.Invoke(++_secondCounter);
            }
            catch (Exception ex)
            {
                _loggingHandler.LogError(ex);
            }
        }
    }
}
