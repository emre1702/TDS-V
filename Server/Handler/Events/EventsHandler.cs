using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using System;
using System.Threading.Tasks;
using TDS_Server.Data.Interfaces.Entities;
using TDS_Server.Data.Interfaces.Entities.Gang;
using TDS_Server.Data.Interfaces.Entities.LobbySystem;
using TDS_Server.Data.Utility;
using TDS_Server.Database.Entity.Player;
using TDS_Shared.Data.Enums;
using TDS_Shared.Data.Models;
using TDS_Shared.Default;

namespace TDS_Server.Handler.Events
{
    public class EventsHandler
    {
        #region Fields

        public AsyncValueTaskEvent<ITDSPlayer>? PlayerLoggedOutBefore;
        public AsyncValueTaskEvent<(ITDSPlayer, Players)>? PlayerRegisteredBefore;

        private int _hourCounter;

        private int _minuteCounter;

        private int _secondCounter;

        #endregion Fields

        #region Constructors

        public EventsHandler()
        {
            //Todo: Add WeaponSwitch @ clientside
            Alt.OnClient<ITDSPlayer, WeaponHash, WeaponHash>(ToServerEvent.WeaponSwitch, OnPlayerWeaponSwitch);
            Alt.OnPlayerDead += OnPlayerDeath;
            Alt.OnColShape += OnColShape;
            AltAsync.OnPlayerDisconnect += OnPlayerDisconnected;
        }

        #endregion Constructors

        #region Delegates

        public delegate void CounterDelegate(int counter);

        public delegate void EmptyDelegate();

        public delegate void EntityDelegate(IEntity entity);

        public delegate void ErrorDelegate(Exception ex, ITDSPlayer? source = null, bool logToBonusBot = true);

        public delegate void GangHouseDelegate(IGangHouse house);

        public delegate void IncomingConnectionDelegate(string ip, ulong socialClubId, CancelEventArgs cancel);

        public delegate void LobbyDelegate(ILobby lobby);

        public delegate void PlayerDelegate(ITDSPlayer player);

        public delegate void PlayerGangDelegate(ITDSPlayer player, IGang gang);

        public delegate void PlayerLobbyDelegate(ITDSPlayer player, ILobby lobby);

        public delegate void TDSDbPlayerDelegate(ITDSPlayer player, Players dbPlayer);

        #endregion Delegates

        #region Events

        public event LobbyDelegate? CustomLobbyCreated;

        public event LobbyDelegate? CustomLobbyRemoved;

        public event EntityDelegate? EntityDeleted;

        public event ErrorDelegate? Error;

        public event GangHouseDelegate? GangHouseLoaded;

        public event CounterDelegate? Hour;

        public event EmptyDelegate? LoadedServerBans;

        public event LobbyDelegate? LobbyCreated;

        public event EmptyDelegate? MapsLoaded;

        public event CounterDelegate? Minute;

        public event PlayerDelegate? PlayerJoinedCustomMenuLobby;

        public event PlayerLobbyDelegate? PlayerJoinedLobby;

        public event PlayerDelegate? PlayerLeftCustomMenuLobby;

        public event PlayerGangDelegate? PlayerLeftGang;

        public event PlayerLobbyDelegate? PlayerLeftLobby;

        public event PlayerDelegate? PlayerLoggedIn;

        public event PlayerDelegate? PlayerLoggedOut;

        public event TDSDbPlayerDelegate? PlayerRegistered;

        public event PlayerDelegate? ReloadPlayerChar;

        public event EmptyDelegate? ResourceStop;

        public event CounterDelegate? Second;

        #endregion Events

        #region Methods

        //Todo: Implement this for all the single entity types
        public void OnEntityDeleted(IEntity entity)
        {
            EntityDeleted?.Invoke(entity);
        }

        public void OnMapsLoaded()
        {
            MapsLoaded?.Invoke();
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

        //Todo: Add custom spawn method, call OnPlayerSpawn yourself
        public void OnPlayerSpawn(ITDSPlayer player)
        {
            player.Lobby?.OnPlayerSpawn(player);
        }

        public void OnResourceStop()
        {
            ResourceStop?.Invoke();
        }

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

        internal void OnGangHouseLoaded(IGangHouse house)
        {
            GangHouseLoaded?.Invoke(house);
        }

        internal void OnGangLeave(ITDSPlayer player, IGang gang)
        {
            PlayerLeftGang?.Invoke(player, gang);
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

        private void OnColShape(IColShape colShape, IEntity targetEntity, bool state)
        {
            if (targetEntity is ITDSPlayer player)
            {
                if (state)
                    OnPlayerEnterColShape((ITDSColShape)colShape, player);
            }
        }

        private void OnPlayerDeath(IPlayer modPlayer, IEntity killer, uint weapon)
        {
            var player = (ITDSPlayer)modPlayer;
            player.Lobby?.OnPlayerDeath(player, killer, weapon);
        }

        private async Task OnPlayerDisconnected(IPlayer modPlayer, string reason)
        {
            var player = (ITDSPlayer)modPlayer;
            if (player.LoggedIn)
                await OnPlayerLoggedOut(player);
        }

        private void OnPlayerEnterColShape(ITDSColShape colShape, ITDSPlayer player)
        {
            player.Lobby?.OnPlayerEnterColShape(colShape, player);
        }

        private async Task OnPlayerLoggedOut(ITDSPlayer tdsPlayer)
        {
            var task = PlayerLoggedOutBefore?.InvokeAsync(tdsPlayer);
            if (task.HasValue)
                await task.Value;
            await AltAsync.Do(() =>
            {
                PlayerLoggedOut?.Invoke(tdsPlayer);
                tdsPlayer.Lobby?.OnPlayerLoggedOut(tdsPlayer);
            });
            await tdsPlayer.ExecuteForDBAsync(async dbContext =>
            {
                await dbContext.DisposeAsync();
            });
        }

        private void OnPlayerWeaponSwitch(ITDSPlayer player, WeaponHash previousWeapon, WeaponHash newWeapon)
        {
            if (!(player.Lobby is IFightLobby fightLobby))
                return;

            player.OnPlayerWeaponSwitch(previousWeapon, newWeapon);
            fightLobby.OnPlayerWeaponSwitch(player, previousWeapon, newWeapon);
        }

        #endregion Methods

        /*public void OnPlayerEnterVehicle(ITDSPlayer tdsPlayer, ITDSVehicle vehicle, sbyte seatId)
        {
            tdsPlayer.Lobby?.OnPlayerEnterVehicle(tdsPlayer, vehicle, seatId);
        }

        public void OnPlayerExitVehicle(ITDSPlayer tdsPlayer, ITDSVehicle vehicle)
        {
            tdsPlayer.Lobby?.OnPlayerExitVehicle(tdsPlayer, vehicle);
        }*/
    }
}
