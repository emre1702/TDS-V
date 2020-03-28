using System;
using System.ComponentModel;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Handler.Entities.Utility;

namespace TDS_Server.Handler.Events
{
    public class EventsHandler
    {
        public EventsHandler()
        {
        }

        public delegate void PlayerDelegate(ITDSPlayer player);
        public event PlayerDelegate? PlayerConnected;
        public event PlayerDelegate? PlayerLoggedIn;

        public event PlayerDelegate? PlayerRegistered;
        public event PlayerDelegate? PlayerLoggedOut;
        public event PlayerDelegate? PlayerJoinedCustomMenuLobby;
        public event PlayerDelegate? PlayerLeftCustomMenuLobby;

        public delegate void IncomingConnectionDelegate(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel);
        public event IncomingConnectionDelegate? IncomingConnection;

        public delegate void ModPlayerDelegate(IPlayer player);
        public event ModPlayerDelegate? PlayerDisconnected;

        public AsyncValueTaskEvent<ITDSPlayer>? PlayerLoggedOutBefore;

        public delegate void PlayerLobbyDelegate(ITDSPlayer player, ILobby lobby);
        public event PlayerLobbyDelegate? PlayerJoinedLobby;
        public event PlayerLobbyDelegate? PlayerLeftLobby;

        public delegate void LobbyDelegate(ILobby lobby);
        public event LobbyDelegate? CustomLobbyCreated;
        public event LobbyDelegate? CustomLobbyRemoved;

        public delegate void CounterDelegate(ulong counter);
        public event CounterDelegate? Second;
        public event CounterDelegate? Minute;
        public event CounterDelegate? Hour;

        public delegate void EmptyDelegate();
        public event EmptyDelegate? MapsLoaded;
        public event EmptyDelegate? Update;
        public event EmptyDelegate? ResourceStop;
        public event EmptyDelegate? LoadedServerBans;

        public delegate void ErrorDelegate(Exception ex, ITDSPlayer? source = null, bool logToBonusBot = true);
        public event ErrorDelegate? Error;


        #region RAGE 
        public void OnUpdate()
        {
            Update?.Invoke();
        }

        public void OnPlayerConnected(ITDSPlayer tdsPlayer)
        {
            PlayerConnected?.Invoke(tdsPlayer);
        }

        public void OnPlayerDisconnected(IPlayer modPlayer)
        {
            PlayerDisconnected?.Invoke(modPlayer);
        }

        public void OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            IncomingConnection?.Invoke(ip, serial, socialClubName, socialClubId, cancel);
        }

        public void OnResourceStop()
        {
            ResourceStop?.Invoke();
        }
        #endregion


        #region Custom
        public void OnPlayerLogin(ITDSPlayer tdsPlayer)
        {
            PlayerLoggedIn?.Invoke(tdsPlayer);
        }

        public async Task OnPlayerLoggedOut(ITDSPlayer tdsPlayer)
        {
            var task = PlayerLoggedOutBefore?.InvokeAsync(tdsPlayer);
            if (task.HasValue)
                await task.Value;
            PlayerLoggedOut?.Invoke(tdsPlayer);
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

        internal void OnCustomLobbyMenuJoin(ITDSPlayer player)
        {
            PlayerJoinedCustomMenuLobby?.Invoke(player);
        }

        internal void OnCustomLobbyMenuLeave(ITDSPlayer player)
        {
            PlayerLeftCustomMenuLobby?.Invoke(player);
        }

        internal void OnCustomLobbyCreated(ILobby lobby)
        {
            CustomLobbyCreated?.Invoke(lobby);
        }

        internal void OnCustomLobbyRemoved(ILobby lobby)
        {
            CustomLobbyRemoved?.Invoke(lobby);
        }

        internal void OnLoadedServerBans()
        {
            LoadedServerBans?.Invoke();
        }
        #endregion


        #region Timer
        private ulong _hourCounter;
        internal void OnHour()
        {
            try
            {
                Hour?.Invoke(++_hourCounter);
            }
            catch (Exception ex)
            {
                Error?.Invoke(ex);
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
                Error?.Invoke(ex);
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
                Error?.Invoke(ex);
            }
        }
        #endregion

    }
}
