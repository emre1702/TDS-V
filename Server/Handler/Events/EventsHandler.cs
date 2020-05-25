using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces;
using TDS_Server.Data.Interfaces.ModAPI;
using TDS_Server.Data.Interfaces.ModAPI.ColShape;
using TDS_Server.Data.Interfaces.ModAPI.Player;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Server.Handler.Entities.LobbySystem;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;

namespace TDS_Server.Handler.Events
{
    public class EventsHandler
    {
        #region Public Fields

        public AsyncValueTaskEvent<ITDSPlayer>? PlayerLoggedOutBefore;
        public AsyncValueTaskEvent<(ITDSPlayer, Players)>? PlayerRegisteredBefore;

        #endregion Public Fields

        #region Private Fields

        private readonly IModAPI _modAPI;

        private int _hourCounter;

        private int _minuteCounter;

        private int _secondCounter;

        #endregion Private Fields

        #region Public Constructors

        public EventsHandler(IModAPI modAPI)
        {
            _modAPI = modAPI;
        }

        #endregion Public Constructors

        #region Public Delegates

        public delegate void CounterDelegate(int counter);

        public delegate void EmptyDelegate();

        public delegate void ErrorDelegate(Exception ex, ITDSPlayer? source = null, bool logToBonusBot = true);

        public delegate void IncomingConnectionDelegate(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel);

        public delegate void LobbyDelegate(ILobby lobby);

        public delegate void ModPlayerDelegate(IPlayer player);

        public delegate void PlayerDelegate(ITDSPlayer player);

        public delegate void PlayerLobbyDelegate(ITDSPlayer player, ILobby lobby);

        public delegate void TDSDbPlayerDelegate(ITDSPlayer player, Players dbPlayer);

        #endregion Public Delegates

        #region Public Events

        public event LobbyDelegate? CustomLobbyCreated;

        public event LobbyDelegate? CustomLobbyRemoved;

        public event ErrorDelegate? Error;

        public event CounterDelegate? Hour;

        public event IncomingConnectionDelegate? IncomingConnection;

        public event EmptyDelegate? LoadedServerBans;

        public event LobbyDelegate? LobbyCreated;

        public event EmptyDelegate? MapsLoaded;

        public event CounterDelegate? Minute;

        public event ModPlayerDelegate? PlayerConnected;

        public event ModPlayerDelegate? PlayerDisconnected;

        public event PlayerDelegate? PlayerJoinedCustomMenuLobby;

        public event PlayerLobbyDelegate? PlayerJoinedLobby;

        public event PlayerDelegate? PlayerLeftCustomMenuLobby;

        public event PlayerLobbyDelegate? PlayerLeftLobby;

        public event PlayerDelegate? PlayerLoggedIn;

        public event PlayerDelegate? PlayerLoggedOut;

        public event TDSDbPlayerDelegate? PlayerRegistered;

        public event PlayerDelegate? ReloadPlayerChar;

        public event EmptyDelegate? ResourceStop;

        public event CounterDelegate? Second;

        public event EmptyDelegate? Update;

        #endregion Public Events

        #region Public Methods

        public void OnIncomingConnection(string ip, string serial, string socialClubName, ulong socialClubId, CancelEventArgs cancel)
        {
            IncomingConnection?.Invoke(ip, serial, socialClubName, socialClubId, cancel);
        }

        public void OnMapsLoaded()
        {
            MapsLoaded?.Invoke();
        }

        public void OnPlayerConnected(IPlayer modPlayer)
        {
            PlayerConnected?.Invoke(modPlayer);
        }

        public void OnPlayerDeath(ITDSPlayer player, ITDSPlayer killer, uint reason)
        {
            player.Lobby?.OnPlayerDeath(player, killer, reason);
        }

        public void OnPlayerDisconnected(IPlayer modPlayer)
        {
            PlayerDisconnected?.Invoke(modPlayer);
        }

        public void OnPlayerEnterColshape(IColShape colshape, ITDSPlayer player)
        {
            player.Lobby?.OnPlayerEnterColshape(colshape, player);
        }

        public async Task OnPlayerLoggedOut(ITDSPlayer tdsPlayer)
        {
            var task = PlayerLoggedOutBefore?.InvokeAsync(tdsPlayer);
            if (task.HasValue)
                await task.Value;
            _modAPI.Thread.RunInMainThread(() =>
            {
                PlayerLoggedOut?.Invoke(tdsPlayer);
                tdsPlayer.Lobby?.OnPlayerLoggedOut(tdsPlayer);
            });
        }

        public void OnPlayerLogin(ITDSPlayer tdsPlayer)
        {
            PlayerLoggedIn?.Invoke(tdsPlayer);
        }

        public async void OnPlayerRegister(ITDSPlayer player, Players dbPlayer)
        {
            var task = PlayerRegisteredBefore?.InvokeAsync((player, dbPlayer));
            if (task.HasValue)
                await task.Value;
            PlayerRegistered?.Invoke(player, dbPlayer);
        }

        public void OnPlayerSpawn(ITDSPlayer player)
        {
            player.Lobby?.OnPlayerSpawn(player);
        }

        public void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash previousWeapon, WeaponHash newWeapon)
        {
            if (!(player.Lobby is FightLobby fightLobby))
                return;

            fightLobby.OnPlayerWeaponSwitch(player, previousWeapon, newWeapon);
        }

        public void OnResourceStop()
        {
            ResourceStop?.Invoke();
        }

        public void OnUpdate()
        {
            Update?.Invoke();
        }

        #endregion Public Methods

        /*public void OnPlayerEnterVehicle(ITDSPlayer tdsPlayer, ITDSVehicle vehicle, sbyte seatId)
        {
            tdsPlayer.Lobby?.OnPlayerEnterVehicle(tdsPlayer, vehicle, seatId);
        }

        public void OnPlayerExitVehicle(ITDSPlayer tdsPlayer, ITDSVehicle vehicle)
        {
            tdsPlayer.Lobby?.OnPlayerExitVehicle(tdsPlayer, vehicle);
        }*/

        #region Internal Methods

        internal void OnCustomLobbyCreated(ILobby lobby)
        {
            CustomLobbyCreated?.Invoke(lobby);
        }

        internal void OnCustomLobbyMenuJoin(ITDSPlayer player)
        {
            PlayerJoinedCustomMenuLobby?.Invoke(player);
        }

        internal void OnCustomLobbyMenuLeave(ITDSPlayer player)
        {
            PlayerLeftCustomMenuLobby?.Invoke(player);
        }

        internal void OnCustomLobbyRemoved(ILobby lobby)
        {
            CustomLobbyRemoved?.Invoke(lobby);
        }

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

        internal void OnLoadedServerBans()
        {
            LoadedServerBans?.Invoke();
        }

        internal void OnLobbyCreated(ILobby lobby)
        {
            LobbyCreated?.Invoke(lobby);
        }

        internal void OnLobbyJoin(ITDSPlayer player, ILobby lobby)
        {
            PlayerJoinedLobby?.Invoke(player, lobby);
        }

        internal void OnLobbyLeave(ITDSPlayer player, ILobby lobby)
        {
            PlayerLeftLobby?.Invoke(player, lobby);
        }

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

        internal void OnReloadPlayerChar(ITDSPlayer player)
        {
            ReloadPlayerChar?.Invoke(player);
        }

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

        #endregion Internal Methods
    }
}
