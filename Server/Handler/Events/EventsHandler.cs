using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Account;
using TDS_Server.Handler.Player;

namespace TDS_Server.Handler.Events
{
    public class EventsHandler
    {
        private readonly TDSPlayerHandler _tdsPlayerHandler;

        private readonly ILoggingHandler _loggingHandler;
        private readonly BansHandler _bansHandler;
        private readonly LobbiesHandler _lobbiesHandler;
        private readonly ResourceStopHandler _resourceStopHandler;

        public EventsHandler(TDSPlayerHandler tdsPlayerHandler, ILoggingHandler loggingHandler, BansHandler bansHandler, LobbiesHandler lobbiesHandler, ResourceStopHandler resourceStopHandler)
            => (_tdsPlayerHandler, _loggingHandler, _bansHandler, _lobbiesHandler, _resourceStopHandler) 
            = (tdsPlayerHandler, loggingHandler, bansHandler, lobbiesHandler, resourceStopHandler);

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
            var tdsPlayer = _tdsPlayerHandler.Get(modPlayer);
            PlayerConnected?.Invoke(tdsPlayer);
        }

        public void OnPlayerDisconnected(IPlayer modPlayer)
        {
            var tdsPlayer = _tdsPlayerHandler.Get(modPlayer);
            PlayerDisconnected?.Invoke(tdsPlayer);
        }

        public void OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            var ban = _bansHandler.GetServerBan(null, ip, serial, socialClubName, socialClubId, true);
            if (ban is { })
                cancel.Cancel = true;
        }

        public void OnResourceStop()
        {
            _resourceStopHandler?.OnResourceStop();
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
